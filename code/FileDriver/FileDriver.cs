/*
**  File Name:		FileDriver.cs
**
**  Functional Description:
**
**      This class is the implementor of IDriver interface and this class provides support 
**      for handling ofac requests through file folders
**	
**
**	Author:	Lakshman Ramakrishnan
**  Facility	    WEI
**  Creation Date:  12/30/2010
**
*******************************************************************************
**                                                                           **
**      COPYRIGHT                                                            **
**                                                                           **
** (C) Copyright 2010                                                        **
** Telavance, inc                                                            **
**                                                                           **
** This software is furnished under a license for use only on a single       **
** computer system and may be copied only with the inclusion of the  above   **
** copyright notice. This software or any other copies thereof, may not be   **
** provided or otherwise made available to any other person  except for use  **
** on such system and to one who agrees to these license terms. title and    **
** ownership of the software shall at all times remain in Telavance,inc      **
** Inc.                                                                      **
**                                                                           **
** The information in this software is subject to change without notice and  **
** should not be construed as a commitment by Telavance, Inc.	             **
**									     									 **
*******************************************************************************
 									    
*******************************************************************************
 		                    Maintenance History				    
-------------|----------|------------------------------------------------------
    Date     |	Person  |  Description of Modification			    
-------------|----------|------------------------------------------------------

12/30/2010       RL        Inital version
12/01/2012                 Changes to the code to include review folder
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telavance.AdvantageSuite.Wei.WeiCommon;
using Telavance.AdvantageSuite.Wei.WeiService;
using System.IO;
using System.Threading;
using System.Security.Principal;

namespace Telavance.AdvantageSuite.Wei.FileDriver
{
    public class FileDriver : IDriver
    {
        private int _interfaceId;
        private RequestManager _manager;
        private Config config;
        private DBUtil _dbUtils;

        private FileSystemWatcher _inputFileWatcher;
        private FileSystemWatcher _reviewFileWatcher;
        private FileSystemWatcher _ofacOkFileWatcher;
        private FileSystemWatcher _ofacConfirmFileWatcher;

        private const int DEFAULT_WAIT_TIME = 1000; //1 sec
        private int _waitTime = DEFAULT_WAIT_TIME;

        private static String timeFormat = "yyyy-MM-dd HH-mm-ss";

        private DriverStatus status = DriverStatus.NotRunning;
        private readonly object syncLock = new object();
        private Boolean pendingStop = false;
        private int currentMessagesToBeProcessed = 0;
        private int totalMessagesProcessed = 0;
        private DateTime lastProcessedMessageTime = DateTime.MinValue;

        private const int REQUEST_RETRY = 10;

        public void initialize(int interfaceId, String configStr, RequestManager manager, DBUtil dbUtils)
        {
            _interfaceId = interfaceId;

            _manager = manager;
            _dbUtils = dbUtils;
            config = Config.getConfig(configStr);
        }

        public void start()
        {
            lock (syncLock)
            {
                LogUtil.logInfo("config:" + config.ToString());

                if (!config.isValid())
                    throw new Exception("Config for the driver is incomplete");

                try
                {
                    if (config.waitTime != null)
                        _waitTime = Convert.ToInt32(config.waitTime);
                }
                catch (Exception e)
                {
                    LogUtil.logInfo("Error using the waitTime of " + config.waitTime + ". Defaulting to " + DEFAULT_WAIT_TIME);
                    _waitTime = DEFAULT_WAIT_TIME;
                }
                initInput();
                initConfirm();
                initOk();
                if (!pendingStop)
                    status = DriverStatus.Running;
            }
        }
        
        public void stop()
        {
            if (status == DriverStatus.Running || status == DriverStatus.Error)
            {
                pendingStop = true;
                status = DriverStatus.Shuttingdown;
                Thread threadStop = new Thread(stopThread);
                threadStop.Start();
            }
        }

        public void stopThread()
        {
            _inputFileWatcher.EnableRaisingEvents = false;
            _reviewFileWatcher.EnableRaisingEvents = false;
            _ofacOkFileWatcher.EnableRaisingEvents = false;
            _ofacConfirmFileWatcher.EnableRaisingEvents = false;

            for (int counter = 0; counter < 10; counter++)
            {
                lock (syncLock)
                {
                    if (currentMessagesToBeProcessed <= 0)
                    {
                        status = DriverStatus.NotRunning;
                        return;
                    }
                }

                Thread.Sleep(5000);
            }

            status = DriverStatus.Error;

        }
        
        public DriverStatus getStatus()
        {
            return status;
        }

        public void initInput()
        {

            _inputFileWatcher = new FileSystemWatcher(config.inputFolder, config.inputFilePattern);

            while (!pendingStop && !_inputFileWatcher.EnableRaisingEvents)
            {
                try
                {

                    _inputFileWatcher.Created += new FileSystemEventHandler(inputFileWatcher_Created);
                   
                    _inputFileWatcher.Error += new ErrorEventHandler(inputFileWatcher_Error);
                    _inputFileWatcher.EnableRaisingEvents = true;
                }
                catch (Exception e)
                {
                    LogUtil.log("Error when initializing filewatcher for input folder:" + config.inputFolder, e);
                    System.Threading.Thread.Sleep(5000);
                }
            }
            if (!pendingStop)
            {
                Thread workerThread = new Thread(this.processPreExistingInputFile);
                workerThread.Start();
            }

        }

        public void initOk()
        {
            _ofacOkFileWatcher = new FileSystemWatcher(config.ofacOkFolder, @"*.*");
            while (!pendingStop && !_ofacOkFileWatcher.EnableRaisingEvents)
            {
                try
                {

                    _ofacOkFileWatcher.Created += new FileSystemEventHandler(_ofacOkFileWatcher_Created);
                    _ofacOkFileWatcher.Error += new ErrorEventHandler(_ofacOkFileWatcher_Error);
                    _ofacOkFileWatcher.EnableRaisingEvents = true;
                }
                catch (Exception e)
                {
                    LogUtil.log("Error when initializing filewatcher for ofacOk folder:" + config.ofacOkFolder, e);
                    System.Threading.Thread.Sleep(5000);
                }
            }

            if (!pendingStop)
            {
                Thread workerThread = new Thread(this.processPreExistingOkFile);
                workerThread.Start();
            }

        }
        public void initConfirm()
        {
            _ofacConfirmFileWatcher = new FileSystemWatcher(config.ofacConfirmFolder, @"*.*");
            while (!pendingStop && !_ofacConfirmFileWatcher.EnableRaisingEvents)
            {
                try
                {

                    _ofacConfirmFileWatcher.Created += new FileSystemEventHandler(_ofacConfirmFileWatcher_Created);
                    _ofacConfirmFileWatcher.Error += new ErrorEventHandler(_ofacConfirmFileWatcher_Error);
                    _ofacConfirmFileWatcher.EnableRaisingEvents = true;

                }
                catch (Exception e)
                {
                    LogUtil.log("Error when initializing filewatcher for ofacconfirm folder:" + config.ofacConfirmFolder, e);
                    System.Threading.Thread.Sleep(5000);
                }
            }
            if (!pendingStop)
            {
                Thread workerThread = new Thread(this.processPreExistingConfirmFile);
                workerThread.Start();
            }
        }


        void processPreExistingInputFile()
        {
            String[] inputFilesPresent = Directory.GetFiles(config.inputFolder, config.inputFilePattern);
            foreach (string fileName in inputFilesPresent)
            {
                if (pendingStop)
                    return;
                processFile(fileName, 0);
            }
        }

        void processPreExistingOkFile()
        {
            String[] okFilesPresent = Directory.GetFiles(config.ofacOkFolder);

            foreach (string fileName in okFilesPresent)
            {
                if (pendingStop)
                    return;
                FileInfo fi = new FileInfo(fileName);
                processOfacResponse(fi.Name, fileName, OfacStatus.OK);
            }
        }

        void processPreExistingConfirmFile()
        {
            String[] confirmFilesPresent = Directory.GetFiles(config.ofacConfirmFolder);

            foreach (string fileName in confirmFilesPresent)
            {
                if (pendingStop)
                    return;
                FileInfo fi = new FileInfo(fileName);
                processOfacResponse(fi.Name, fileName, OfacStatus.Confirm);
            }
        }

        void _ofacConfirmFileWatcher_Error(object source, ErrorEventArgs e)
        {
            Exception watchException = e.GetException();
            LogUtil.logInfo("Error when watching filewatcher for ofacConfirm folder:" + config.ofacConfirmFolder, watchException);
            initConfirm();
        }

        void _ofacConfirmFileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(_waitTime);
                processOfacResponse(e.Name, e.FullPath, OfacStatus.Confirm);
            }
            catch (Exception ex)
            {
                LogUtil.log("Unknown error when processing confirm file:" + e.FullPath, ex);
            }
        }

        void _ofacOkFileWatcher_Error(object source, ErrorEventArgs e)
        {
            Exception watchException = e.GetException();
            LogUtil.logInfo("Error when watching filewatcher for ofacOk folder:" + config.ofacOkFolder, watchException);
            initOk();
        }

        void _ofacOkFileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(_waitTime);
                processOfacResponse(e.Name, e.FullPath, OfacStatus.OK);
            }
            catch (Exception ex)
            {
                LogUtil.log("Unknown error when processing ok file:" + e.FullPath, ex);
            }
        }

        void inputFileWatcher_Error(object source, ErrorEventArgs e)
        {
            Exception watchException = e.GetException();
            LogUtil.logInfo("Error when watching filewatcher for input folder:" + config.inputFolder, watchException);
            initInput();
        }

        void inputFileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(_waitTime);
                processFile(e.FullPath, 0);
            }
            catch (Exception ex)
            {
                LogUtil.log("Unknown error when processing input file:" + e.FullPath, ex);
            }
        }

        public void processOfacResponse(String name, String filename, OfacStatus ofacStatus)
        {
            lock (syncLock)
            {
                if (pendingStop)
                    return;
                currentMessagesToBeProcessed++;
            }
            try
            {
                LogUtil.logInfo("Processing Response file :" + filename);
                String message = ReadFile(filename);
                if (message == null)
                    return;

                int index = name.LastIndexOf('.');
                String fileRequestId = name.Substring(0, index);
                index = fileRequestId.IndexOf('-');
                if (index != -1)
                {
                    fileRequestId = fileRequestId.Substring(0, index);
                }
                //string extension = name.Substring(index + 1);


                int requestId = Int32.Parse(fileRequestId);

                _dbUtils.addOfacResponse(requestId, name, message, ofacStatus);
                File.Delete(filename);
                _manager.processResponse(requestId, _interfaceId, name, message, ofacStatus);
                updateStatistics();
            }
            catch (Exception e)
            {
                LogUtil.log("Error in processOfacResponse for file:" + filename, e);

            }
            finally
            {
                lock (syncLock)
                {
                    currentMessagesToBeProcessed--;
                }
            }

        }

        private void updateStatistics()
        {
            lock (syncLock)
            {
                totalMessagesProcessed++;
                lastProcessedMessageTime = DateTime.Now;
            }
        }

        public void processFile(String filename, int retryCount)
        {
            lock (syncLock)
            {
                if (pendingStop)
                    return;
                currentMessagesToBeProcessed++;
            }
            try
            {
                LogUtil.logInfo("Processing input file :" + filename);
                String message = ReadFile(filename);

                if (message == null)
                    return;

                Request request = new Request();
                request.Name = filename;
                request.MessageBody = message;
                request.InterfaceId = _interfaceId;
                WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
                if (windowsIdentity != null) request.CreateOper = windowsIdentity.Name;

                _dbUtils.addRequest(request);
                File.Delete(filename);

                _manager.process(request);
                updateStatistics();
            }
            catch (Exception e)
            {
                LogUtil.log("Error in processFile for file:" + filename, e);

                if (retryCount<REQUEST_RETRY && File.Exists(filename))
                {
                    LogUtil.logInfo("Retrying file:" + filename);
                    processFile(filename, (retryCount+1));
                    //cannot add the request to db...
                }

                // if file exists after the retrys then we stop the driver
                if (File.Exists(filename))
                {
                    stop();
                }

                

            }
            finally
            {
                lock (syncLock)
                {
                    currentMessagesToBeProcessed--;
                }
            }
        }

        internal static string ReadFile(string filename)
        {
            StreamReader sr = null;
            if (File.Exists(filename) == false)
                throw new FileNotFoundException(filename + " not found");
            try
            {
                sr = new StreamReader(filename);
                var msg = sr.ReadToEnd();
                return msg;
            }
            catch (Exception e)
            {
                LogUtil.log("Error reading file " + filename, e);
                return null;
            }

            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        public bool sendForOfacCheck(Request request)
        {
            bool retValue = false;
            try
            {
                int index = request.Name.LastIndexOf('\\');
                string filename = request.RequestId + "-" + request.Name.Substring(index + 1);
                StreamWriter sw = new StreamWriter(new FileStream(config.ofacInputFolder + "\\" + filename, FileMode.CreateNew));
                sw.Write(request.TranslatedMessage);
                sw.Close();
                retValue = true;
               
            }
            catch (IOException e)
            {
                LogUtil.log("Cannot send requestid:" + request.RequestId + " for ofac check", e);
            }

            return retValue;
        }
        private StreamWriter getResponseStreamWriter(String outputDirectory, string filename, string extension)
        {
            LogUtil.logInfo("sending Response file :" + filename);
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(new FileStream(outputDirectory + "\\" + filename + "." + extension, FileMode.CreateNew));
            }
            catch (IOException e)
            {
                filename = filename + '-' + DateTime.Now.ToString(timeFormat);
                LogUtil.logInfo("Error sending response file :" + filename + ". So trying with filename:" + filename);
                sw = getResponseStreamWriter(outputDirectory, filename, extension);
            }
            return sw;
        }

        public bool sendResponse(Request request, string ofacResponseIdentifier)
        {
            bool retValue = false;



            try
            {
                string outputDirectory = config.okFolder;
                if (request.OfacStatus == OfacStatus.Confirm)
                    outputDirectory = config.confirmFolder;
                FileInfo fi = new FileInfo(request.Name);
                int index = fi.Name.LastIndexOf('.');
                String filename = fi.Name.Substring(0, index);
                index = ofacResponseIdentifier.LastIndexOf(".");
                string extension = ofacResponseIdentifier.Substring(index + 1);

                StreamWriter sw = getResponseStreamWriter(outputDirectory, filename, extension);
                sw.Write(request.ResponseMessage);
                sw.Close();
                retValue = true;
            }
            catch (Exception e)
            {
                LogUtil.log("Cannot send response for requestid:" + request.RequestId, e);
                throw (e);
            }

            return retValue;
        }

        public bool moveToError(Request request)
        {
            bool retValue = false;



            try
            {

                string outputDirectory = config.errorFolder;

                FileInfo fi = new FileInfo(request.Name);
                int index = fi.Name.LastIndexOf('.');
                String filename = fi.Name.Substring(0, index);
                string extension = fi.Name.Substring(index + 1);

                StreamWriter sw = getResponseStreamWriter(outputDirectory, filename, extension);
                sw.Write(request.MessageBody);
                sw.Close();
                retValue = true;
            }
            catch (Exception e)
            {
                LogUtil.log("Cannot move the file to Error folder for requestid:" + request.RequestId, e);
                throw (e);
            }

            return retValue;

        }
        //added to move the message to review folder
        public bool moveToReview(Request request)
        {
            bool retValue = false;



            try
            {

                string outputDirectory = config.reviewFolder;

                FileInfo fi = new FileInfo(request.Name);
                int index = fi.Name.LastIndexOf('.');
                String filename = fi.Name.Substring(0, index);
                string extension = fi.Name.Substring(index + 1);

                StreamWriter sw = getResponseStreamWriter(outputDirectory, filename, extension);
                sw.Write(request.MessageBody);
                sw.Close();
                retValue = true;
                File.Delete(filename);
            }
            catch (Exception e)
            {
                LogUtil.log("Cannot move the message to Review folder for requestid:" + request.RequestId, e);
                throw (e);
            }

            return retValue;

        }
        public string getStatistics()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(" Status:");
            builder.Append(status.ToString());
            builder.Append("\n Total messages processed:");
            builder.Append(totalMessagesProcessed);
            builder.Append("\n Last processed message time:");
            if (lastProcessedMessageTime == DateTime.MinValue)
                builder.Append("None");
            else
                builder.Append(lastProcessedMessageTime);

            builder.Append("\n current messages in process:");
            builder.Append(currentMessagesToBeProcessed);

            return builder.ToString();
        }

        public bool shouldMoveToError()
        {
            return config.shouldMoveToError;
        }

        public bool shouldMoveToReview()
        {
            return config.shouldMoveToReview;
        }
    }
}

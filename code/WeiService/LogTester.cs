using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Telavance.AdvantageSuite.Wei.WeiCommon;

namespace Telavance.AdvantageSuite.Wei.WeiService
{
    public static class LogTester
    {
        static void Main()
        {
            Console.WriteLine("started");
            for (int i = 0; i < 25; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(logwrite), i);
            }

            Thread.Sleep(1000000);
        }

        static void logwrite(object objThreadId)
        {
            int threadId = (int)objThreadId;
            for (int i = 0; i < 10000; i++)
            {
                LogUtil.logError("Threadid:" + threadId + ". counter:" + i);

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace Telavance.AdvantageSuite.Wei.ServiceManager
{
    public class DbUtil
    {
        private Database _weidb;

        public DbUtil(/*[Dependency("WeiDB")]*/Database db)
        {
            _weidb = db;
        }
        public DataSet GetInterfaces()
        {

            try
            {
                string sStoreProcName = "Wei_getInterfaces";
                DbCommand dbCommand = _weidb.GetStoredProcCommand(sStoreProcName);

                return _weidb.ExecuteDataSet(dbCommand);
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

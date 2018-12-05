using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using HiTuanReportTool.Models;

namespace HiTuanReportTool.Utility
{
    public class DBUtility
    {
        static string connString = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.ConnectionStrings["SQLiteDB"].ConnectionString;
        SQLiteHelper sh = new SQLiteHelper(connString);

        public DataTable GetDataTable(string commandText, SQLiteParameter[] paraList)
        {
            return sh.ExecuteDataTable(commandText, paraList);
        }

        public void SetSystemInfo(AppInfo appInfo)
        {
            
        }
    }
}

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

        #region get data
        public DataTable GetDataTable(string commandText, SQLiteParameter[] paraList)
        {
            return sh.ExecuteDataTable(commandText, paraList);
        }

        public void SetSystemInfo(AppInfo appInfo)
        {

        }
        #endregion

        #region insert data to DB
        public int InsertProducts(List<Product> lists)
        {
            if (lists.Count() > 0)
            {
                StringBuilder sbSqlText = new StringBuilder("insert into Products(PName,Combinition,Unit,DirectPrice,DistributionPrice,GroupPrice,ProductDate,Remarks)");
                List<SQLiteParameter> paras = new List<SQLiteParameter>();
                string sqlText = string.Empty;
                for (int i = 0; i < lists.Count(); i++)
                {
                    sbSqlText.Append("select ");
                    sbSqlText.Append("@PName" + i + ",");
                    sbSqlText.Append("@Combinition" + i + ",");
                    sbSqlText.Append("@Unit" + i + ",");
                    sbSqlText.Append("@DirectPrice" + i + ",");
                    sbSqlText.Append("@DistributionPrice" + i + ",");
                    sbSqlText.Append("@GroupPrice" + i + ",");
                    sbSqlText.Append("@ProductDate" + i + ",");
                    sbSqlText.AppendLine("@Remarks" + i);
                    sbSqlText.AppendLine("union");


                    paras.Add(new SQLiteParameter("@PName" + i, DbType.String, 200));
                    paras.Add(new SQLiteParameter("@Combinition" + i, DbType.Int32, 0));
                    paras.Add(new SQLiteParameter("@Unit" + i, DbType.String, 50));
                    paras.Add(new SQLiteParameter("@DirectPrice" + i, DbType.Currency, 0));
                    paras.Add(new SQLiteParameter("@DistributionPrice" + i, DbType.Currency, 0));
                    paras.Add(new SQLiteParameter("@GroupPrice" + i, DbType.Currency, 0));
                    paras.Add(new SQLiteParameter("@ProductDate" + i, DbType.Date, 0));
                    paras.Add(new SQLiteParameter("@Remarks" + i, DbType.String, 2000));

                    paras[i * 8 + 0].Value = lists[i].PName;
                    paras[i * 8 + 1].Value = lists[i].Combinition;
                    paras[i * 8 + 2].Value = lists[i].Unit;
                    paras[i * 8 + 3].Value = lists[i].DirectPrice;
                    paras[i * 8 + 4].Value = lists[i].DistributionPrice;
                    paras[i * 8 + 5].Value = lists[i].GroupPrice;
                    paras[i * 8 + 6].Value = Convert.ToDateTime(lists[i].ProductDate.ToString());
                    paras[i * 8 + 7].Value = lists[i].Remarks;
                }
                sqlText = sbSqlText.ToString();
                sqlText = sqlText.Substring(0, sqlText.LastIndexOf("union"));
                return sh.ExecuteNonQuery(sqlText, paras); ;
            }
            else
                return 0;
        }
        #endregion
    }
}

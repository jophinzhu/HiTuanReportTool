using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiTuanReportTool.Models;
using HiTuanReportTool.Utility;

namespace HiTuanReportTool.ViewModels
{
    public class MainViewModel
    {
        private DBUtility ut = new DBUtility();
        #region constructors
        private bool isExpired;
        public bool IsExpired
        {
            get { return isExpired; }
            set { isExpired = value; }
        }

        private Vendor[] vendor;
        public Vendor[] Vendor
        {
            get { return vendor; }
            set { vendor = value; }
        }

        private AppInfo appInfo;
        public AppInfo AppInfo
        {
            get { return appInfo; }
            set { appInfo = value; }
        }
        #endregion
        public MainViewModel()
        {
            AppInfo = new AppInfo();
            IsExpired = false;
            GetApplicationInformation();
        }

        #region Private Methods
        private void GetApplicationInformation()
        {
            DataTable dt = new DataTable();
            dt = ut.GetDataTable("select * from AppInfo", null);
            if (dt != null)
            {
                DateTime dtExpired = new DateTime();
                AppInfo.MacAddress = dt.Rows[0]["MacAddress"].ToString();
                AppInfo.ProductKey = dt.Rows[0]["ProductKey"].ToString();
                dtExpired = Convert.ToDateTime(dt.Rows[0]["ExpiredDate"].ToString());
                AppInfo.ExpiredDate = dtExpired.ToString("yyyy年MM月dd日");
                if (dtExpired < DateTime.Now)
                    IsExpired = true;
            }
            else
            {
                IsExpired = true;
            }
        }
        #endregion
    }
}

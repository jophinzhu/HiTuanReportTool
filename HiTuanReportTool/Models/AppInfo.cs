using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTuanReportTool.Models
{
    public class AppInfo
    {
        private string macAddress;
        private string productKey;
        private string expiredDate;

        public string MacAddress
        {
            get { return macAddress; }
            set { macAddress = value; }
        }
        public string ProductKey
        {
            get { return productKey; }
            set { productKey = value; }
        }
        public string ExpiredDate
        {
            get { return expiredDate; }
            set { expiredDate = value; }
        }
    }
}

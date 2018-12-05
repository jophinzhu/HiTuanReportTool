using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTuanReportTool.Models
{
    public class Product
    {
        private int pId;

        public int PId
        {
            get { return pId; }
            set { pId = value; }
        }
        private string pName;

        public string PName
        {
            get { return pName; }
            set { pName = value; }
        }
        private int combinition;

        public int Combinition
        {
            get { return combinition; }
            set { combinition = value; }
        }
        private string unit;

        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        private decimal directPrice;

        public decimal DirectPrice
        {
            get { return directPrice; }
            set { directPrice = value; }
        }
        private decimal distributionPrice;

        public decimal DistributionPrice
        {
            get { return distributionPrice; }
            set { distributionPrice = value; }
        }
        private decimal groupPrice;

        public decimal GroupPrice
        {
            get { return groupPrice; }
            set { groupPrice = value; }
        }
        private DateTime productTime;

        public DateTime ProductTime
        {
            get { return productTime; }
            set { productTime = value; }
        }
        private string remarks;

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}

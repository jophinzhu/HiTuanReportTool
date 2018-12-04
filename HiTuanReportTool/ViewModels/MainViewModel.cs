﻿using System;
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

        private Vendor[] vendor;
        public Vendor[] Vendor
        {
            get { return vendor; }
            set { vendor = value; }
        }

        private DataView dvBinding;
        public DataView DVBinding
        {
            get { return dvBinding; }
            set { dvBinding = value; }
        }


        private DataSet dsBinding;
        public DataSet DSBinding
        {
            get { return dsBinding; }
            set { dsBinding = value; }
        }

        public MainViewModel()
        {
            DSBinding = ut.GetDataSet("select * from Vendors", null);
            DVBinding = DSBinding.Tables[0].DefaultView;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiTuanReportTool.Models
{
    public class Vendor
    {
        private int _vId;
        private string _vName;
        private int _superiorId;
        private bool _hasDirectPrice;
        private string _text;

        public int VId
        {
            get { return _vId; }
            set { _vId = value; }
        }

        public string VName
        {
            get { return _vName; }
            set { _vName = value; }
        }

        public int SuperiorId
        {
            get { return _superiorId; }
            set { _superiorId = value; }
        }

        public bool HasDirectPrice
        {
            get { return _hasDirectPrice; }
            set { _hasDirectPrice = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}

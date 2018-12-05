using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using HiTuanReportTool.ViewModels;
using HiTuanReportTool.Models;

namespace HiTuanReportTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel;
        public MainWindow()
        {
            mainViewModel = new MainViewModel();
            InitializeComponent();
            this.DataContext = mainViewModel;
        }

        private void btnIdentity_Click_1(object sender, RoutedEventArgs e)
        {
            string strText = this.txtDataIdentify.Text.ToString();
            if (!string.IsNullOrEmpty(strText) && strText.IndexOf("①") > 0)
            {
                ///单个团品信息字符串
                string strSingleProduct = "";
                ///单个团品字符串在整个识别信息中的起始位置
                int startIndex;
                ///下一个团品信息的起始位置
                int endIndex;
                ///“直属”在单个团品字符串中的位置
                int directPriceIndex;

                
                ///单个团品实体
                Product singleProduct = new Product();
                for (int i = 0; i < Constants.CircleNumbers.Length - 1; i++)
                {
                    //get each Product start position and next
                    startIndex = strText.IndexOf(Constants.CircleNumbers[i].ToString());
                    endIndex = strText.IndexOf(Constants.CircleNumbers[i + 1].ToString());
                    endIndex = endIndex < 0 ? strText.Length : endIndex;
                    strSingleProduct = strText.Substring(startIndex, endIndex - startIndex).Trim();
                    strText = strText.Substring(endIndex, strText.Length - endIndex).Trim();

                    singleProduct.PName = strSingleProduct.Substring(1, 100);


                }
            }
        }
    }
}

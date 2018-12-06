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
using System.Text.RegularExpressions;

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

        #region Private Methos
        private bool SingleProductImportCheck(Product obj, out string result)
        {
            if (string.IsNullOrEmpty(obj.PName) ||
                obj.Combinition <= 0 ||
                string.IsNullOrEmpty(obj.Unit) ||
                obj.DistributionPrice <= 0 ||
                obj.GroupPrice <= 0 ||
                string.IsNullOrEmpty(obj.ProductDate)
                )
            {
                result = "团品信息不完整，请填写完整后再【录入】！";
                return false;
            }
            else if (mainViewModel.Products.Find(p => p.PId == mainViewModel.SingleProduct.PId) != null)
            {
                result = "团品信息已存在，请勿重复添加！";
                return false;
            }
            else
            {
                result = "";
                return true;
            }
        }

        /// <summary>
        /// Init the Single Product data
        /// </summary>
        private void SingleProductInitial()
        {
            mainViewModel.SingleProduct = new Product();
            mainViewModel.SingleProduct.ProductDate = DateTime.Now.ToString("yyyy/MM/dd");
            this.gbSingleImport.DataContext = null;
            this.gbSingleImport.DataContext = mainViewModel.SingleProduct;
        }
        /// <summary>
        /// Refresh the Single Product data
        /// </summary>
        private void SingleProductRefresh()
        {
            this.gbSingleImport.DataContext = null;
            this.gbSingleImport.DataContext = mainViewModel.SingleProduct;
        }
        /// <summary>
        /// 
        /// </summary>
        private void DataGridInitial()
        {
            mainViewModel.Products = new List<Product>();
            this.ProductManageDG.ItemsSource = null;
            this.ProductManageDG.ItemsSource = mainViewModel.Products;
        }
        /// <summary>
        /// Refresh the DataGrid data
        /// </summary>
        private void DataGridRefresh()
        {
            this.ProductManageDG.ItemsSource = null;
            this.ProductManageDG.ItemsSource = mainViewModel.Products;
        }
        #endregion

        #region System Events
        private void btnIdentity_Click_1(object sender, RoutedEventArgs e)
        {
            string strText = this.txtDataIdentify.Text.ToString();
            //string strTmp = Regex.Replace(strText, @"[^\d+\.*\d*]", "");
            if (!string.IsNullOrEmpty(strText) && strText.IndexOf("①") > -1)
            {
                //清空原数据
                mainViewModel.Products = new List<Product>();
                ///单个团品信息字符串
                string strSingleProduct = "";
                ///单个团品字符串在整个识别信息中的起始位置
                int startIndex;
                ///下一个团品信息的起始位置
                int endIndex;
                ///“直属”在单个团品字符串中的位置
                int directPriceIndex;
                //“分销”在单个团品字符串中的位置
                int distributionPriceIndex;
                //“团购”在单个团品字符串中的位置
                int groupPriceIndex;
                //团品组合和单位
                string cUnit = "";
                ///单个团品实体
                Product singleProduct;
                for (int i = 0; i < Constants.CircleNumbers.Length - 1; i++)
                {
                    singleProduct = new Product();
                    singleProduct.PId = i + 1;
                    //get each Product start position and next
                    startIndex = strText.IndexOf(Constants.CircleNumbers[i].ToString());
                    endIndex = strText.IndexOf(Constants.CircleNumbers[i + 1].ToString());
                    endIndex = endIndex < 0 ? strText.Length : endIndex;
                    strSingleProduct = strText.Substring(startIndex, endIndex - startIndex).Trim();

                    directPriceIndex = strSingleProduct.IndexOf(Constants.DirectPriceName);
                    distributionPriceIndex = strSingleProduct.IndexOf(Constants.DistributionPriceName);
                    groupPriceIndex = strSingleProduct.IndexOf(Constants.GroupPriceName);

                    ///1.获取团品名称
                    //如果有直属价
                    if (directPriceIndex > -1)
                    {
                        singleProduct.PName = strSingleProduct.Substring(1, directPriceIndex - 1).Trim();
                        //获取直属价
                        singleProduct.DirectPrice = Convert.ToDecimal(Regex.Match(strSingleProduct, Constants.DirectPriceName + @"\d+").ToString()
                            .Replace(Constants.DirectPriceName, ""));
                    }
                    else
                    {
                        singleProduct.PName = strSingleProduct.Substring(1, distributionPriceIndex - 1).Trim();
                    }
                    //去掉团品名称中的数量及单位
                    if (!string.IsNullOrEmpty(Regex.Match(singleProduct.PName, @"\d+[\u4e00-\u9fa5]").ToString()))
                        singleProduct.PName = singleProduct.PName.Substring(0, singleProduct.PName.IndexOf(Regex.Match(singleProduct.PName, @"\d+[\u4e00-\u9fa5]").ToString()));

                    //去除团品名称（和直属价）字符串，即获取第一个“,”之后的字符串
                    //strSingleProduct = strSingleProduct.Substring(strSingleProduct.IndexOf("，") + 1);
                    ///2.获取分销价
                    singleProduct.DistributionPrice = Convert.ToDecimal(Regex.Match(strSingleProduct, Constants.DistributionPriceName + @"\d+").ToString()
                            .Replace(Constants.DistributionPriceName, ""));
                    //去除分销价字符串，即获取第一个“,”之后的字符串
                    //strSingleProduct = strSingleProduct.Substring(strSingleProduct.IndexOf("，") + 1);

                    ///3.获取团品组合数字和单位
                    cUnit = Regex.Match(strSingleProduct, @"\d+[\u4e00-\u9fa5]" + Constants.DoubleMark).ToString().Replace(Constants.DoubleMark, "");
                    singleProduct.Combinition = Convert.ToInt32(Regex.Match(cUnit, @"\d+").ToString());
                    singleProduct.Unit = cUnit.Replace(singleProduct.Combinition.ToString(), "");
                    //去除团品组合数字和单位字符串
                    //strSingleProduct = strSingleProduct.Replace(cUnit, "");
                    ///4.获取团购价
                    singleProduct.GroupPrice = Convert.ToDecimal(Regex.Match(strSingleProduct, Constants.GroupPriceName + @"\d+").ToString()
                            .Replace(Constants.GroupPriceName, ""));
                    ///5.获取备注信息
                    if (endIndex == strText.Length)
                    {
                        int doubleMarkIndex = strSingleProduct.IndexOf(Constants.DoubleMark);
                        int unusualInfoIndex = strSingleProduct.IndexOf(Constants.OrderFormat);
                        unusualInfoIndex = unusualInfoIndex > -1 ? unusualInfoIndex - 1 : strSingleProduct.Length;
                        singleProduct.Remarks = strSingleProduct.Substring(doubleMarkIndex + 1, unusualInfoIndex - doubleMarkIndex - 1).Trim();
                    }
                    else
                        singleProduct.Remarks = strSingleProduct.Substring(strSingleProduct.IndexOf(Constants.DoubleMark) + 1).Trim();

                    ///6. 添加日期
                    singleProduct.ProductDate = DateTime.Now.ToString("yyyy/MM/dd");
                    ///7.将单个团品信息添加到ProductList中
                    mainViewModel.Products.Add(singleProduct);

                    if (endIndex == strText.Length)
                        break;
                    else
                        strText = strText.Substring(endIndex).Trim();
                }
            }
            else
            {
                mainViewModel.Products = new List<Product>();
            }
            SingleProductInitial();
            DataGridRefresh();
        }

        private void ProductManageDG_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void ProductManageDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            mainViewModel.SingleProduct = this.ProductManageDG.SelectedItem as Product;
            gbSingleImport.DataContext = mainViewModel.SingleProduct;
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            //check the SingleProduct data
            string checkResult = "";
            if (SingleProductImportCheck(mainViewModel.SingleProduct, out checkResult))
            {
                mainViewModel.Products.Add(mainViewModel.SingleProduct);
                SingleProductInitial();
                DataGridRefresh();
            }
            else
                MessageBox.Show(checkResult, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var objUpdate = mainViewModel.Products.Find(p => p.PId == mainViewModel.SingleProduct.PId);
            objUpdate = mainViewModel.SingleProduct;
            DataGridRefresh();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp.Wpf;
using CefSharp.Core;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using System.Security.Policy;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using CefSharp.DevTools.DOM;
using EChartsTest.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
//20231031新增
using System.Runtime.InteropServices;
using System.Diagnostics;
using ExcelDataReader;
using MiniExcelLibs.Attributes;
using System.Data;
using AduSkin.Utility;
using CefSharp;
using System.Threading;
using System.Threading.Tasks;

namespace EChartsTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public class ExcelConverter
    {

        //注：这个ConvertToXYZ并没有使用
        public static DataTable ConvertToXYZ(string filePath)
        {
            // 读取Excel文件
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false
                        }
                    });

                    var dataTable = dataSet.Tables[0];
                    var numRows = dataTable.Rows.Count;
                    var numColumns = dataTable.Columns.Count;

                    // 创建新的DataTable用于存储转换后的数据
                    var newDataTable = new DataTable();
                    newDataTable.Columns.Add("x", typeof(int));
                    newDataTable.Columns.Add("y", typeof(int));
                    newDataTable.Columns.Add("z");

                    // 遍历每一行
                    for (int i = 0; i < numRows; i++)
                    {
                        // 遍历每一列
                        for (int j = 0; j < numColumns; j++)
                        {
                            var value = dataTable.Rows[i][j];

                            // 添加新的一行数据到新的DataTable中
                            newDataTable.Rows.Add(i + 1, j + 1, value);
                        }
                    }

                    return newDataTable;
                }
            }
        }
        /// <summary>
        ///转换数据为xyz，保存为json
        /// </summary>
        /// <param name="filename">excel路径</param>
        /// <returns></returns>
        /// 

        public static string ConvertToXYZExcel(string filename)
        {

            var newxlsx = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename), $"{System.IO.Path.GetFileNameWithoutExtension(filename)}_NEW{System.IO.Path.GetExtension(filename)}");
            if (File.Exists(newxlsx)) File.Delete(newxlsx);
            var list = MiniExcelLibs.MiniExcel.Query(filename).ToList();




            List<Vector3> vector3s = new List<Vector3>();
            //修改过后的，效果为x:123456......y:00000.....
            //for (int i = 0; i < list.Count(); i++)
            //{
            //    if (list[i] is IDictionary<string, object> dic)
            //    {
            //        var data = dic.Select(x => (double)x.Value).ToList();
            //        for (int j = 0; j < data.Count; j++)
            //        {
            //            vector3s.Add(new Vector3() { x = j, y = i, z = data[j]});
            //        }
            //    }
            //}
            var temp = new List<List<double>>();
            //暂时同上方代码
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] is IDictionary<string, object> dic)
                {
                    var data = dic.Select(x => (double)x.Value).ToList();
                    temp.Add(data);
                    //for (int j = 0; j < data.Count; j++)
                    //{
                    //    vector3s.Add(new Vector3() { x = j, y = i, z = data[j] });
                    //}
                }
            }
            for (int i = 0; i < temp[0].Count(); i++)
            {
                for (int j = 0; j < temp.Count(); j++)
                {
                    vector3s.Add(new Vector3() { x = i, y = j, z = new List<double> { temp[j][i] } });

                }
            }


            //原来的代码 xy反了
            //for (int i = 0; i < list.Count(); i++)
            //{
            //    if (list[i] is IDictionary<string, object> dic)
            //    {
            //        var data = dic.Select(x => (double)x.Value).ToList();
            //        for (int j = 0; j < data.Count; j++)
            //        {
            //            vector3s.Add(new Vector3() { x = i, y = j, z = data[j] });
            //        }
            //    }
            //}
            MiniExcelLibs.MiniExcel.SaveAs(newxlsx, vector3s.Select(x => new { X = x.x, Y = x.y, Z = x.z }));
            return newxlsx;
        }
        public static string Csv_d2Js(string filename)
        {

            var list = MiniExcelLibs.MiniExcel.Query(filename).ToList();

            List<Vector3ds> vector3s = new List<Vector3ds>();
            //修改过后的，效果为x:123456......y:00000.....
            //for (int i = 0; i < list.Count(); i++)
            //{
            //    if (list[i] is IDictionary<string, object> dic)
            //    {
            //        var data = dic.Select(x => (double)x.Value).ToList();
            //        for (int j = 0; j < data.Count; j++)
            //        {
            //            vector3s.Add(new Vector3() { x = j, y = i, z = data[j]});
            //        }
            //    }
            //}
            var temp = new List<List<double>>();
            //暂时同上方代码
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] is IDictionary<string, object> dic)
                {
                    List<double> data = new List<double>();
                    foreach (var item in dic.Values)
                    {
                        if (double.TryParse(item.ToString(), out double d))
                        {
                            data.Add(d);
                        }
                        else data.Add(0);
                    }
                    temp.Add(data);
                }
            }

            for (int i = 0; i < temp[0].Count(); i++)
            {
                for (int j = 0; j < temp.Count(); j++)
                {
                    vector3s.Add(new Vector3ds() { x = j + 1, y = i + 1, z = temp[j][i] });
                }
            }

            var js = filename.Replace(System.IO.Path.GetExtension(filename), ".js");
            File.WriteAllText(js, $"var myData = {JsonConvert.SerializeObject(vector3s.Select(x => new { x = x.x, y = x.y, z = x.z }))}");
            return js;
        }
        public static string Csv_s2Js(string filename)
        {

            var list = MiniExcelLibs.MiniExcel.Query(filename).ToList();

            List<Vector3ds> vector3s = new List<Vector3ds>();
            //修改过后的，效果为x:123456......y:00000.....
            //for (int i = 0; i < list.Count(); i++)
            //{
            //    if (list[i] is IDictionary<string, object> dic)
            //    {
            //        var data = dic.Select(x => (double)x.Value).ToList();
            //        for (int j = 0; j < data.Count; j++)
            //        {
            //            vector3s.Add(new Vector3() { x = j, y = i, z = data[j]});
            //        }
            //    }
            //}
            var temp = new List<List<double>>();
            //暂时同上方代码
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] is IDictionary<string, object> dic)
                {
                    List<double> data = new List<double>();
                    foreach (var item in dic.Values)
                    {
                        if (double.TryParse(item.ToString(), out double d))
                        {
                            data.Add(d);
                        }
                        else data.Add(0);
                    }
                    temp.Add(data);
                }
            }
            //原代码，有问题，形状不对，应该在上半部分的在下半部分，应该在下半部分的在上半部分
            //for (int i = 0; i < temp[0].Count(); i++)
            //{
            //    for (int j = 0; j < temp.Count(); j++)
            //    {
            //        vector3s.Add(new Vector3ds() { x = i, y = j, z = temp[i][j] });

            //    }
            //}

            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < temp[i].Count; j++)
                {
                    if (i < temp.Count / 2)
                    {
                        vector3s.Add(new Vector3ds() { x = j, y = temp.Count / 2 - i, z = temp[i][j] });
                    }
                    else
                    {
                        vector3s.Add(new Vector3ds() { x = j, y = temp.Count / 2 + temp.Count % 2 + (temp.Count / 2 - i), z = temp[i][j] });
                    }
                }
            }


            var js = filename.Replace(System.IO.Path.GetExtension(filename), ".js");
            File.WriteAllText(js, $"var myData = {JsonConvert.SerializeObject(vector3s.Select(x => new { x = x.x, y = x.y, z = x.z }))}");
            return js;
        }
        public static string Csv2Js(string filename)
        {

            var list = MiniExcelLibs.MiniExcel.Query(filename).ToList();

            List<Vector3> vector3s = new List<Vector3>();
            //修改过后的，效果为x:123456......y:00000.....
            //for (int i = 0; i < list.Count(); i++)
            //{
            //    if (list[i] is IDictionary<string, object> dic)
            //    {
            //        var data = dic.Select(x => (double)x.Value).ToList();
            //        for (int j = 0; j < data.Count; j++)
            //        {
            //            vector3s.Add(new Vector3() { x = j, y = i, z = data[j]});
            //        }
            //    }
            //}
            var temp = new List<List<double>>();
            //暂时同上方代码
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i] is IDictionary<string, object> dic)
                {
                    List<double> data = new List<double>();
                    foreach (var item in dic.Values)
                    {
                        if (double.TryParse(item.ToString(), out double d))
                        {
                            data.Add(d);
                        }
                        else data.Add(0);
                    }
                    temp.Add(data);
                    //for (int j = 0; j < data.Count; j++)
                    //{
                    //    vector3s.Add(new Vector3() { x = j, y = i, z = data[j] });
                    //}
                }
            }
            //for (int i = 0; i < temp[0].Count(); i++)
            //{
            //    for (int j = 0; j < temp.Count(); j++)
            //    {
            //        vector3s.Add(new Vector3() { x = i, y = j, z = new List<double> { temp[j][i] } });

            //    }
            //}
            for (int i = 0; i < temp[0].Count(); i++)
            {
                for (int j = 0; j < temp.Count(); j++)
                {
                    vector3s.Add(new Vector3() { x = j, y = i, z = new List<double> { temp[j][i] } });

                }
            }


            //原来的代码 xy反了
            //for (int i = 0; i < list.Count(); i++)
            //{
            //    if (list[i] is IDictionary<string, object> dic)
            //    {
            //        var data = dic.Select(x => (double)x.Value).ToList();
            //        for (int j = 0; j < data.Count; j++)
            //        {
            //            vector3s.Add(new Vector3() { x = i, y = j, z = data[j] });
            //        }
            //    }
            //}
            var js = filename.Replace(System.IO.Path.GetExtension(filename), ".js");
            File.WriteAllText(js, $"var myData = {JsonConvert.SerializeObject(vector3s.Select(x => new { x = x.x, y = x.y, z = x.z }))}");
            return js;
        }
        //20231113新增代码，u与v结合形成新js。
        public static string U_v2Js(string filenameU, string filenameV)
        {
            var listU = MiniExcelLibs.MiniExcel.Query(filenameU).ToList();
            var listV = MiniExcelLibs.MiniExcel.Query(filenameV).ToList();

            List<Vector3> vector3s = new List<Vector3>();

            var tempU = new List<List<double>>();
            for (int i = 0; i < listU.Count(); i++)
            {
                if (listU[i] is IDictionary<string, object> dic)
                {
                    List<double> data = new List<double>();
                    foreach (var item in dic.Values)
                    {
                        if (double.TryParse(item.ToString(), out double d))
                        {
                            data.Add(d);
                        }
                        else data.Add(0);
                    }
                    tempU.Add(data);
                }
            }

            var tempV = new List<List<double>>();
            for (int i = 0; i < listV.Count(); i++)
            {
                if (listV[i] is IDictionary<string, object> dic)
                {
                    List<double> data = new List<double>();
                    foreach (var item in dic.Values)
                    {
                        if (double.TryParse(item.ToString(), out double d))
                        {
                            data.Add(d);
                        }
                        else data.Add(0);
                    }
                    tempV.Add(data);
                }
            }

            for (int i = 0; i < tempU[0].Count(); i++)
            {
                for (int j = 0; j < tempU.Count(); j++)
                {
                    vector3s.Add(new Vector3() { x = i, y = j, z = new List<double> { tempU[j][i] } });
                }
            }

            // 合并 Z 列数据
            for (int i = 0; i < tempU[0].Count(); i++)
            {
                for (int j = 0; j < tempU.Count(); j++)
                {
                    var vectorA = vector3s[i + j * tempU[0].Count()];
                    var vectorB = new Vector3() { x = i, y = j, z = new List<double> { tempV[j][i] } };
                    var cZ = Vector3.CombineZ(vectorA, vectorB);
                    vector3s[i + j * tempU[0].Count()].z = cZ;
                }
            }

            var js = filenameU.Replace(System.IO.Path.GetExtension(filenameU), "_v.js");
            File.WriteAllText(js, $"var windData = {{ nx: 100, ny: 100, max: 0.5, data: {JsonConvert.SerializeObject(vector3s.Select(x => new[] { x.z[0], x.z[1] }))} }}");
            return js;
        }




        //public static string XYZExcelToJson(string xyzExcelFileName)
        //{
        //    var jsonPath = xyzExcelFileName.Replace(Path.GetExtension(xyzExcelFileName), ".json");
        //    var list = MiniExcelLibs.MiniExcel.Query<Vector3>(xyzExcelFileName).ToList();
        //    File.WriteAllText(jsonPath, JsonConvert.SerializeObject(list.ToArray()));
        //                        return jsonPath;
        //}
        public static string XYZExcelToJs(string xyzExcelFileName)
        {
            var jsPath = xyzExcelFileName.Replace(System.IO.Path.GetExtension(xyzExcelFileName), ".js");
            var list = MiniExcelLibs.MiniExcel.Query<Vector3>(xyzExcelFileName).ToList();
            File.WriteAllText(jsPath, JsonConvert.SerializeObject(list.ToArray()));
            return jsPath;
        }

        internal static void ConvertToXYZExcel(object csv)
        {
            throw new NotImplementedException();
        }

        //以下为原Vector3
        //public class Vector3
        //{
        //    [ExcelColumnName("X")]
        //    public double x { get; set; }
        //    [ExcelColumnName("Y")]
        //    public double y { get; set; }
        //    [ExcelColumnName("Z")]
        //    //public double z { get; set; }
        //    public List<double> z { get; set; }

        //    public static List<double> CombineZ(Vector3 vectorA, Vector3 vectorB)
        //    {
        //        return new List<double> { vectorA.z[0], vectorB.z[0] }; // 返回一个包含两个double值的列表
        //    }
        //}

        //以下为20231113修改过后的Vector3
        public class Vector3
        {
            [ExcelColumnName("X")]
            public double x { get; set; }
            [ExcelColumnName("Y")]
            public double y { get; set; }
            [ExcelColumnName("Z")]
            //public double z { get; set; }
            public List<double> z { get; set; }

            public static List<double> CombineZ(Vector3 vectorA, Vector3 vectorB)
            {
                return new List<double> { vectorA.z[0], vectorB.z[0] }; // 返回一个包含两个double值的列表
            }
        }
        public class Vector3ds
        {
            [ExcelColumnName("X")]
            public double x { get; set; }
            [ExcelColumnName("Y")]
            public double y { get; set; }
            [ExcelColumnName("Z")]
            public double z { get; set; }
        }

    }

    public partial class MainWindow : Window
    {



        public ObservableCollection<Note> Notes { get; set; }
        //public ChromiumWebBrowser browser;
        int show = 1;
        //urlt是为了下面引用的
        string urlt = "chart1";
        string url;
        string url1;
        string url2;
        string url3;
        string url4;
        string url5;
        string url6;
        string url7;
        string url8;
        string url9;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Notes = new ObservableCollection<Note>();

            //Web.ObjectForScripting = new WebAdapter(Web);
            //Web.Navigate(new Uri(Directory.GetCurrentDirectory() + "/chart1.html"));

            //browser = new ChromiumWebBrowser();


            //两种url引用方式都行，都是对的
            //url = Directory.GetCurrentDirectory() + "/" + urlt + ".html";

            url = Directory.GetCurrentDirectory() + "/chart1.html";
            url1 = Directory.GetCurrentDirectory() + "/速度场.html";
            url2 = Directory.GetCurrentDirectory() + "/剖面图.html";
            url3 = Directory.GetCurrentDirectory() + "/heatmap-large.html";
            url4 = Directory.GetCurrentDirectory() + "/水深.html";
            url5 = Directory.GetCurrentDirectory() + "/scatter3d-simplex-noise.html";
            url6 = Directory.GetCurrentDirectory() + "/三维图-imagesurface-动态-z=0不变.html";
            url7 = Directory.GetCurrentDirectory() + "/chart2.html";
            url8 = Directory.GetCurrentDirectory() + "/沉积物浓度图.html";
          //  browser.LoadUrl(url);
           // browser1.LoadUrl(url1);
           // browser2.LoadUrl(url2);
           // browser3.LoadUrl(url3);
           // browser4.LoadUrl(url4);
           // browser5.LoadUrl(url5);
          //  browser6.LoadUrl(url6);
           // browser7.LoadUrl(url7);
           // browser8.LoadUrl(url8);
        }
        //private void Button5_Click(object sender, RoutedEventArgs e)
        //{
        //    var childWindow = new ChildWindow();

        //    //写法一：代码就写在MainWindow的后台代码中时写this即是MainWindow
        //    //_childWindow.Owner = this;

        //    //写法二：代码写在其他地方时，写清楚当前应用程序的MainWindow
        //    childWindow.Owner = Application.Current.MainWindow;
        //    //childWindow.Show();
        //    //为显示窗口，需要创建Window类的实例并使用Show()或ShowDialog()方法。            
        //    //其中Show()方法显示非模态窗口，不会阻止用户访问其他任何窗口；
        //    //ShowDialog()方法显示模态窗口，会锁定所有鼠标和键盘输入来阻止用户访问父窗口，知道模态窗口关闭。
        //    childWindow.ShowDialog();
        //}

        private void btnShowHide_Click(object sender, RoutedEventArgs e)
        {
            //Web.InvokeScript("jsShowHide", show);
            //if (show == 0)
            //    show = 1;
            //else
            //    show = 0;
            show = show + 1;
            if (show > 5) { show = 1; }
            urlt = show.ToString();
            url = Directory.GetCurrentDirectory() + "/" + urlt + ".html";
            //browser.LoadUrl(url);
        }


        private void btnPushData_Click(object sender, RoutedEventArgs e)
        {
            //Web.InvokeScript("jsPushData", "x", 1000);
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            // 处理打开菜单项的事件
            // 可以使用 OpenFileDialog 打开文件
            // 例如：
            // OpenFileDialog openFileDialog = new OpenFileDialog();
            // if (openFileDialog.ShowDialog() == true)
            // {
            //     string filename = openFileDialog.FileName;
            //     // 在这里进行文件的加载和处理
            // }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            // 处理保存菜单项的事件
            // 可以使用 SaveFileDialog 保存文件
            // 例如：
            // SaveFileDialog saveFileDialog = new SaveFileDialog();
            // if (saveFileDialog.ShowDialog() == true)
            // {
            //     string filename = saveFileDialog.FileName;
            //     // 在这里进行文件的保存
            // }
        }

        private string preSelectProjectName = "";
        private void Program1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var note = Program1.SelectedItem as Note;
            if (note == null) return;
            if (string.IsNullOrEmpty(note.Name)) return;
            if (preSelectProjectName != note.Tag)
            {
                if (!string.IsNullOrEmpty(note.Path))
                {
                    GetConfig(note.Tag, note.Path);
                }
                preSelectProjectName = note.Tag;
            }
            var headers = note.Name.Split('.');
            if (headers == null) return;
            string header = "";
            if (headers.Length > 1)
                header = headers[1];
            else header = headers[0];
            if (treeHeaders.Any(x => x.Equals(note.Name)))
            {
                tab1.SelectedItem = tab1.Items[1];
                foreach (var item in tab2.Items)
                {
                    if (item is TabItem ti)
                    {
                        if (ti.Header.Equals(header))
                        {
                            tab2.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in tab1.Items)
                {
                    if (item is TabItem ti)
                    {
                        if (ti.Header.Equals(header))
                        {
                            tab1.SelectedItem = item;
                            break;
                        }
                    }
                }
            }

            //Program1.SelectedItem
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (Program1.SelectedItem is TreeViewItem tv && tv.Header.Equals("Thin Dams"))
            //{
            var window = new Window1();
            window.Owner = this;
            window.ShowDialog();
            //}
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //沉积参数的确定键
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ltdlnd.Text, out double ltdlnd1) &&
                ltdlnd1 > 0 &&
                double.TryParse(zzlj.Text, out double zzlj1) &&
                zzlj1 > 0 &&
                //double.TryParse(csnsmd.Text, out double csnsmd1) &&
                double.TryParse(klmd.Text, out double klmd1) &&
                klmd1 > 0 &&
                double.TryParse(ccl.Text, out double ccl1) &&
                ccl1 > 0 &&
                double.TryParse(ltmd.Text, out double ltmd1) &&
                ltmd1 > 0 &&
                double.TryParse(sd.Text, out double sd1) &&
                sd1 > 0
                )
            {
                MyData.Instance.Type = type.Text;
                MyData.Instance.Ltdlnd = ltdlnd1;
                MyData.Instance.Zzlj = zzlj1;
                //MyData.Instance.Csnsmd = csnsmd1;
                MyData.Instance.Klmd = klmd1;
                MyData.Instance.Ccl = ccl1;
                MyData.Instance.Ltmd = ltmd1;
                MyData.Instance.Sd = sd1;

                //计算公式
                var setting = (MyData.Instance.Klmd - MyData.Instance.Ltmd) * 9.8 * Math.Pow(MyData.Instance.Zzlj, 2) / 18 / MyData.Instance.Ltdlnd;
                var vc = Math.Sqrt(2 * 9.8 * MyData.Instance.Zzlj * (MyData.Instance.Klmd - MyData.Instance.Ltmd) / MyData.Instance.Ltmd);
                var t = 1 / MyData.Instance.Ccl * Math.Pow(MyData.Instance.Sd, 1.0 / 6);
                MessageBox.Show($"setting:{setting}\nvc:{vc}\nt:{t}");
            }
            else
            {
                MessageBox.Show("参数不合理");
            }
        }
        private void Button_Click_2wg(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(lx.Text, out double lx1) &&
                lx1 > 0 &&
                double.TryParse(ly.Text, out double ly1) &&
                ly1 > 0 &&
                //double.TryParse(csnsmd.Text, out double csnsmd1) &&
                int.TryParse(nx.Text, out int nx1) &&
                nx1 > 0 &&
                int.TryParse(ny.Text, out int ny1) &&
                ny1 > 0 &&
                double.TryParse(dt.Text, out double dt1) &&
                dt1 > 0 &&
                double.TryParse(t.Text, out double t1) &&
                t1 > 0 &&
                double.TryParse(jingdu.Text, out double jingdu1) &&
                jingdu1 > 0 &&
                double.TryParse(weidu.Text, out double weidu1) &&
                weidu1 > 0 &&
                double.TryParse(jiaosudu.Text, out double jiaosudu1) &&
                jiaosudu1 > 0 &&
                double.TryParse(k.Text, out double k1) &&
                k1 > 0

                )
            {
                MyData.Instance.Type = type.Text;
                MyData.Instance.Lx = lx1;
                MyData.Instance.Ly = ly1;
                //MyData.Instance.Csnsmd = csnsmd1;
                MyData.Instance.Nx = nx1;
                MyData.Instance.Ny = ny1;
                MyData.Instance.Dt = dt1;
                MyData.Instance.T = t1;
                MyData.Instance.Jingdu = jingdu1;
                MyData.Instance.Weidu = weidu1;
                MyData.Instance.Jiaosudu = jiaosudu1;
                MyData.Instance.K = k1;



                var f = MyData.Instance.Jiaosudu * 2.0 * Math.Sin(MyData.Instance.Weidu);
                //double abc = (MyData.Instance.T / MyData.Instance.Dt);
                //var nt = Math.Ceiling(abc);

                //MessageBox.Show($"f(科氏力):{f}\nNt:{nt}");
                MessageBox.Show($"f(科氏力):{f}");


            }
            else
            {
                MessageBox.Show("参数不合理");
            }
        }
        private void Button_Click_3fl(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(kqmd.Text, out double kqmd1) &&
                kqmd1 > 0 &&
                double.TryParse(fyl.Text, out double fyl1) &&
                fyl1 > 0 &&
                //double.TryParse(csnsmd.Text, out double csnsmd1) &&
                double.TryParse(pjfs.Text, out double pjfs1) &&
                pjfs1 > 0 &&
                double.TryParse(jd.Text, out double jd1) &&
                //jd1 > 0 &&
                double.TryParse(bc.Text, out double bc1) &&
                bc1 > 0 &&
                double.TryParse(bxj.Text, out double bxj1) &&
                //bxj1 < 0 &&
                double.TryParse(ltmd.Text, out double ltmd1) &&
                ltmd1 > 0

                )
            {
                MyData.Instance.Type = type.Text;
                MyData.Instance.Kqmd = kqmd1;
                MyData.Instance.Fyl = fyl1;
                //MyData.Instance.Csnsmd = csnsmd1;
                MyData.Instance.Pjfs = pjfs1;
                MyData.Instance.Jd = jd1;
                MyData.Instance.Bc = bc1;
                MyData.Instance.Bxj = bxj1;
                MyData.Instance.Ltmd = ltmd1;
                if (MyData.Instance.Pjfs > 0 && MyData.Instance.Pjfs < 16.808)
                {
                    MyData.Instance.WaveH = -0.082 + 0.076 * MyData.Instance.Pjfs + 0.011 * MyData.Instance.Pjfs * MyData.Instance.Pjfs;
                }
                else if (MyData.Instance.Pjfs > 16.808 && MyData.Instance.Pjfs < 40)
                {
                    MyData.Instance.WaveH = 0.588 + 0.217 * MyData.Instance.Pjfs;
                }
                else
                {
                    MessageBox.Show("波高相关参数不合理");
                }


                var waveH = MyData.Instance.WaveH;
                var waveE = 0.125 * MyData.Instance.Ltmd * 9.8 * MyData.Instance.WaveH * MyData.Instance.WaveH;
                var waveK = 1 / MyData.Instance.Bc;
                //double abc = (MyData.Instance.T / MyData.Instance.Dt);
                //var nt = Math.Ceiling(abc);

                //MessageBox.Show($"f(科氏力):{f}\nNt:{nt}");
                MessageBox.Show($"waveH(波高):{waveH}\nwaveE(不知道是啥):{waveE}\nwaveK(波数):{waveK}");


            }
            else
            {
                MessageBox.Show("参数不合理");
            }
        }

        //[DllImport("libuntitled.dll")]   //应该要注释掉
        // public static extern void hello();

        //这里的a对应dell中函数的
        [DllImport("libuntitled.dll")]
        //[DllImport("untitled20231110.dll")]

        public static extern void mm(double ltdlnd1, double zzlj1, double klmd1, double ccl1, double ltmd1, double sd1, double lx1, double ly1, int nx1, int ny1, double dx, double dy, double dt1, double t1, int nt, double jingdu1, double weidu1, double jiaosudu1, double k1, double kqmd1, double fyl1, double pjfs1, double jd1, double bc1, double bxj1, double f, double setting, double vc, double tcadepth, double waveH, double waveE, double waveK, double md1);
        //以下4lt按钮内容暂时错误，如果需要用到该按钮再更改 



        private void Button_Click_0All(object sender, RoutedEventArgs e)
        {
            //这里的1是你输入的

            //var ia = 0;
            if (double.TryParse(ltdlnd.Text, out double ltdlnd1) &&
                ltdlnd1 > 0 &&
                double.TryParse(zzlj.Text, out double zzlj1) &&
                zzlj1 > 0 &&
                //double.TryParse(csnsmd.Text, out double csnsmd1) &&
                double.TryParse(klmd.Text, out double klmd1) &&
                klmd1 > 0 &&
                double.TryParse(ccl.Text, out double ccl1) &&
                ccl1 > 0 &&
                double.TryParse(ltmd.Text, out double ltmd1) &&
                ltmd1 > 0 &&
                double.TryParse(sd.Text, out double sd1) &&
                sd1 > 0 &&

                double.TryParse(lx.Text, out double lx1) &&
                lx1 > 0 &&
                double.TryParse(ly.Text, out double ly1) &&
                ly1 > 0 &&
                //double.TryParse(csnsmd.Text, out double csnsmd1) &&
                int.TryParse(nx.Text, out int nx1) &&
                nx1 > 0 &&
                int.TryParse(ny.Text, out int ny1) &&
                ny1 > 0 &&
                double.TryParse(dt.Text, out double dt1) &&
                dt1 > 0 &&
                double.TryParse(t.Text, out double t1) &&
                t1 > 0 &&
                double.TryParse(jingdu.Text, out double jingdu1) &&
                jingdu1 > 0 &&
                double.TryParse(weidu.Text, out double weidu1) &&
                weidu1 > 0 &&
                double.TryParse(jiaosudu.Text, out double jiaosudu1) &&
                jiaosudu1 > 0 &&
                double.TryParse(k.Text, out double k1) &&
                k1 > 0 &&

                double.TryParse(kqmd.Text, out double kqmd1) &&
                kqmd1 > 0 &&
                double.TryParse(fyl.Text, out double fyl1) &&
                fyl1 > 0 &&
                //double.TryParse(csnsmd.Text, out double csnsmd1) &&
                double.TryParse(pjfs.Text, out double pjfs1) &&
                pjfs1 > 0 &&
                double.TryParse(jd.Text, out double jd1) &&
                //jd1 > 0 &&
                double.TryParse(bc.Text, out double bc1) &&
                bc1 > 0 &&
                double.TryParse(bxj.Text, out double bxj1) &&

                double.TryParse(bxj.Text, out double md1) &&
                md1 > 0
                //&&  bxj1 > 0 
                //double.TryParse(ltmd.Text, out double ltmd1) &&
                //ltmd1 > 0

                )
            {

                MyData.Instance.Type = type.Text;
                MyData.Instance.Ltdlnd = ltdlnd1;
                MyData.Instance.Zzlj = zzlj1;
                //MyData.Instance.Csnsmd = csnsmd1;
                MyData.Instance.Klmd = klmd1;
                MyData.Instance.Ccl = ccl1;
                MyData.Instance.Ltmd = ltmd1;
                MyData.Instance.Sd = sd1;
                MyData.Instance.Md = md1;


                MyData.Instance.Type = type.Text;
                MyData.Instance.Lx = lx1;
                MyData.Instance.Ly = ly1;
                //MyData.Instance.Csnsmd = csnsmd1;
                MyData.Instance.Nx = nx1;
                MyData.Instance.Ny = ny1;
                MyData.Instance.Dt = dt1;
                MyData.Instance.T = t1;
                MyData.Instance.Jingdu = jingdu1;
                MyData.Instance.Weidu = weidu1;
                MyData.Instance.Jiaosudu = jiaosudu1;
                MyData.Instance.K = k1;

                //计算公式
                int nt = (int)Math.Ceiling(MyData.Instance.T / MyData.Instance.Dt);
                double dx = MyData.Instance.Lx / (MyData.Instance.Nx - 1);
                double dy = MyData.Instance.Ly / (MyData.Instance.Ny - 1);
                double setting = (MyData.Instance.Klmd - MyData.Instance.Ltmd) * 9.8 * Math.Pow(MyData.Instance.Zzlj, 2) / 18 / MyData.Instance.Ltdlnd;
                double vc = Math.Sqrt(2 * 9.8 * MyData.Instance.Zzlj * (MyData.Instance.Klmd - MyData.Instance.Ltmd) / MyData.Instance.Ltmd);
                double tcadepth = 1 / MyData.Instance.Ccl * Math.Pow(MyData.Instance.Sd, 1.0 / 6);
                MessageBox.Show($"setting:{setting}\nvc:{vc}\ntcadepth:{tcadepth}");


                //MyData.Instance.Type = type.Text;
                //MyData.Instance.Lx = lx1;
                //MyData.Instance.Ly = ly1;
                ////MyData.Instance.Csnsmd = csnsmd1;
                //MyData.Instance.Nx = nx1;
                //MyData.Instance.Ny = ny1;
                //MyData.Instance.Dt = dt1;
                //MyData.Instance.T = t1;
                //MyData.Instance.Jingdu = jingdu1;
                //MyData.Instance.Weidu = weidu1;
                //MyData.Instance.Jiaosudu = jiaosudu1;
                //MyData.Instance.K = k1;

                double f = MyData.Instance.Jiaosudu * 2.0 * Math.Sin(MyData.Instance.Weidu);

                MessageBox.Show($"f(科氏力):{f}");

                MyData.Instance.Type = type.Text;
                MyData.Instance.Kqmd = kqmd1;
                MyData.Instance.Fyl = fyl1;
                //MyData.Instance.Csnsmd = csnsmd1;
                MyData.Instance.Pjfs = pjfs1;
                MyData.Instance.Jd = jd1;
                MyData.Instance.Bc = bc1;
                MyData.Instance.Bxj = bxj1;
                MyData.Instance.Ltmd = ltmd1;
                double waveH = 0.0;
                if (MyData.Instance.Pjfs > 0 && MyData.Instance.Pjfs < 16.808)
                {
                    waveH = -0.082 + 0.076 * MyData.Instance.Pjfs + 0.011 * MyData.Instance.Pjfs * MyData.Instance.Pjfs;
                }
                else if (MyData.Instance.Pjfs > 16.808 && MyData.Instance.Pjfs < 40)
                {
                    waveH = 0.588 + 0.217 * MyData.Instance.Pjfs;
                }
                else
                {
                    MessageBox.Show("波高相关参数不合理");
                }


                //double waveH  = MyData.Instance.WaveH;




                double waveE = 0.125 * MyData.Instance.Ltmd * 9.8 * Math.Pow(waveH, 2);
                double waveK = 1 / MyData.Instance.Bc;

                MessageBox.Show($"waveH(波高):{waveH}\nwaveE(不知道是啥):{waveE}\nwaveK(波数):{waveK}");
                Debug.WriteLine($"{ltdlnd1},{zzlj1}, {klmd1}, {ccl1}, {ltmd1}, {sd1}, {lx1}, {ly1}, {nx1}, {ny1}, {dx} ,{dy}, {dt1}, {t1}, {nt}, {jingdu1}, {weidu1}, {jiaosudu1}, {k1}, {kqmd1}, {fyl1}, {pjfs1}, {jd1}, {bc1}, {bxj1}, {f}, {setting}, {vc} ,{tcadepth} ,{waveH}, {waveE}, {waveK}, {md1}");


                //这里调用mm(1);//20231114新增，尝试多线程以免假死


                //mm(ltdlnd1, zzlj1, klmd1, ccl1, ltmd1, sd1, lx1, ly1, nx1, ny1, dx, dy, dt1, t1, nt, jingdu1, weidu1, jiaosudu1, k1, kqmd1, fyl1, pjfs1, jd1, bc1, bxj1, f, setting, vc, tcadepth, waveH, waveE, waveK, md1);




                //var csvFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "outcsv");
                //if (Directory.Exists(csvFolder))
                //{
                //    string[] csvFiles = Directory.GetFiles(csvFolder, "*.csv");
                //    foreach (var item in csvFiles)
                //    {
                //        ExcelConverter.Csv2Js(item);
                //    }
                //    var ccsv = System.IO.Path.Combine(csvFolder, "c.js");
                //    var newccsv = System.IO.Path.Combine(Environment.CurrentDirectory, "c.js");

                //    foreach (var u in csvFiles)
                //    {
                //        foreach (var v in csvFiles)
                //        {
                //            ExcelConverter.U_v2Js(u, v);
                //        }
                //    }
                //    var u_vcsv = System.IO.Path.Combine(csvFolder, "u_v.js");
                //    var newu_vcsv = System.IO.Path.Combine(Environment.CurrentDirectory, "u_v.js");
                //    if (File.Exists(u_vcsv))
                //    {
                //        if (File.Exists(newu_vcsv)) File.Delete(newu_vcsv);
                //        File.Copy(u_vcsv, newu_vcsv);
                //        url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                //        browser9.LoadUrl(url9);
                //        foreach (var item in tab1.Items)
                //        {
                //            if (item is TabItem ti)
                //            {
                //                if (ti.Header.Equals("测试图"))
                //                {
                //                    tab1.SelectedItem = item;
                //                    break;
                //                }
                //            }
                //        }
                //    }

                //}


                //原线程代码，可正确运行版本
                //Task.Run(() =>
                //{
                //    // 第一段代码
                //    mm(ltdlnd1, zzlj1, klmd1, ccl1, ltmd1, sd1, lx1, ly1, nx1, ny1, dx, dy, dt1, t1, nt, jingdu1, weidu1, jiaosudu1, k1, kqmd1, fyl1, pjfs1, jd1, bc1, bxj1, f, setting, vc, tcadepth, waveH, waveE, waveK, md1);

                //    // 第二段代码
                //    var csvFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "outcsv");
                //    if (Directory.Exists(csvFolder))
                //    {
                //        string[] csvFiles = Directory.GetFiles(csvFolder, "*.csv");
                //        foreach (var item in csvFiles)
                //        {
                //            ExcelConverter.Csv2Js(item);
                //        }
                //        var ccsv = System.IO.Path.Combine(csvFolder, "c.js");
                //        var newccsv = System.IO.Path.Combine(Environment.CurrentDirectory, "c.js");

                //        foreach (var u in csvFiles)
                //        {
                //            foreach (var v in csvFiles)
                //            {
                //                ExcelConverter.U_v2Js(u, v);
                //            }
                //        }
                //        var u_vcsv = System.IO.Path.Combine(csvFolder, "u_v.js");
                //        var newu_vcsv = System.IO.Path.Combine(Environment.CurrentDirectory, "u_v.js");
                //        if (File.Exists(u_vcsv))
                //        {
                //            if (File.Exists(newu_vcsv)) File.Delete(newu_vcsv);
                //            File.Copy(u_vcsv, newu_vcsv);
                //            url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                //            browser9.LoadUrl(url9);
                //            foreach (var item in tab1.Items)
                //            {
                //                if (item is TabItem ti)
                //                {
                //                    if (ti.Header.Equals("测试图"))
                //                    {
                //                        tab1.SelectedItem = item;
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //});





                //Task.Run(() =>
                //{
                //    // 第一段代码
                //    mm(ltdlnd1, zzlj1, klmd1, ccl1, ltmd1, sd1, lx1, ly1, nx1, ny1, dx, dy, dt1, t1, nt, jingdu1, weidu1, jiaosudu1, k1, kqmd1, fyl1, pjfs1, jd1, bc1, bxj1, f, setting, vc, tcadepth, waveH, waveE, waveK, md1);

                //    // 第二段代码
                //    var csvFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "outcsv");
                //    if (Directory.Exists(csvFolder))
                //    {
                //        string[] csvFiles = Directory.GetFiles(csvFolder, "*.csv");
                //        foreach (var item in csvFiles)
                //        {
                //            ExcelConverter.Csv2Js(item);
                //        }
                //        var ccsv = System.IO.Path.Combine(csvFolder, "c.js");
                //        var newccsv = System.IO.Path.Combine(Environment.CurrentDirectory, "c.js");
                //        if (File.Exists(ccsv))
                //        {
                //            if (File.Exists(newccsv)) File.Delete(newccsv);
                //            File.Copy(ccsv, newccsv);
                //            url7 = Directory.GetCurrentDirectory() + "/chart2.html";
                //            browser7.LoadUrl(url7);
                //            foreach (var item in tab1.Items)
                //            {
                //                if (item is TabItem ti)
                //                {
                //                    if (ti.Header.Equals("沉积物厚度图"))
                //                    {
                //                        tab1.SelectedItem = item;
                //                        break;
                //                    }
                //                }
                //            }
                //        }
                //        foreach (var u in csvFiles)
                //        {
                //            foreach (var v in csvFiles)
                //            {
                //                ExcelConverter.U_v2Js(u, v);
                //            }
                //        }
                //        var u_vcsv = System.IO.Path.Combine(csvFolder, "u_v.js");
                //        var newu_vcsv = System.IO.Path.Combine(Environment.CurrentDirectory, "u_v.js");
                //        if (File.Exists(u_vcsv))
                //        {
                //            if (File.Exists(newu_vcsv)) File.Delete(newu_vcsv);
                //            File.Copy(u_vcsv, newu_vcsv);
                //            //url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                //            //browser9.LoadUrl(url9);
                //        }
                //        // 更新UI界面
                //        Application.Current.Dispatcher.Invoke(() =>
                //        {
                //            url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                //            browser9.LoadUrl(url9);
                //            foreach (var item in tab1.Items)
                //            {
                //                if (item is TabItem ti)
                //                {
                //                    if (ti.Header.Equals("测试图"))
                //                    {
                //                        tab1.SelectedItem = item;
                //                        break;
                //                    }
                //                }
                //            }
                //        });
                //    }
                //});


                //20231116的可运行代码，
                //Task.Run(() =>
                //{
                //    // 第一段代码
                //    mm(ltdlnd1, zzlj1, klmd1, ccl1, ltmd1, sd1, lx1, ly1, nx1, ny1, dx, dy, dt1, t1, nt, jingdu1, weidu1, jiaosudu1, k1, kqmd1, fyl1, pjfs1, jd1, bc1, bxj1, f, setting, vc, tcadepth, waveH, waveE, waveK, md1);

                //    // 第二段代码
                //    var csvFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "outcsv");
                //    if (Directory.Exists(csvFolder))
                //    {
                //        string[] csvFiles = Directory.GetFiles(csvFolder, "*.csv");
                //        foreach (var item in csvFiles)
                //        {
                //            ExcelConverter.Csv2Js(item);
                //        }
                //        var ccsv = System.IO.Path.Combine(csvFolder, "c.js");
                //        var newccsv = System.IO.Path.Combine(Environment.CurrentDirectory, "c.js");

                //        foreach (var u in csvFiles)
                //        {
                //            foreach (var v in csvFiles)
                //            {
                //                ExcelConverter.U_v2Js(u, v);
                //            }
                //        }
                //        var u_vcsv = System.IO.Path.Combine(csvFolder, "u_v.js");
                //        var newu_vcsv = System.IO.Path.Combine(Environment.CurrentDirectory, "u_v.js");
                //        if (File.Exists(u_vcsv))
                //        {
                //            if (File.Exists(newu_vcsv)) File.Delete(newu_vcsv);
                //            File.Copy(u_vcsv, newu_vcsv);
                //            //url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                //            //browser9.LoadUrl(url9);
                //        }
                //        // 更新UI界面
                //        Application.Current.Dispatcher.Invoke(() =>
                //        {
                //            url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                //            browser9.LoadUrl(url9);
                //            foreach (var item in tab1.Items)
                //            {
                //                if (item is TabItem ti)
                //                {
                //                    if (ti.Header.Equals("测试图"))
                //                    {
                //                        tab1.SelectedItem = item;
                //                        break;
                //                    }
                //                }
                //            }
                //        });
                //    }
                //});
                Task.Run(() =>
                {
                    // 第一段代码
                    mm(ltdlnd1, zzlj1, klmd1, ccl1, ltmd1, sd1, lx1, ly1, nx1, ny1, dx, dy, dt1, t1, nt, jingdu1, weidu1, jiaosudu1, k1, kqmd1, fyl1, pjfs1, jd1, bc1, bxj1, f, setting, vc, tcadepth, waveH, waveE, waveK, md1);

                    // 第二段代码
                    var csvFolder = System.IO.Path.Combine(Environment.CurrentDirectory, "outcsv");
                    if (Directory.Exists(csvFolder))
                    {
                        string[] csvFiles = Directory.GetFiles(csvFolder, "*.csv");
                        foreach (var item in csvFiles)
                        {
                            if (System.IO.Path.GetFileNameWithoutExtension(item).Equals("d"))
                            {
                                ExcelConverter.Csv_d2Js(item);
                            }
                            else if (System.IO.Path.GetFileNameWithoutExtension(item).Equals("s"))
                            {
                                ExcelConverter.Csv_s2Js(item);
                            }
                            else ExcelConverter.Csv2Js(item);
                        }


                        foreach (var u in csvFiles)
                        {
                            foreach (var v in csvFiles)
                            {
                                ExcelConverter.U_v2Js(u, v);
                            }
                        }
                        var u_vcsv = System.IO.Path.Combine(csvFolder, "u_v.js");
                        var newu_vcsv = System.IO.Path.Combine(Environment.CurrentDirectory, "u_v.js");
                        if (File.Exists(u_vcsv))
                        {
                            if (File.Exists(newu_vcsv)) File.Delete(newu_vcsv);
                            File.Copy(u_vcsv, newu_vcsv);
                            //url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                            //browser9.LoadUrl(url9);
                        }
                        // 更新UI界面
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            url9 = Directory.GetCurrentDirectory() + "/速度场.html";
                            //browser9.LoadUrl(url9);
                            foreach (var item in tab1.Items)
                            {
                                if (item is TabItem ti)
                                {
                                    if (ti.Header.Equals("测试图"))
                                    {
                                        tab1.SelectedItem = item;
                                        break;
                                    }
                                }
                            }
                        });

                        var ccsv = System.IO.Path.Combine(csvFolder, "c.js");
                        var newccsv = System.IO.Path.Combine(Environment.CurrentDirectory, "c.js");
                        if (File.Exists(ccsv))
                        {
                            if (File.Exists(newccsv)) File.Delete(newccsv);
                            File.Copy(ccsv, newccsv);
                            url7 = Directory.GetCurrentDirectory() + "/chart2.html";
                            //browser7.LoadUrl(url7);
                            //foreach (var item in tab1.Items)
                            //{
                            //    if (item is TabItem ti)
                            //    {
                            //        if (ti.Header.Equals("沉积物厚度图"))
                            //        {
                            //            tab1.SelectedItem = item;
                            //            break;
                            //        }
                            //    }
                            //}
                        }

                        var scsv = System.IO.Path.Combine(csvFolder, "s.js");
                        var newscsv = System.IO.Path.Combine(Environment.CurrentDirectory, "s.js");
                        if (File.Exists(scsv))
                        {
                            if (File.Exists(newscsv)) File.Delete(newscsv);
                            File.Copy(scsv, newscsv);
                            url8 = Directory.GetCurrentDirectory() + "/沉积物浓度图新.html";
                            //browser8.LoadUrl(url8);
                            //foreach (var item in tab1.Items)
                            //{
                            //    if (item is TabItem ti)
                            //    {
                            //        if (ti.Header.Equals("沉积物浓度图"))
                            //        {
                            //            tab1.SelectedItem = item;
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                        var dcsv = System.IO.Path.Combine(csvFolder, "d.js");
                        var newdcsv = System.IO.Path.Combine(Environment.CurrentDirectory, "d.js");
                        if (File.Exists(dcsv))
                        {
                            if (File.Exists(newdcsv)) File.Delete(newdcsv);
                            File.Copy(dcsv, newdcsv);
                            url5 = Directory.GetCurrentDirectory() + "/地形图新.html";
                            //browser5.LoadUrl(url5);
                            //foreach (var item in tab1.Items)
                            //{
                            //    if (item is TabItem ti)
                            //    {
                            //        if (ti.Header.Equals("地形图"))
                            //        {
                            //            tab1.SelectedItem = item;
                            //            break;
                            //        }
                            //    }
                            //}
                        }


                    }
                });



            }
            else
            {
                MessageBox.Show("参数不合理");
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {

        }
        string[] treeHeaders = new string[] { "1.网格", "2.地形", "3.边界", "4.水深", "5.流体参数", "6.沉积参数", "7.风浪参数", "8.其它参数", "9.时间设置", "10.模拟计算", "11.图形显示" };
        string[] treeHeaders1 = new string[] { "三维显示", "矢量场图", "剖面图", "热力图", "水深图", "地形图", "沉积物厚度图", "沉积物浓度图", "测试图" };

        //20231220新增{
        // 定义配置文件路径
        private string configFilePath = "config.xml";
        //}
        private void CreateProject(object sender, RoutedEventArgs e)
        {
            var window = new SetProjectNameWindow();
            window.Owner = this;
            if (window.ShowDialog().Value)
            {
                // 获取项目名称、项目类型和更多类型
                string projectName = window.ProjectName;
                string projectType = window.ProjectType;
                string moreType = window.MoreType;

                // 构建完整的文件名
                string fullName = $"{projectName}  {projectType}  {moreType}";

                var note = new Note();
                note.Tag = fullName; // 使用完整的文件名作为 Tag
                note.ChildNotes = new ObservableCollection<Note>();

                foreach (var item in treeHeaders)
                {
                    if (item.Equals("11.图形显示"))
                    {
                        var note1 = new Note();
                        note1.Name = item;
                        note1.Tag = fullName; // 使用完整的文件名作为 Tag
                        note1.ChildNotes = new ObservableCollection<Note>();
                        foreach (var item1 in treeHeaders1)
                        {
                            note1.ChildNotes.Add(new Note() { Name = item1, Tag = fullName }); // 使用完整的文件名作为 Tag
                        }
                        note.ChildNotes.Add(note1);
                    }
                    else
                    {
                        note.ChildNotes.Add(new Note() { Name = item, Tag = fullName }); // 使用完整的文件名作为 Tag
                    }
                }
                //原代码，20231114注释
                //Notes.Add(new Note()
                //{
                //    Name = window.ProjectName,
                //    ChildNotes = new ObservableCollection<Note>()
                //    {
                //        new Note(){ Name="1.网格" },
                //        new Note(){ Name="2.地形" },
                //        new Note(){ Name="3.边界" },
                //        new Note(){ Name="4.水深" },
                //        new Note(){ Name="5.流体参数" },
                //        new Note(){ Name="6.沉积参数" },
                //        new Note(){ Name="7.其它参数" },
                //        new Note(){ Name="8.时间设置" },
                //        new Note(){ Name="9.模拟计算" },
                //        new Note(){ Name="10.图形显示", ChildNotes=new ObservableCollection<Note>()
                //        {
                //        new Note(){ Name="三维显示"},
                //        new Note(){ Name="矢量场图"},
                //        new Note(){ Name="剖面图"},
                //        new Note(){ Name="热力图"},
                //        new Note(){ Name="水深图"},
                //        new Note(){ Name="地形图"},
                //        new Note(){ Name="沉积物厚度图"},
                //        } },
                //    }
                //});
                //20231114修改
                Notes.Add(new Note()
                {
                    Name = window.ProjectName,
                    ChildNotes = new ObservableCollection<Note>()
                    {
                        new Note(){ Name="盆地" },
                        new Note(){ Name="物源" },
                        new Note(){ Name="输移" },
                        new Note(){ Name="水体" },
                        new Note(){ Name="风浪" },
                        new Note(){ Name="模拟" },
                        new Note(){ Name="显示" },
                      //  new Note(){ Name="8.其它参数" },
                       // new Note(){ Name="9.时间设置" },
                       // new Note(){ Name="10.模拟计算" },
                       // new Note(){ Name="11.图形显示", ChildNotes=new ObservableCollection<Note>()
                      //{
                      //  new Note(){ Name="三维显示"},
                      //  new Note(){ Name="矢量场图"},
                       // new Note(){ Name="剖面图"},
                       // new Note(){ Name="热力图"},
                       // new Note(){ Name="水深图"},
                       // new Note(){ Name="地形图"},
                       // new Note(){ Name="沉积物厚度图"},
                      //  new Note(){ Name="沉积物浓度图"},
                       // new Note(){ Name="测试图"},
                     //   } },
                    }
                });
            }
        }
        //20231220新增{


        // 保存项目按钮事件
        private void SaveProject(object sender, RoutedEventArgs e)
        {
            // 创建 XML 文档
            XDocument document = new XDocument(
                new XElement("parameters.xml",
                    new XElement("lx1", lx.Text),
                    new XElement("ly2", ly.Text),
                    new XElement("Nx3", nx.Text),
                    new XElement("Ny4", ny.Text),
                    new XElement("dt5", dt.Text),
                    new XElement("t6", t.Text),
                    new XElement("jingdu7", jingdu.Text),
                    new XElement("weidu8", weidu.Text),
                    new XElement("jiaosudu9", jiaosudu.Text),
                    new XElement("ltdlnd10", ltdlnd.Text),
                    new XElement("ltmd11", ltmd.Text),
                    new XElement("sd12", sd.Text),
                    new XElement("k13", k.Text),
                    new XElement("klmd14", klmd.Text),
                    new XElement("zzlj15", zzlj.Text),
                    new XElement("ccl16", ccl.Text),
                    new XElement("csnsmd17", csnsmd.Text),
                    new XElement("kqmd18", kqmd.Text),
                    new XElement("fyl19", fyl.Text),
                    new XElement("pjfs20", pjfs.Text),
                    new XElement("jd21", jd.Text),
                    new XElement("bc22", bc.Text),
                    new XElement("bxj23", bxj.Text)
                )
            );

            // 保存 XML 文档
            document.Save(configFilePath);

            if (Program1.SelectedItem == null || !(Program1.SelectedItem is Note))
            {
                MessageBox.Show("请先选择一个项目进行保存！");
                return;
            }
            var file = new SaveFileDialog();

            file.Filter = "*.ini|*.ini";
            if (file.ShowDialog() == true)
            {
                //File.Create(file.FileName);
                foreach (var item in Notes)
                {
                    if (item.Tag != ((Note)(Program1.SelectedItem)).Tag)
                    {
                        continue;
                    }
                    item.Path = file.FileName;
                    foreach (var child in item.ChildNotes)
                    {
                        child.Path = file.FileName;
                    }
                }
                SaveConfig(((Note)(Program1.SelectedItem)).Tag, file.FileName);
                INIHelper.Write("projectName", "projectName", ((Note)(Program1.SelectedItem)).Tag, file.FileName);

            }
        }

        // 打开项目按钮事件
        private void OpenProject(object sender, RoutedEventArgs e)
        {
            // 加载 XML 文档
            XDocument document = XDocument.Load(configFilePath);

            // 读取参数值
            string lx1 = document.Root.Element("lx1").Value;
            string ly2 = document.Root.Element("ly2").Value;
            string Nx3 = document.Root.Element("Nx3").Value;
            string Ny4 = document.Root.Element("Ny4").Value;
            string dt5 = document.Root.Element("dt5").Value;
            string t6 = document.Root.Element("t6").Value;
            string jingdu7 = document.Root.Element("jingdu7").Value;
            string weidu8 = document.Root.Element("weidu8").Value;
            string jiaosudu9 = document.Root.Element("jiaosudu9").Value;
            string ltdlnd10 = document.Root.Element("ltdlnd10").Value;
            string ltmd11 = document.Root.Element("ltmd11").Value;
            string sd12 = document.Root.Element("sd12").Value;
            string k13 = document.Root.Element("k13").Value;
            string klmd14 = document.Root.Element("klmd14").Value;
            string zzlj15 = document.Root.Element("zzlj15").Value;
            string ccl16 = document.Root.Element("ccl16").Value;
            string csnsmd17 = document.Root.Element("csnsmd17").Value;
            string kqmd18 = document.Root.Element("kqmd18").Value;
            string fyl19 = document.Root.Element("fyl19").Value;
            string pjfs20 = document.Root.Element("pjfs20").Value;
            string jd21 = document.Root.Element("jd21").Value;
            string bc22 = document.Root.Element("bc22").Value;
            string bxj23 = document.Root.Element("bxj23").Value;

            // 将参数值填充到控件中
            lx.Text = lx1;
            ly.Text = ly2;
            nx.Text = Nx3;
            ny.Text = Ny4;
            dt.Text = dt5;
            t.Text = t6;
            jingdu.Text = jingdu7;
            weidu.Text = weidu8;
            jiaosudu.Text = jiaosudu9;
            ltdlnd.Text = ltdlnd10;
            ltmd.Text = ltmd11;
            sd.Text = sd12;
            k.Text = k13;
            klmd.Text = klmd14;
            zzlj.Text = zzlj15;
            ccl.Text = ccl16;
            csnsmd.Text = csnsmd17;
            kqmd.Text = kqmd18;
            fyl.Text = fyl19;
            pjfs.Text = pjfs20;
            jd.Text = jd21;
            bc.Text = bc22;
            bxj.Text = bxj23;
            var dialog = new OpenFileDialog();
            dialog.Filter = "*.ini|*.ini";
            if (dialog.ShowDialog() == true)
            {
                var projectName = INIHelper.Read("projectName", "projectName", "", dialog.FileName);
                Notes.Add(new Note()
                {
                    Name = projectName,
                    Tag = projectName,
                    Path = dialog.FileName,
                    ChildNotes = new ObservableCollection<Note>()
                    {
                        new Note(){ Name="1.网格", Tag = projectName,Path = dialog.FileName  },
                        new Note(){ Name="2.地形" , Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="3.边界", Tag = projectName,Path = dialog.FileName  },
                        new Note(){ Name="4.水深" , Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="5.流体参数" , Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="6.沉积参数", Tag = projectName,Path = dialog.FileName  },
                        new Note(){ Name="7.风浪参数", Tag = projectName ,Path = dialog.FileName },
                        new Note(){ Name="8.其它参数", Tag = projectName,Path = dialog.FileName  },
                        new Note(){ Name="9.时间设置" , Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="10.模拟计算", Tag = projectName,Path = dialog.FileName  },
                        new Note(){ Name="11.图形显示",Tag = projectName ,Path = dialog.FileName, ChildNotes=new ObservableCollection<Note>()
                        {
                        new Note(){ Name="三维显示", Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="矢量场图", Tag = projectName ,Path = dialog.FileName},
                        new Note(){ Name="剖面图", Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="热力图", Tag = projectName ,Path = dialog.FileName},
                        new Note(){ Name="水深图", Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="地形图", Tag = projectName ,Path = dialog.FileName},
                        new Note(){ Name="沉积物厚度图", Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="沉积物浓度图", Tag = projectName,Path = dialog.FileName },
                        new Note(){ Name="测试图", Tag = projectName,Path = dialog.FileName },
                        } },
                    }
                });
            }

        }
        //}20231220新增

        //private string file = AppDomain.CurrentDomain.BaseDirectory + "\\config.ini";
        private void GetConfig(string projectName, string file)
        {
            lx.Text = INIHelper.Read(projectName, "lx", "", file);
            ly.Text = INIHelper.Read(projectName, "ly", "", file);
            nx.Text = INIHelper.Read(projectName, "nx", "", file);
            ny.Text = INIHelper.Read(projectName, "ny", "", file);
            dt.Text = INIHelper.Read(projectName, "dt", "", file);
            t.Text = INIHelper.Read(projectName, "t", "", file);
            jingdu.Text = INIHelper.Read(projectName, "jingdu", "", file);
            weidu.Text = INIHelper.Read(projectName, "weidu", "", file);
            jiaosudu.Text = INIHelper.Read(projectName, "jiaosudu", "", file);
            ltdlnd.Text = INIHelper.Read(projectName, "ltdlnd", "", file);
            ltmd.Text = INIHelper.Read(projectName, "ltmd", "", file);
            sd.Text = INIHelper.Read(projectName, "sd", "", file);
            k.Text = INIHelper.Read(projectName, "k", "", file);
            klmd.Text = INIHelper.Read(projectName, "klmd", "", file);
            zzlj.Text = INIHelper.Read(projectName, "zzlj", "", file);
            ccl.Text = INIHelper.Read(projectName, "ccl", "", file);
            csnsmd.Text = INIHelper.Read(projectName, "csnsmd", "", file);
            kqmd.Text = INIHelper.Read(projectName, "kqmd", "", file);
            fyl.Text = INIHelper.Read(projectName, "fyl", "", file);
            pjfs.Text = INIHelper.Read(projectName, "pjfs", "", file);
            jd.Text = INIHelper.Read(projectName, "jd", "", file);
            bc.Text = INIHelper.Read(projectName, "bc", "", file);
            bxj.Text = INIHelper.Read(projectName, "bxj", "", file);
        }

        private void SaveConfig(string projectName, string file)
        {
            INIHelper.Write(projectName, "lx", lx.Text, file);
            INIHelper.Write(projectName, "ly", ly.Text, file);
            INIHelper.Write(projectName, "nx", nx.Text, file);
            INIHelper.Write(projectName, "ny", ny.Text, file);
            INIHelper.Write(projectName, "dt", dt.Text, file);
            INIHelper.Write(projectName, "t", t.Text, file);
            INIHelper.Write(projectName, "jingdu", jingdu.Text, file);
            INIHelper.Write(projectName, "weidu", weidu.Text, file);
            INIHelper.Write(projectName, "jiaosudu", jiaosudu.Text, file);
            INIHelper.Write(projectName, "ltdlnd", ltdlnd.Text, file);
            INIHelper.Write(projectName, "ltmd", ltmd.Text, file);
            INIHelper.Write(projectName, "sd", sd.Text, file);
            INIHelper.Write(projectName, "k", k.Text, file);
            INIHelper.Write(projectName, "klmd", klmd.Text, file);
            INIHelper.Write(projectName, "zzlj", zzlj.Text, file);
            INIHelper.Write(projectName, "ccl", ccl.Text, file);
            INIHelper.Write(projectName, "csnsmd", csnsmd.Text, file);
            INIHelper.Write(projectName, "kqmd", kqmd.Text, file);
            INIHelper.Write(projectName, "fyl", fyl.Text, file);
            INIHelper.Write(projectName, "pjfs", pjfs.Text, file);
            INIHelper.Write(projectName, "jd", jd.Text, file);
            INIHelper.Write(projectName, "bc", bc.Text, file);
            INIHelper.Write(projectName, "bxj", bxj.Text, file);
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseProject(object sender, RoutedEventArgs e)
        {
            if (Program1.SelectedItem != null && Program1.SelectedItem is Note note)
            {
                Notes.Remove(note);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            var result = openFileDialog.ShowDialog();
            if (!result.Value) return;
            var excel = ExcelConverter.ConvertToXYZExcel(openFileDialog.FileName);
            if (!string.IsNullOrEmpty(excel))
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
                var result1 = saveFileDialog.ShowDialog();
                if (!result1.Value) return;
                File.Copy(excel, saveFileDialog.FileName);
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Excel File (*.xlsx)|*.xlsx";
            var result = openFileDialog.ShowDialog();
            if (!result.Value) return;
            var json = ExcelConverter.XYZExcelToJs(openFileDialog.FileName);
            if (!string.IsNullOrEmpty(json))
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "JSon File (*.json)|*.json";
                var result1 = saveFileDialog.ShowDialog();
                if (!result1.Value) return;
                File.Copy(json, saveFileDialog.FileName);
            }
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_4(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_5(object sender, TextChangedEventArgs e)
        {

        }

        private void klmd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisible(true)]//给予权限并设置可见

    public class Program
    {
        //  begin
        [DllImport("tranlsbasic20231008.dll",
        EntryPoint = "main",
        CharSet = CharSet.Ansi,
        CallingConvention = CallingConvention.StdCall)]
        public static extern int main();
        //end  

        //static void Main(string[] args)
        //{
        //    main();
        //}
    }


    public class WebAdapter
    {
        private ChromiumWebBrowser browser;
        public WebAdapter(ChromiumWebBrowser browser)
        {
            this.browser = browser;
        }

        public void ShowMsg(string Msg)
        {
            MessageBox.Show(Msg);
        }
    }
    public class Note : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string tag;
        public string Tag
        {
            get => tag;
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }
        private string path;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }
        private ObservableCollection<Note> childNotes;
        public ObservableCollection<Note> ChildNotes
        {
            get => childNotes;
            set
            {
                childNotes = value;
                OnPropertyChanged("ChildNotes");
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
    public class MyData : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private static MyData instance = null;
        public static MyData Instance = instance ?? new MyData();
        private double klmd;
        public double Klmd
        {
            get => klmd;
            set
            {
                klmd = value;
                OnPropertyChanged();
            }
        }
        private double ltmd;
        public double Ltmd
        {
            get => ltmd;
            set
            {
                ltmd = value;
                OnPropertyChanged();
            }
        }
        private double zzlj;
        public double Zzlj
        {
            get => zzlj;
            set
            {
                zzlj = value;
                OnPropertyChanged();
            }
        }
        private double ltdlnd;
        public double Ltdlnd
        {
            get => ltdlnd;
            set
            {
                ltdlnd = value;
                OnPropertyChanged();
            }
        }
        private double ccl;
        public double Ccl
        {
            get => ccl;
            set
            {
                ccl = value;
                OnPropertyChanged();
            }
        }
        private double csnsmd;
        public double Csnsmd
        {
            get => csnsmd;
            set
            {
                csnsmd = value;
                OnPropertyChanged();
            }
        }
        private double sd;
        public double Sd
        {
            get => sd;
            set
            {
                sd = value;
                OnPropertyChanged();
            }
        }
        private string type;
        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }
        private double lx;
        public double Lx
        {
            get => lx;
            set
            {
                lx = value;
                OnPropertyChanged();
            }
        }
        private double ly;
        public double Ly
        {
            get => ly;
            set
            {
                ly = value;
                OnPropertyChanged();
            }
        }
        private double nx;
        public double Nx
        {
            get => nx;
            set
            {
                nx = value;
                OnPropertyChanged();
            }
        }
        private double ny;
        public double Ny
        {
            get => ny;
            set
            {
                ny = value;
                OnPropertyChanged();
            }
        }
        private double dt;
        public double Dt
        {
            get => dt;
            set
            {
                dt = value;
                OnPropertyChanged();
            }
        }
        private double t;
        public double T
        {
            get => t;
            set
            {
                t = value;
                OnPropertyChanged();
            }
        }
        private double jingdu;
        public double Jingdu
        {
            get => jingdu;
            set
            {
                jingdu = value;
                OnPropertyChanged();

            }

        }
        private double weidu;
        public double Weidu
        {
            get => weidu;
            set
            {
                weidu = value;
                OnPropertyChanged();

            }

        }
        private double jiaosudu;
        public double Jiaosudu
        {
            get => jiaosudu;
            set
            {
                jiaosudu = value;
                OnPropertyChanged();

            }

        }
        private double k;
        public double K
        {
            get => k;
            set
            {
                k = value;
                OnPropertyChanged();

            }

        }
        private double kqmd;
        public double Kqmd
        {
            get => kqmd;
            set
            {
                kqmd = value;
                OnPropertyChanged();

            }

        }
        private double fyl;
        public double Fyl
        {
            get => fyl;
            set
            {
                fyl = value;
                OnPropertyChanged();

            }

        }
        private double pjfs;
        public double Pjfs
        {
            get => pjfs;
            set
            {
                pjfs = value;
                OnPropertyChanged();

            }

        }
        private double jd;
        public double Jd
        {
            get => jd;
            set
            {
                jd = value;
                OnPropertyChanged();

            }

        }
        private double bc;
        public double Bc
        {
            get => bc;
            set
            {
                bc = value;
                OnPropertyChanged();

            }

        }
        private double bxj;
        public double Bxj
        {
            get => bxj;
            set
            {
                bxj = value;
                OnPropertyChanged();

            }

        }
        private double waveH;
        public double WaveH
        {
            get => waveH;
            set
            {
                waveH = value;
                OnPropertyChanged();

            }

        }
        private double md;
        public double Md
        {
            get => md;
            set
            {
                md = value;
                OnPropertyChanged();

            }

        }

    }
}



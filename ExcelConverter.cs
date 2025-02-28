using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ExcelDataReader;
using Microsoft.Win32;
using MiniExcelLibs.Attributes;
using Newtonsoft.Json;

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
        
        var newxlsx = Path.Combine(Path.GetDirectoryName(filename), $"{Path.GetFileNameWithoutExtension(filename)}_NEW{Path.GetExtension(filename)}");
        if (File.Exists(newxlsx))File.Delete(newxlsx);
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
                vector3s.Add(new Vector3() { x = i, y = j, z = temp[j][i] });
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
    //public static string XYZExcelToJson(string xyzExcelFileName)
    //{
    //    var jsonPath = xyzExcelFileName.Replace(Path.GetExtension(xyzExcelFileName), ".json");
    //    var list = MiniExcelLibs.MiniExcel.Query<Vector3>(xyzExcelFileName).ToList();
    //    File.WriteAllText(jsonPath, JsonConvert.SerializeObject(list.ToArray()));
    //                        return jsonPath;
    //}
    public static string XYZExcelToJs(string xyzExcelFileName)
    {
        var jsPath = xyzExcelFileName.Replace(Path.GetExtension(xyzExcelFileName), ".js");
        var list = MiniExcelLibs.MiniExcel.Query<Vector3>(xyzExcelFileName).ToList();
        File.WriteAllText(jsPath, JsonConvert.SerializeObject(list.ToArray()));
        return jsPath;
    }

    internal static void ConvertToXYZExcel(object csv)
    {
        throw new NotImplementedException();
    }

    public class Vector3
    {
        [ExcelColumnName("X")]
        public double x { get; set; }
        [ExcelColumnName("Y")]
        public double y { get; set; }
        [ExcelColumnName("Z")]
        public double z { get; set; }
    }
}

public class Program
{
    


    public static void Main1(string[] args)
    {
        string filePath = "C:\\Users\\221-2\\Desktop\\20230906beifen\\EChartsTest 3D\\EChartsTest\\速度场v.xlsx";
        //调这个方法生成新的excel
        var newxlsx= ExcelConverter.ConvertToXYZExcel(filePath);
        //传入新的excel路径生成json
        ExcelConverter.XYZExcelToJs(newxlsx);



        DataTable result = ExcelConverter.ConvertToXYZ(filePath);

        // 打印转换后的数据
        foreach (DataRow row in result.Rows)
        {
            Console.WriteLine($"{row["x"]}, {row["y"]}, {row["z"]}");
        }
    }
}

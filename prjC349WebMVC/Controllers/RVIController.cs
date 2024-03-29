﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using prjC349WebMVC.Models;
using System.Data;
using PagedList;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using NPOI.XSSF.UserModel;

namespace prjC349WebMVC.Controllers
{

    public class RVIController : Controller
    {
        string connStr = WebConfigurationManager.ConnectionStrings["c349ConnectionString"].ConnectionString;
        int pageSize = 10;
        public ActionResult Index(string queryMonth, int page = 1)
        {
            //if (queryMonth == null)
            //{
            //    queryMonth = DateTime.Now.Month.ToString().PadLeft(2,'0')+"/"+ DateTime.Now.Year.ToString();
            //}
            ViewBag.queryMonth = queryMonth;
            int currentPage = page < 1 ? 1 : page;
            var result = GetAllRecrods(queryMonth).ToPagedList(currentPage, pageSize);
            //ViewBag.Cookie = Request.Cookies.AllKeys[0];
            return View(result);
        }

        public ActionResult Create()
        {

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem {Text="text-1", Value="value-1" },
                new SelectListItem {Text="text-2", Value="value-2" },
                new SelectListItem {Text="text-3", Value="value-3" },
            };

            //預設選擇哪一筆
            selectList.Where(q => q.Value == "value-2").First().Selected = true;

            ViewBag.SelectList = selectList;
            return View();
        }
        [HttpPost]
        public ActionResult Create(remote_visual_inspection remote_visua_inspection)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Error = false;
                try
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "INSERT INTO remote_visua_inspection(tdate,carId,comment1,comment2,coil1,coil2,coil3,coil4,coil5,coil6,coil7,coil8,creator,updateTime,ip)" +
                        "VALUES(@tdate,@carId,@comment1,@comment2,@coil1,@coil2,@coil3,@coil4,@coil5,@coil6,@coil7,@coil8,@creator,@updateTime,@ip)";
                    //cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.VarChar)).Value = employee.id;
                    cmd.Parameters.Add(new MySqlParameter("@tdate", MySqlDbType.DateTime)).Value = DateTime.Parse(remote_visua_inspection.tdate.ToString())
                        .AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                    cmd.Parameters.Add(new MySqlParameter("@carId", MySqlDbType.VarChar)).Value = remote_visua_inspection.carId;
                    cmd.Parameters.Add(new MySqlParameter("@comment1", MySqlDbType.VarChar)).Value = remote_visua_inspection.comment1;
                    cmd.Parameters.Add(new MySqlParameter("@comment2", MySqlDbType.VarChar)).Value = remote_visua_inspection.comment2;
                    cmd.Parameters.Add(new MySqlParameter("@coil1", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil1;
                    cmd.Parameters.Add(new MySqlParameter("@coil2", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil2;
                    cmd.Parameters.Add(new MySqlParameter("@coil3", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil3;
                    cmd.Parameters.Add(new MySqlParameter("@coil4", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil4;
                    cmd.Parameters.Add(new MySqlParameter("@coil5", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil5;
                    cmd.Parameters.Add(new MySqlParameter("@coil6", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil6;
                    cmd.Parameters.Add(new MySqlParameter("@coil7", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil7;
                    cmd.Parameters.Add(new MySqlParameter("@coil8", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil8;

                    if (IPAddress.Get() == "::1")
                    {
                        cmd.Parameters.Add(new MySqlParameter("@creator", MySqlDbType.VarChar)).Value = "test";
                    }
                    else
                    {
                        cmd.Parameters.Add(new MySqlParameter("@creator", MySqlDbType.VarChar)).Value = Request.Cookies["uid"].Value;
                    }
                    cmd.Parameters.Add(new MySqlParameter("@updateTime", MySqlDbType.DateTime)).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add(new MySqlParameter("@ip", MySqlDbType.VarChar)).Value = IPAddress.Get();
                    ExecuteCmd(cmd);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    return View(remote_visua_inspection);
                }
            }
            return View(remote_visua_inspection);
        }
        public ActionResult Edit(string id)
        {

            return View(GetRecrod(id));
        }
        [HttpPost]
        public ActionResult Edit(remote_visual_inspection remote_visua_inspection)
        {
            if (ModelState.IsValid)
            {
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = "UPDATE remote_visua_inspection SET tdate=@tdate, carId=@carId,comment1=@comment1, comment2=@comment2, " +
                    "coil1=@coil1, coil2=@coil2, coil3=@coil3, coil4=@coil4, coil5=@coil5, coil6=@coil6, coil7=@coil7 ,coil8=@coil8,updateTime=@updateTime WHERE id=@id";
                cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.VarChar)).Value = remote_visua_inspection.id;
                cmd.Parameters.Add(new MySqlParameter("@tdate", MySqlDbType.Date)).Value = remote_visua_inspection.tdate;
                cmd.Parameters.Add(new MySqlParameter("@carId", MySqlDbType.VarChar)).Value = remote_visua_inspection.carId;
                cmd.Parameters.Add(new MySqlParameter("@comment1", MySqlDbType.VarChar)).Value = remote_visua_inspection.comment1;
                cmd.Parameters.Add(new MySqlParameter("@comment2", MySqlDbType.VarChar)).Value = remote_visua_inspection.comment2;
                cmd.Parameters.Add(new MySqlParameter("@coil1", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil1;
                cmd.Parameters.Add(new MySqlParameter("@coil2", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil2;
                cmd.Parameters.Add(new MySqlParameter("@coil3", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil3;
                cmd.Parameters.Add(new MySqlParameter("@coil4", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil4;
                cmd.Parameters.Add(new MySqlParameter("@coil5", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil5;
                cmd.Parameters.Add(new MySqlParameter("@coil6", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil6;
                cmd.Parameters.Add(new MySqlParameter("@coil7", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil7;
                cmd.Parameters.Add(new MySqlParameter("@coil8", MySqlDbType.VarChar)).Value = remote_visua_inspection.coil8;
                cmd.Parameters.Add(new MySqlParameter("@updateTime", MySqlDbType.DateTime)).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.Add(new MySqlParameter("@ip", MySqlDbType.VarChar)).Value = IPAddress.Get();


                ExecuteCmd(cmd);
                return RedirectToAction("Index");
            }
            return View(remote_visua_inspection);
        }
        public ActionResult Delete(string id)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "DELETE FROM remote_visua_inspection WHERE id=@id";
            cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.VarChar)).Value = id;

            ExecuteCmd(cmd);
            return RedirectToAction("Index");
        }
        // GET: Home
        private List<remote_visual_inspection> GetAllRecrods(string queryMonth)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.ConnectionString = connStr;
            string SQL = "";
            if (queryMonth == "" || queryMonth == null)
            {
                SQL = $"SELECT * FROM remote_visua_inspection ORDER BY id DESC";
            }
            else
            {
                SQL = $"SELECT * FROM remote_visua_inspection WHERE MONTH(tdate) = {DateTime.Parse(queryMonth).Month} AND YEAR(tdate) = {DateTime.Parse(queryMonth).Year} ORDER BY id DESC";
            }
            MySqlDataAdapter adp = new MySqlDataAdapter(SQL, conn);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            DataTable dt = ds.Tables[0];
            List<remote_visual_inspection> employees = new List<remote_visual_inspection>();
            ViewBag.Count = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var yyy = dt.Rows[i]["comment1"].ToString();
                employees.Add(new remote_visual_inspection
                {
                    id = dt.Rows[i]["id"].ToString(),
                    tdate = DateTime.Parse(dt.Rows[i]["tdate"].ToString()),
                    carId = dt.Rows[i]["carId"].ToString(),
                    comment1 = dt.Rows[i]["comment1"].ToString(),
                    comment2 = dt.Rows[i]["comment2"].ToString(),
                    coil1 = dt.Rows[i]["coil1"].ToString(),
                    coil2 = dt.Rows[i]["coil2"].ToString(),
                    coil3 = dt.Rows[i]["coil3"].ToString(),
                    coil4 = dt.Rows[i]["coil4"].ToString(),
                    coil5 = dt.Rows[i]["coil5"].ToString(),
                    coil6 = dt.Rows[i]["coil6"].ToString(),
                    coil7 = dt.Rows[i]["coil7"].ToString(),
                    coil8 = dt.Rows[i]["coil8"].ToString(),
                    creator = dt.Rows[i]["creator"].ToString(),
                    updateTime = DateTime.Parse(dt.Rows[i]["updateTime"].ToString()),
                    ip = dt.Rows[i]["ip"].ToString()
                });
                ViewBag.Count++;
            }
            return employees;
        }
        private remote_visual_inspection GetRecrod(string id)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.ConnectionString = connStr;
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM remote_visua_inspection WHERE id=@id", conn);
            cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.VarChar)).Value = id;
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            DataTable dt = ds.Tables[0];
            remote_visual_inspection emp = new remote_visual_inspection
            {
                id = dt.Rows[0]["id"].ToString(),
                tdate = DateTime.Parse(dt.Rows[0]["tdate"].ToString()),
                carId = dt.Rows[0]["carId"].ToString(),
                comment1 = dt.Rows[0]["comment1"].ToString(),
                comment2 = dt.Rows[0]["comment2"].ToString(),
                coil1 = dt.Rows[0]["coil1"].ToString(),
                coil2 = dt.Rows[0]["coil2"].ToString(),
                coil3 = dt.Rows[0]["coil3"].ToString(),
                coil4 = dt.Rows[0]["coil4"].ToString(),
                coil5 = dt.Rows[0]["coil5"].ToString(),
                coil6 = dt.Rows[0]["coil6"].ToString(),
                coil7 = dt.Rows[0]["coil7"].ToString(),
                coil8 = dt.Rows[0]["coil8"].ToString(),
                creator = dt.Rows[0]["creator"].ToString(),
                updateTime = DateTime.Parse(dt.Rows[0]["updateTime"].ToString()),
                ip = dt.Rows[0]["ip"].ToString()
            };
            return emp;
        }
        private void ExecuteCmd(MySqlCommand cmd)
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            cmd.Connection = conn;
            var yy = cmd.ExecuteNonQuery();
            conn.Close();
        }


        class Member
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public ActionResult ExporttoExcel()
        {
            List<remote_visual_inspection> remote_Visua_Inspections = GetAllRecrods("");


            //建立Excel
            XSSFWorkbook hssfworkbook = new XSSFWorkbook(); //建立活頁簿
            ISheet sheet = hssfworkbook.CreateSheet("sheet1"); //建立sheet

            //設定樣式
            ICellStyle headerStyle = hssfworkbook.CreateCellStyle();
            IFont headerfont = hssfworkbook.CreateFont();
            headerStyle.Alignment = HorizontalAlignment.Center; //水平置中
            headerStyle.VerticalAlignment = VerticalAlignment.Center; //垂直置中
            headerfont.FontName = "微軟正黑體";
            headerfont.FontHeightInPoints = 20;
            headerfont.Boldweight = (short)FontBoldWeight.Bold;
            headerStyle.SetFont(headerfont);

            List<string> strList = new List<string> { "紀錄編號", "建立日期", "備註1", "備註2", "鋼捲1", "鋼捲2", "鋼捲3", "鋼捲4", "鋼捲5", "鋼捲6", "鋼捲7", "鋼捲8","載運車牌","輸入者","更新時間","IP" };
            for (int i = 0; i < strList.Count; i++)
            {
                if (i == 0)
                    sheet.CreateRow(0).CreateCell(i).SetCellValue(strList[i]);
                else
                    sheet.GetRow(0).CreateCell(i).SetCellValue(strList[i]);
            }



            //填入資料
            int rowIndex = 1;
            foreach (var record in remote_Visua_Inspections)
            {
                sheet.CreateRow(rowIndex).CreateCell(0).SetCellValue(record.id);
                sheet.SetColumnWidth(0, 6 * 2 * 256);  //寬度調整
                sheet.GetRow(rowIndex).CreateCell(1).SetCellValue(record.tdate.ToString());
                sheet.SetColumnWidth(1, 7 * 2 * 256);  //寬度調整
                sheet.GetRow(rowIndex).CreateCell(2).SetCellValue(record.comment1);
                sheet.SetColumnWidth(2, 10 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(3).SetCellValue(record.comment2);
                sheet.SetColumnWidth(3, 10 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(4).SetCellValue(record.coil1);
                sheet.SetColumnWidth(4, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(5).SetCellValue(record.coil2);
                sheet.SetColumnWidth(5, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(6).SetCellValue(record.coil3);
                sheet.SetColumnWidth(6, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(7).SetCellValue(record.coil4);
                sheet.SetColumnWidth(7, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(8).SetCellValue(record.coil5);
                sheet.SetColumnWidth(8, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(9).SetCellValue(record.coil6);
                sheet.SetColumnWidth(9, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(10).SetCellValue(record.coil7);
                sheet.SetColumnWidth(10, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(11).SetCellValue(record.coil8);
                sheet.SetColumnWidth(11, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(12).SetCellValue(record.carId);
                sheet.SetColumnWidth(12, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(13).SetCellValue(record.creator);
                sheet.SetColumnWidth(13, 6 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(14).SetCellValue(record.updateTime.ToString());
                sheet.SetColumnWidth(14, 7 * 2 * 256);   //寬度調整
                sheet.GetRow(rowIndex).CreateCell(15).SetCellValue(record.ip);
                sheet.SetColumnWidth(15, 6 * 2 * 256);   //寬度調整
                rowIndex++;
            }
            var excelDatas = new MemoryStream();
            hssfworkbook.Write(excelDatas);
            return File(excelDatas.ToArray(), "application/vnd.ms-excel", string.Format($"紀錄清單.xlsx"));
        }
    }
}
using ClosedXML.Excel;
using LogiTax_Avinash.Models;
using LogiTax_Avinash.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogiTax_Avinash.Controllers
{
    public class DataController : Controller
    {
        private UploadDataService service = new UploadDataService();
        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        // GET: Data
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public FileResult ExportToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[8] { new DataColumn("SrNo"),
                                                     new DataColumn("Company Name"),
                                                     new DataColumn("GSTIN"),
                                                     new DataColumn("Start Date"),
                                                     new DataColumn("End Date"),
                                                     new DataColumn("Turnover Amount"),
                                                     new DataColumn("Contact Email"),
                                                     new DataColumn("Contact Number") });

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                }
            }
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            List<DataSummary> uploadDatasList = new List<DataSummary>();
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls":
                        conString = Excel03ConString;
                        break;
                    case ".xlsx":
                        conString = Excel07ConString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                
                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    connExcel.Open();
                    using (OleDbCommand cmdExcel = connExcel.CreateCommand())
                    {
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        var sheetname = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        cmdExcel.CommandText = "SELECT * From ["+ sheetname + "]";
                        using (var dr = cmdExcel.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                uploadDatasList.Add(new DataSummary()
                                {
                                    UploadData = new UploadData()
                                    {
                                        CompanyName = dr.GetValue(dr.GetOrdinal("Company Name")) != (object)DBNull.Value ? dr.GetString(dr.GetOrdinal("Company Name")) : string.Empty,
                                        ContactEmail = dr.GetValue(dr.GetOrdinal("Contact Email")) != (object)DBNull.Value ? dr.GetString(dr.GetOrdinal("Contact Email")) : string.Empty,
                                        ContactNumber = dr.GetValue(dr.GetOrdinal("Contact Number")) != (object)DBNull.Value ? dr.GetValue(dr.GetOrdinal("Contact Number")).ToString() : string.Empty,
                                        EndDate =dr.TryGetDateTime(dr.GetOrdinal("End Date")),
                                        StartDate = dr.TryGetDateTime(dr.GetOrdinal("Start Date")),
                                        GSTIN = dr.GetValue(dr.GetOrdinal("GSTIN")) != (object)DBNull.Value ? dr.GetValue(dr.GetOrdinal("GSTIN")).ToString() : string.Empty,
                                        TurnoverAmount= dr.TryGetDecimal(dr.GetOrdinal("Turnover Amount")),
                                       
                                       
                                    }
                                });
                            }
                        }
                    }
                }
                foreach(var model in uploadDatasList)
                {
                    var context = new ValidationContext(model.UploadData);
                    model.IsValid = Validator.TryValidateObject(model.UploadData, context, model.results, true);

                    if (model.IsValid)
                    {
                        service.Save(model.UploadData, "insert");
                        if(service.Errors.Count>0)
                        {
                            model.IsDuplicateData = true;
                        }
                    }
                }
            }

            return View("DataSummary", uploadDatasList);
        }
    }

    public static class HelperDataReader
    {
        public static DateTime TryGetDateTime(this IDataReader dr,int o)
        {
            var date = new DateTime();
            if (dr.GetValue(o) != null)
            {
                DateTime.TryParse(dr.GetValue(o).ToString(), out date);
            }
            return date;
        }

        public static Decimal TryGetDecimal(this IDataReader dr, int o)
        {
            decimal data =0 ;
            if (dr.GetValue(o) != null)
            {
                Decimal.TryParse(dr.GetValue(o).ToString().Replace(",", ""), out data);
            }
            return data;
        }
    }
}

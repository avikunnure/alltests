using LogiTax_Avinash.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LogiTax_Avinash.Services
{
    public class UploadDataService
    {
        private ExcelDataContext db = new ExcelDataContext();
        public List<string> Errors = new List<string>(); 
        public List<UploadData> GetList()
        {
           return db.UploadDatas.ToList();
        }

        public UploadData Get(Guid id)
        {
            UploadData uploadData = db.UploadDatas.Find(id);
            return uploadData;
        }
        public bool IsDuplicates(UploadData uploadData)
        {
            if (db.UploadDatas.Where(x => x.ContactEmail.Equals(uploadData.ContactEmail, StringComparison.InvariantCultureIgnoreCase) && x.Id!=uploadData.Id).Count() > 0)
            {
                Errors.Add("ContactEmail are duplicate");
                return true;
            }
            if (db.UploadDatas.Where(x => x.GSTIN.Equals(uploadData.GSTIN, StringComparison.InvariantCultureIgnoreCase) && x.Id != uploadData.Id).Count() > 0)
            {
                Errors.Add("GSTIN are duplicate");
                return true;
            }
            if (db.UploadDatas.Where(x => x.ContactNumber.Equals(uploadData.ContactNumber, StringComparison.InvariantCultureIgnoreCase) && x.Id != uploadData.Id).Count() > 0)
            {
                Errors.Add("ContactNumber are duplicate");
                return true;
            }
            return false;
        }

        public bool Save(UploadData uploadData,string mode)
        {
            Errors = new List<string>();
            switch (mode.ToLower())
            {
                case "insert":
                    if (IsDuplicates(uploadData) !=true)
                    {
                        uploadData.Id = Guid.NewGuid();
                        db.UploadDatas.Add(uploadData);
                        db.SaveChanges();
                    }
                    break;
                case "update":
                    if (IsDuplicates(uploadData) != true)
                    {
                        db.Entry(uploadData).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    break;
                case "delete":
                    db.UploadDatas.Remove(uploadData);
                    db.SaveChanges();
                    break;
            }
            return Errors.Count==0;
        }
    }
}
namespace LogiTax_Avinash.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ExcelDataContext : DbContext
    {
        public ExcelDataContext()
            : base("name=DataModel")
        {
        }

        public virtual DbSet<UploadData> UploadDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FCFDFE.Entity.CIMSModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CIMSEntities : DbContext
    {
        public CIMSEntities()
            : base("name=CIMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<INDEXDATA> INDEXDATA { get; set; }
        public virtual DbSet<INDEXDESC> INDEXDESC { get; set; }
        public virtual DbSet<TBM_FILE> TBM_FILE { get; set; }
        public virtual DbSet<TBM_PUBLIC_BID> TBM_PUBLIC_BID { get; set; }
        public virtual DbSet<TBM_PUBLIC_BID_99> TBM_PUBLIC_BID_99 { get; set; }
        public virtual DbSet<VENAGENT> VENAGENT { get; set; }
        public virtual DbSet<COMMONLINK> COMMONLINK { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FCFDFE.Entity.GMModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GMEntities : DbContext
    {
        public GMEntities()
            : base("name=GMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACCOUNT> ACCOUNTs { get; set; }
        public virtual DbSet<ACCOUNT_AUTH> ACCOUNT_AUTH { get; set; }
        public virtual DbSet<ACCOUNT_SHIFT> ACCOUNT_SHIFT { get; set; }
        public virtual DbSet<BILLBOARD> BILLBOARDs { get; set; }
        public virtual DbSet<BILLBOARD_GROUP> BILLBOARD_GROUP { get; set; }
        public virtual DbSet<CODE_TABLE> CODE_TABLE { get; set; }
        public virtual DbSet<GMGROUP> GMGROUPs { get; set; }
        public virtual DbSet<PROBLEM_ADD> PROBLEM_ADD { get; set; }
        public virtual DbSet<PROBLEM_KIND> PROBLEM_KIND { get; set; }
        public virtual DbSet<PROBLEM_SYSTEM> PROBLEM_SYSTEM { get; set; }
        public virtual DbSet<TBM1220_2> TBM1220_2 { get; set; }
        public virtual DbSet<TBM1301> TBM1301 { get; set; }
        public virtual DbSet<TBM1301_PLAN> TBM1301_PLAN { get; set; }
        public virtual DbSet<TBM1407> TBM1407 { get; set; }
        public virtual DbSet<TBMDEPT> TBMDEPTs { get; set; }
        public virtual DbSet<USER_LOGIN> USER_LOGIN { get; set; }
        public virtual DbSet<YESNO> YESNOes { get; set; }
        public virtual DbSet<MENU_PAGES> MENU_PAGES { get; set; }
        public virtual DbSet<AUTH_MENU> AUTH_MENU { get; set; }
        public virtual DbSet<PROBLEM_DATA> PROBLEM_DATA { get; set; }
        public virtual DbSet<TBM2133> TBM2133 { get; set; }
        public virtual DbSet<ALLSYS_LOG> ALLSYS_LOG { get; set; }
        public virtual DbSet<TBMSTANDARDITEM> TBMSTANDARDITEMs { get; set; }
        public virtual DbSet<BLACKLIST> BLACKLISTs { get; set; }
        public virtual DbSet<USER_LOGIN_ERRTRY> USER_LOGIN_ERRTRY { get; set; }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace prjC349WebMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class c349dbEntities_Dev : DbContext
    {
        public c349dbEntities_Dev()
            : base("name=c349dbEntities_Dev")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<advanceoy15_clearmixarea> advanceoy15_clearmixarea { get; set; }
        public virtual DbSet<phone_book> phone_book { get; set; }
        public virtual DbSet<tMakingCargoPlan> tMakingCargoPlan { get; set; }
        public virtual DbSet<tAdvanceOY15_ClearMixArea> tAdvanceOY15_ClearMixArea { get; set; }
    }
}

namespace Models.Main
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MainDBC : DbContext
    {
        public MainDBC()
            : base("name=MainDBC")
        {
        }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsL> NewsLs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsL>()
                .Property(e => e.lang)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}

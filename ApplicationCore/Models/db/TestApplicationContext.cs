using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApplicationCore.Models.db
{
    public partial class TestApplicationContext : DbContext
    {
        public TestApplicationContext()
        {
        }

        public TestApplicationContext(DbContextOptions<TestApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=TCJSRVDB02;Database=TestApplication;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Isbn);

                entity.Property(e => e.Isbn).HasColumnName("ISBN");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.TiTle).HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CusId);

                entity.Property(e => e.CusAddress).HasMaxLength(200);

                entity.Property(e => e.CusEmail).HasMaxLength(50);

                entity.Property(e => e.CusName).HasMaxLength(50);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Isbn);

                entity.Property(e => e.Isbn)
                    .ValueGeneratedNever()
                    .HasColumnName("ISBN");

                entity.Property(e => e.TotalPrice).HasColumnName("Total_Price");

                entity.HasOne(d => d.Cus)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CusId)
                    .HasConstraintName("FK_Transactions_Customers");

                entity.HasOne(d => d.IsbnNavigation)
                    .WithOne(p => p.Transaction)
                    .HasForeignKey<Transaction>(d => d.Isbn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Books");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

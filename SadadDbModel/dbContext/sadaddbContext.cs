using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SadadDbModel.dbContext
{
    public partial class sadaddbContext : DbContext
    {
        public sadaddbContext()
        {
        }

        public sadaddbContext(DbContextOptions<sadaddbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Custmer> Custmers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=alisaqer@1994;database=sadaddb");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Custmer>(entity =>
            {
                entity.ToTable("custmer");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.IsAracived)
                    .HasColumnType("tinyint")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("FirstName");

                entity.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("LastName");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.CreatedDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalDept)
                .HasMaxLength(255)
                .HasColumnType("DECIMAL(11,0)")
                .HasColumnName("TotalDept");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(11,0)")
                    .HasColumnName("price");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.HasIndex(e => e.UserId, "CustmerID_idx");

                entity.HasIndex(e => e.ProductId, "ProductID_idx");

                entity.Property(e => e.Coantity).HasColumnName("coantity");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ProductID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CustmerID");

                entity.Property(e => e.CreatedDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

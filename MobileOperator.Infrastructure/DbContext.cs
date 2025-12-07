using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MobileOperator.Domain.Entities;
using CallType = MobileOperator.Domain.Entities.CallType;

namespace MobileOperator.Infrastructure
{
    public partial class MobileOperator : DbContext
    {
        public MobileOperator(DbContextOptions<MobileOperator> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Call> Call { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<FL> FL { get; set; }
        public virtual DbSet<Rate> Rate { get; set; }
        public virtual DbSet<RateHistory> RateHistory { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServiceHistory> ServiceHistory { get; set; }
        public virtual DbSet<CallType> CallType { get; set; }
        public virtual DbSet<UL> UL { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WriteOff> WriteOff { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Admin>().ToTable("admins");
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<FL>().ToTable("fl");
            modelBuilder.Entity<UL>().ToTable("ul");
            modelBuilder.Entity<Call>().ToTable("calls");
            modelBuilder.Entity<CallType>().ToTable("types");
            modelBuilder.Entity<Service>().ToTable("services");
            modelBuilder.Entity<Rate>().ToTable("rates");
            modelBuilder.Entity<RateHistory>().ToTable("rate_history");
            modelBuilder.Entity<ServiceHistory>().ToTable("service_history");
            // Маппинг новой таблицы
            modelBuilder.Entity<WriteOff>().ToTable("write_offs");

            modelBuilder.Entity<User>(e =>
            {
                e.Property(u => u.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Admin>(e =>
            {
                e.HasKey(a => a.UserId);
                e.Property(a => a.UserId).HasColumnName("user_id");
                e.Property(a => a.Login)
                    .HasColumnName("login")
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                e.HasOne(a => a.User)
                 .WithOne(u => u.Admin)
                 .HasForeignKey<Admin>(a => a.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Rate>(e =>
            {
                e.Property(r => r.Name).HasColumnName("name").HasMaxLength(50).IsUnicode(false);
                e.Property(r => r.CityCost).HasColumnName("city_cost").HasColumnType("decimal(18,2)");
                e.Property(r => r.IntercityCost).HasColumnName("intercity_cost").HasColumnType("decimal(18,2)");
                e.Property(r => r.InternationalCost).HasColumnName("international_cost").HasColumnType("decimal(18,2)");
                e.Property(r => r.GB).HasColumnName("gb");
                e.Property(r => r.SMS).HasColumnName("sms");
                e.Property(r => r.Minutes).HasColumnName("minutes");
                e.Property(r => r.ConnectionCost).HasColumnName("connection_cost").HasColumnType("decimal(18,2)");
                e.Property(r => r.GBCost).HasColumnName("gb_cost").HasColumnType("decimal(18,2)");
                e.Property(r => r.SMSCost).HasColumnName("sms_cost").HasColumnType("decimal(18,2)");
                e.Property(r => r.Cost).HasColumnName("cost").HasColumnType("decimal(18,2)");
                e.Property(r => r.Corporate).HasColumnName("corporate");
            });

            modelBuilder.Entity<Client>(e =>
            {
                e.HasKey(c => c.UserId);
                e.Property(c => c.UserId).HasColumnName("user_id");
                e.Property(c => c.Number).HasColumnName("number").IsRequired().HasMaxLength(11).IsUnicode(false);
                e.Property(c => c.Balance).HasColumnName("balance").HasColumnType("decimal(18,2)");
                e.Property(c => c.RateId).HasColumnName("rate");
                e.Property(c => c.Minutes).HasColumnName("minutes").IsRequired();
                e.Property(c => c.GB).HasColumnName("gb");
                e.Property(c => c.SMS).HasColumnName("sms");

                e.HasOne(c => c.User)
                 .WithOne(u => u.Client)
                 .HasForeignKey<Client>(c => c.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(c => c.Rate)
                 .WithMany(r => r.Client)
                 .HasForeignKey(c => c.RateId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<FL>(e =>
            {
                e.HasKey(x => x.UserId);
                e.Property(x => x.UserId).HasColumnName("user_id");
                e.Property(x => x.FIO).HasColumnName("fio").IsRequired().HasMaxLength(50).IsUnicode(false);
                e.Property(x => x.PassportDetails).HasColumnName("passport_details").IsRequired().HasMaxLength(10).IsUnicode(false);

                e.HasOne(x => x.Client)
                 .WithOne(c => c.FL)
                 .HasForeignKey<FL>(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UL>(e =>
            {
                e.HasKey(x => x.UserId);
                e.Property(x => x.UserId).HasColumnName("user_id");
                e.Property(x => x.OrganizationName).HasColumnName("organization_name").IsRequired().HasMaxLength(50).IsUnicode(false);
                e.Property(x => x.Address).HasColumnName("address").IsRequired().HasMaxLength(50).IsUnicode(false);

                e.HasOne(x => x.Client)
                 .WithOne(c => c.UL)
                 .HasForeignKey<UL>(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CallType>(e =>
            {
                e.Property(t => t.Name).HasColumnName("name").IsRequired().HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Call>(e =>
            {
                e.Property(c => c.Id).HasColumnName("id");
                e.Property(c => c.CallerId).HasColumnName("caller_id");
                e.Property(c => c.CallerNumber).HasColumnName("caller_number").HasMaxLength(15).IsUnicode(false);
                e.Property(c => c.CalledId).HasColumnName("called_id");
                e.Property(c => c.CalledNumber).HasColumnName("called_number").HasMaxLength(15).IsUnicode(false);
                e.Property(c => c.CallTime).HasColumnName("call_time");
                e.Property(c => c.Duration).HasColumnName("duration");
                e.Property(c => c.TypeId).HasColumnName("type_id");
                e.Property(c => c.Cost).HasColumnName("cost").HasColumnType("decimal(18,2)");

                e.HasOne(c => c.ClientCaller)
                 .WithMany(cl => cl.Call1)
                 .HasForeignKey(c => c.CallerId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(c => c.ClientCalled)
                 .WithMany(cl => cl.Call)
                 .HasForeignKey(c => c.CalledId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasOne(c => c.CallType)
                 .WithMany(ct => ct.Call)
                 .HasForeignKey(c => c.TypeId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Service>(e =>
            {
                e.Property(s => s.Name).HasColumnName("name").IsRequired().HasMaxLength(50).IsUnicode(false);
                e.Property(s => s.Cost).HasColumnName("cost").HasColumnType("decimal(18,2)");
                e.Property(s => s.ConnectionCost).HasColumnName("connection_cost").HasColumnType("decimal(18,2)");
                e.Property(s => s.Conditions).HasColumnName("conditions");
            });

            modelBuilder.Entity<RateHistory>(e =>
            {
                e.Property(rh => rh.Id).HasColumnName("id");
                e.Property(rh => rh.ClientId).HasColumnName("client_id");
                e.Property(rh => rh.RateId).HasColumnName("rate_id");
                e.Property(rh => rh.FromDate).HasColumnName("from_date");
                e.Property(rh => rh.TillDate).HasColumnName("till_date");

                e.HasOne(rh => rh.Client)
                 .WithMany(c => c.RateHistory)
                 .HasForeignKey(rh => rh.ClientId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(rh => rh.Rate)
                 .WithMany(r => r.RateHistory)
                 .HasForeignKey(rh => rh.RateId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ServiceHistory>(e =>
            {
                e.Property(sh => sh.Id).HasColumnName("id");
                e.Property(sh => sh.ClientId).HasColumnName("client_id");
                e.Property(sh => sh.ServiceId).HasColumnName("service_id");
                e.Property(sh => sh.FromDate).HasColumnName("from_date");
                e.Property(sh => sh.TillDate).HasColumnName("till_date");

                e.HasOne(sh => sh.Client)
                 .WithMany(c => c.ServiceHistory)
                 .HasForeignKey(sh => sh.ClientId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(sh => sh.Service)
                 .WithMany(s => s.ServiceHistory)
                 .HasForeignKey(sh => sh.ServiceId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<WriteOff>(e =>
            {
                e.Property(w => w.Id).HasColumnName("id");
                e.Property(w => w.ClientId).HasColumnName("client_id");
                e.Property(w => w.Amount).HasColumnName("amount").HasColumnType("decimal(18,2)");
                e.Property(w => w.WriteOffDate).HasColumnName("write_off_date");
                e.Property(w => w.Category).HasColumnName("category").IsRequired().HasMaxLength(50).IsUnicode(false);
                e.Property(w => w.Description).HasColumnName("description");

                e.HasOne(w => w.Client)
                 .WithMany()
                 .HasForeignKey(w => w.ClientId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
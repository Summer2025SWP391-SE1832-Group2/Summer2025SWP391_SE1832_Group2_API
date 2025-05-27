using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BDNAT_Repository.Entities;

public partial class DnaTestingDbContext : DbContext
{
    public DnaTestingDbContext()
    {
    }

    public DnaTestingDbContext(DbContextOptions<DnaTestingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogsType> BlogsTypes { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<KitOrder> KitOrders { get; set; }

    public virtual DbSet<Parameter> Parameters { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Sample> Samples { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<ShippingOrder> ShippingOrders { get; set; }

    public virtual DbSet<TestKit> TestKits { get; set; }

    public virtual DbSet<TestParameter> TestParameters { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database= DNA_Testing_db;Uid=sa;Pwd=admin12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blogs__54379E5035B76863");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.BlogTypeId).HasColumnName("BlogTypeID");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.BlogType).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.BlogTypeId)
                .HasConstraintName("FK__Blogs__BlogTypeI__3B75D760");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK__Blogs__CreateBy__3A81B327");
        });

        modelBuilder.Entity<BlogsType>(entity =>
        {
            entity.HasKey(e => e.BlogTypeId).HasName("PK__BlogsTyp__D9FADCC68E851983");

            entity.ToTable("BlogsType");

            entity.Property(e => e.BlogTypeId).HasColumnName("BlogTypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Booking__73951ACD3449CA61");

            entity.ToTable("Booking");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasColumnType("date");
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.PreferredDate).HasColumnType("date");
            entity.Property(e => e.SampleMethod).HasMaxLength(100);
            entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ServiceTypeId)
                .HasConstraintName("FK__Booking__Service__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__UserID__5629CD9C");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.UniqueId).HasName("PK__Comments__A2A2BAAA18CF5866");

            entity.Property(e => e.UniqueId).HasColumnName("UniqueID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.Comment1).HasColumnName("Comment");
            entity.Property(e => e.RootId).HasColumnName("RootID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__BlogID__4316F928");

            entity.HasOne(d => d.Root).WithMany(p => p.InverseRoot)
                .HasForeignKey(d => d.RootId)
                .HasConstraintName("FK__Comments__RootID__440B1D61");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__UserID__4222D4EF");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favorite__CE74FAF514726D0C");

            entity.ToTable("Favorite");

            entity.Property(e => e.FavoriteId).HasColumnName("FavoriteID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Blog).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK__Favorite__BlogID__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Favorite__UserID__3E52440B");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF6713FB312");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Service).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Feedback__Servic__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Feedback__UserID__4BAC3F29");
        });

        modelBuilder.Entity<KitOrder>(entity =>
        {
            entity.HasKey(e => e.KitOrderId).HasName("PK__KitOrder__0D6B4CC37D4D8557");

            entity.ToTable("KitOrder");

            entity.Property(e => e.KitOrderId).HasColumnName("KitOrderID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.ShippingId).HasColumnName("ShippingID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TestKitId).HasColumnName("TestKitID");

            entity.HasOne(d => d.Booking).WithMany(p => p.KitOrders)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__KitOrder__Bookin__6A30C649");

            entity.HasOne(d => d.Shipping).WithMany(p => p.KitOrders)
                .HasForeignKey(d => d.ShippingId)
                .HasConstraintName("FK__KitOrder__Shippi__6B24EA82");

            entity.HasOne(d => d.TestKit).WithMany(p => p.KitOrders)
                .HasForeignKey(d => d.TestKitId)
                .HasConstraintName("FK__KitOrder__TestKi__693CA210");
        });

        modelBuilder.Entity<Parameter>(entity =>
        {
            entity.HasKey(e => e.ParameterId).HasName("PK__Paramete__F80C6297686DAC29");

            entity.ToTable("Parameter");

            entity.Property(e => e.ParameterId).HasColumnName("ParameterID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__FCCDF85CBFEF1BC5");

            entity.ToTable("Rating");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");

            entity.HasOne(d => d.Booking).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Rating__BookingI__59063A47");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Users");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__Result__97690228812C6926");

            entity.ToTable("Result");

            entity.Property(e => e.ResultId).HasColumnName("ResultID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.TestParameterId).HasColumnName("TestParameterID");
            entity.Property(e => e.Value).HasMaxLength(100);

            entity.HasOne(d => d.Booking).WithMany(p => p.Results)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK_Result_Booking");

            entity.HasOne(d => d.TestParameter).WithMany(p => p.Results)
                .HasForeignKey(d => d.TestParameterId)
                .HasConstraintName("FK_Result_TestParameter");
        });

        modelBuilder.Entity<Sample>(entity =>
        {
            entity.HasKey(e => e.SampleId).HasName("PK__Sample__8B99EC0AC524F52E");

            entity.ToTable("Sample");

            entity.Property(e => e.SampleId).HasColumnName("SampleID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CollectedBy).HasMaxLength(100);
            entity.Property(e => e.CollectedDate).HasColumnType("date");
            entity.Property(e => e.ParticipantName).HasMaxLength(100);
            entity.Property(e => e.SampleType).HasMaxLength(100);
            entity.Property(e => e.Transport).HasMaxLength(100);

            entity.HasOne(d => d.Booking).WithMany(p => p.Samples)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Sample__BookingI__628FA481");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB0EA24D4B412");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("PK__ServiceT__8ADFAA0CF0AD14F5");

            entity.ToTable("ServiceType");

            entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceTypes)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceTy__Servi__48CFD27E");
        });

        modelBuilder.Entity<ShippingOrder>(entity =>
        {
            entity.HasKey(e => e.ShippingId).HasName("PK__Shipping__5FACD460BD8C606F");

            entity.Property(e => e.ShippingId).HasColumnName("ShippingID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Receiver).HasMaxLength(100);
            entity.Property(e => e.ShipperName).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TestKit>(entity =>
        {
            entity.HasKey(e => e.TestKitId).HasName("PK__TestKits__E8145AEF4BD91147");

            entity.Property(e => e.TestKitId).HasColumnName("TestKitID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<TestParameter>(entity =>
        {
            entity.HasKey(e => e.TestParameterId).HasName("PK__TestPara__4BCB0FB8CC120776");

            entity.ToTable("TestParameter");

            entity.Property(e => e.TestParameterId).HasColumnName("TestParameterID");
            entity.Property(e => e.ParameterId).HasColumnName("ParameterID");
            entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

            entity.HasOne(d => d.Parameter).WithMany(p => p.TestParameters)
                .HasForeignKey(d => d.ParameterId)
                .HasConstraintName("FK_TestParameter_Parameter");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.TestParameters)
                .HasForeignKey(d => d.ServiceTypeId)
                .HasConstraintName("FK_TestParameter_ServiceType");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B3DE84185");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Booking).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Transacti__Booki__5FB337D6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC1196B26D");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

// <auto-generated />
using System;
using CarbonaraWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarbonaraWebAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220717123543_Removed Required Startstation 2")]
    partial class RemovedRequiredStartstation2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ZIP")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.AuthData", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.HasKey("Id");

                    b.ToTable("AuthData");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("BillDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RelatedBookingId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RelatedBookingId");

                    b.HasIndex("UserId");

                    b.ToTable("Bill");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Bookingtime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Cancelled")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<int>("CarclassId")
                        .HasColumnType("int");

                    b.Property<float>("EndKilometers")
                        .HasColumnType("float");

                    b.Property<int>("EndstationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Endtime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Returned")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ReturnedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("StartKilometers")
                        .HasColumnType("float");

                    b.Property<int>("StartstationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Starttime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("CarclassId");

                    b.HasIndex("EndstationId");

                    b.HasIndex("StartstationId");

                    b.HasIndex("UserId");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("KilometersDriven")
                        .HasColumnType("int");

                    b.Property<int>("LockStatus")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<float>("TankLevel")
                        .HasColumnType("float");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Carclass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("PriceFaktor")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Carclass");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Cartype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CarclassId")
                        .HasColumnType("int");

                    b.Property<string>("Fueltype")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CarclassId");

                    b.ToTable("Cartype");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Cleaning", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Finished")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Cleaning");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Employee", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<double>("Lat")
                        .HasColumnType("double");

                    b.Property<double>("Lon")
                        .HasColumnType("double");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Maintanance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Finished")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Maintanance");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FormOfAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Person");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Picture", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<byte[]>("Daten")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("PriceHourDay")
                        .HasColumnType("float");

                    b.Property<float>("PriceHourNight")
                        .HasColumnType("float");

                    b.Property<float>("PriceHourOverdue")
                        .HasColumnType("float");

                    b.Property<float>("PriceKm")
                        .HasColumnType("float");

                    b.Property<float>("PriceWholeDay")
                        .HasColumnType("float");

                    b.Property<float>("RegistrationFee")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Plan");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.User", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<string>("DriverlicenseNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PaymentMethod_")
                        .HasColumnType("int");

                    b.Property<int>("PlanId")
                        .HasColumnType("int");

                    b.Property<int>("UserState_")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("PlanId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.AuthData", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Person", "Person")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Bill", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Booking", "RelatedBooking")
                        .WithMany()
                        .HasForeignKey("RelatedBookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarbonaraWebAPI.Model.DAO.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RelatedBooking");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Booking", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarbonaraWebAPI.Model.DAO.Carclass", "Carclass")
                        .WithMany()
                        .HasForeignKey("CarclassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarbonaraWebAPI.Model.DAO.Station", "Endstation")
                        .WithMany()
                        .HasForeignKey("EndstationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarbonaraWebAPI.Model.DAO.Station", "Startstation")
                        .WithMany()
                        .HasForeignKey("StartstationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarbonaraWebAPI.Model.DAO.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Carclass");

                    b.Navigation("Endstation");

                    b.Navigation("Startstation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Car", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Cartype", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Cartype", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Carclass", "Carclass")
                        .WithMany()
                        .HasForeignKey("CarclassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Carclass");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Cleaning", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Employee", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Person", "Person")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Location", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Maintanance", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Picture", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.User", "User")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.Station", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("CarbonaraWebAPI.Model.DAO.User", b =>
                {
                    b.HasOne("CarbonaraWebAPI.Model.DAO.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarbonaraWebAPI.Model.DAO.Person", "Person")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarbonaraWebAPI.Model.DAO.Plan", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Person");

                    b.Navigation("Plan");
                });
#pragma warning restore 612, 618
        }
    }
}

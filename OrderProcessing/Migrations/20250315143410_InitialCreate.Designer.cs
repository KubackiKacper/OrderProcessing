﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderProcessing.Data;

#nullable disable

namespace OrderProcessing.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250315143410_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("OrderProcessing.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameOfProduct")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalOfOrder")
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeOfClient")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeOfPayment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderProcessing.Models.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrdersStatuses");
                });

            modelBuilder.Entity("OrderProcessing.Models.OrderStatus", b =>
                {
                    b.HasOne("OrderProcessing.Models.Order", "Order")
                        .WithMany("Statuses")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OrderProcessing.Models.Order", b =>
                {
                    b.Navigation("Statuses");
                });
#pragma warning restore 612, 618
        }
    }
}

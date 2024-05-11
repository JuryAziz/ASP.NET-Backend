﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Store.EntityFramework;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Store.EntityFramework.Entities.Cart", b =>
                {
                    b.Property<Guid>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("Items")
                        .HasColumnType("uuid[]");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("CartId");

                    b.ToTable("Cart", (string)null);
                });

            modelBuilder.Entity("Store.EntityFramework.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("Addresses")
                        .HasColumnType("uuid[]");

                    b.Property<Guid?>("CartId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<Guid>>("Orders")
                        .HasColumnType("uuid[]");

                    b.Property<List<Guid>>("PaymentMethods")
                        .HasColumnType("uuid[]");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<List<Guid>>("ShoppingLists")
                        .HasColumnType("uuid[]");

                    b.HasKey("UserId");

                    b.HasIndex("CartId")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Store.EntityFramework.Entities.User", b =>
                {
                    b.HasOne("Store.EntityFramework.Entities.Cart", "Cart")
                        .WithOne("User")
                        .HasForeignKey("Store.EntityFramework.Entities.User", "CartId");

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("Store.EntityFramework.Entities.Cart", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

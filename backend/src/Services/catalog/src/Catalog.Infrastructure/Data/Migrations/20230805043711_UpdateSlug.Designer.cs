﻿// <auto-generated />
using System;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catalog.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230805043711_UpdateSlug")]
    partial class UpdateSlug
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Catalog.Domain.Entities.Adsenses.Adsense", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Adsense", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Categories.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Slug");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Categories.SubCategory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SubCategory", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Certificate.CertificateType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CertificateType", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Deals.HomeDeal", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImgUrl")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("HomeDeal", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Deals.HotDeal", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DealsFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DealsTo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DiscountPercent")
                        .HasColumnType("integer");

                    b.Property<int>("DiscountPrice")
                        .HasColumnType("integer");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("HotDeals", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Facilities.Facility", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Facility", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CertificateId")
                        .HasColumnType("text");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Details")
                        .HasColumnType("text");

                    b.Property<string>("District")
                        .HasColumnType("text");

                    b.Property<string>("LocationLatitude")
                        .HasColumnType("text");

                    b.Property<string>("LocationLongitude")
                        .HasColumnType("text");

                    b.Property<string>("PostCode")
                        .HasColumnType("text");

                    b.Property<decimal>("PriceFrom")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PriceTo")
                        .HasColumnType("numeric");

                    b.Property<string>("Province")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RegisteredSince")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SubCategoryId")
                        .HasColumnType("text");

                    b.Property<string>("SubDistrict")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CertificateId");

                    b.HasIndex("Slug");

                    b.HasIndex("SubCategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductDiscount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DiscountEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DiscountName")
                        .HasColumnType("text");

                    b.Property<decimal>("DiscountPercent")
                        .HasColumnType("numeric");

                    b.Property<decimal>("DiscountPrice")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("DiscountStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("Slug");

                    b.ToTable("ProductDiscount", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductFacility", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("FacilityId")
                        .HasColumnType("text");

                    b.HasKey("ProductId", "FacilityId");

                    b.HasIndex("FacilityId");

                    b.ToTable("ProductFacility", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductImage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImageName")
                        .HasColumnType("text");

                    b.Property<string>("ImageType")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductNear", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductNear");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductNearItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductNearId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductNearId");

                    b.ToTable("ProductNearItem");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductSpecification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSpecification", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Adsenses.Adsense", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("Adsenses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Deals.HomeDeal", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("HomeDeals")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Deals.HotDeal", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("HotDeals")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.Product", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Categories.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.Domain.Entities.Certificate.CertificateType", "CertificateType")
                        .WithMany("Products")
                        .HasForeignKey("CertificateId");

                    b.HasOne("Catalog.Domain.Entities.Categories.SubCategory", "SubCategory")
                        .WithMany("Products")
                        .HasForeignKey("SubCategoryId");

                    b.Navigation("Category");

                    b.Navigation("CertificateType");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductDiscount", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("ProductDiscounts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductFacility", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Facilities.Facility", "Facility")
                        .WithMany("ProductFacilities")
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("ProductFacilities")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductImage", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductNear", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("ProductNears")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductNearItem", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.ProductNear", "ProductNear")
                        .WithMany("ProductNearItems")
                        .HasForeignKey("ProductNearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductNear");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductSpecification", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Products.Product", "Product")
                        .WithMany("ProductSpecifications")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Categories.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Categories.SubCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Certificate.CertificateType", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Facilities.Facility", b =>
                {
                    b.Navigation("ProductFacilities");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.Product", b =>
                {
                    b.Navigation("Adsenses");

                    b.Navigation("HomeDeals");

                    b.Navigation("HotDeals");

                    b.Navigation("ProductDiscounts");

                    b.Navigation("ProductFacilities");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductNears");

                    b.Navigation("ProductSpecifications");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Products.ProductNear", b =>
                {
                    b.Navigation("ProductNearItems");
                });
#pragma warning restore 612, 618
        }
    }
}
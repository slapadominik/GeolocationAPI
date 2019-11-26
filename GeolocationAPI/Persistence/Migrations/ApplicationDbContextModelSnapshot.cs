﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GeolocationAPI.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("GeolocationAPI.Persistence.Entities.GeolocationData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("ContinentCode")
                        .HasMaxLength(2);

                    b.Property<string>("ContinentName");

                    b.Property<string>("CountryCode")
                        .HasMaxLength(2);

                    b.Property<string>("CountryName");

                    b.Property<string>("IpAddress");

                    b.Property<string>("IpAddressType");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("RegionCode")
                        .HasMaxLength(10);

                    b.Property<string>("RegionName");

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("GeolocationData");
                });
#pragma warning restore 612, 618
        }
    }
}

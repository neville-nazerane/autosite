﻿// <auto-generated />
using AutoSite.Migrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutoSite.Migrations.Migrations
{
    [DbContext(typeof(MigrationDbContext))]
    [Migration("20181017071920_NoLimits")]
    partial class NoLimits
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AutoSite.Core.Entities.ClassItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<int>("SiteContentId");

                    b.HasKey("Id");

                    b.HasIndex("SiteContentId");

                    b.HasIndex("Name", "SiteContentId")
                        .IsUnique();

                    b.ToTable("ClassItems");
                });

            modelBuilder.Entity("AutoSite.Core.Entities.PropertyItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassItemId");

                    b.Property<int>("DataType");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("ClassItemId");

                    b.HasIndex("Name", "ClassItemId")
                        .IsUnique();

                    b.ToTable("PropertyItems");
                });

            modelBuilder.Entity("AutoSite.Core.Entities.SiteContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("SiteContent");
                });

            modelBuilder.Entity("AutoSite.Core.Entities.ClassItem", b =>
                {
                    b.HasOne("AutoSite.Core.Entities.SiteContent")
                        .WithMany("ClassItems")
                        .HasForeignKey("SiteContentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AutoSite.Core.Entities.PropertyItem", b =>
                {
                    b.HasOne("AutoSite.Core.Entities.ClassItem", "ClassItem")
                        .WithMany("Properties")
                        .HasForeignKey("ClassItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

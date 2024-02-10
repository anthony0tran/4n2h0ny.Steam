﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _4n2h0ny.Steam.API.Entities;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    [DbContext(typeof(ProfileContext))]
    [Migration("20240128155914_Add_Profile_CommentedOn")]
    partial class Add_Profile_CommentedOn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("_4n2h0ny.Steam.API.Entities.Profile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CommentedOn")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsExcluded")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFriend")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LatestCommentReceivedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("URI")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("URI")
                        .IsUnique();

                    b.ToTable("Profiles");
                });
#pragma warning restore 612, 618
        }
    }
}

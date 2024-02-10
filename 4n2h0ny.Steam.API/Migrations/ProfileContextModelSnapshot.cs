﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _4n2h0ny.Steam.API.Entities;

#nullable disable

namespace _4n2h0ny.Steam.API.Migrations
{
    [DbContext(typeof(ProfileContext))]
    partial class ProfileContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime?>("FetchedOn")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsExcluded")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFriend")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LatestCommentReceivedOn")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProfileDataId")
                        .HasColumnType("TEXT");

                    b.Property<string>("URI")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProfileDataId")
                        .IsUnique();

                    b.HasIndex("URI")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("_4n2h0ny.Steam.API.Entities.ProfileData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("AwardCount")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BadgeCount")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CommentAreaDisabled")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CommonFriendCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FriendCount")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastFetchedOn")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PersonaName")
                        .HasColumnType("TEXT");

                    b.Property<string>("RealName")
                        .HasColumnType("TEXT");

                    b.Property<long?>("SteamId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TotalCommendsCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ProfileData");
                });

            modelBuilder.Entity("_4n2h0ny.Steam.API.Entities.Profile", b =>
                {
                    b.HasOne("_4n2h0ny.Steam.API.Entities.ProfileData", "ProfileData")
                        .WithOne()
                        .HasForeignKey("_4n2h0ny.Steam.API.Entities.Profile", "ProfileDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProfileData");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />

using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220510082418_Event_Add_Award")]
    partial class Event_Add_Award
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventsTeams", b =>
                {
                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.HasKey("EventId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("EventsTeams", (string)null);
                });

            modelBuilder.Entity("Hackathon.Entities.AuditEventEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Type");

                    b.HasIndex("UserId");

                    b.ToTable("Audit");
                });

            modelBuilder.Entity("Hackathon.Entities.EventEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Award")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<ICollection<ChangeEventStatusMessage>>("ChangeEventStatusMessages")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("DevelopmentMinutes")
                        .HasColumnType("integer");

                    b.Property<bool>("IsCreateTeamsAutomatically")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxEventMembers")
                        .HasColumnType("integer");

                    b.Property<int>("MemberRegistrationMinutes")
                        .HasColumnType("integer");

                    b.Property<int>("MinTeamMembers")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TeamPresentationMinutes")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Events", (string)null);
                });

            modelBuilder.Entity("Hackathon.Entities.FileStorageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("BucketName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("Length")
                        .HasColumnType("bigint");

                    b.Property<string>("MimeType")
                        .HasColumnType("text");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("StorageFiles");
                });

            modelBuilder.Entity("Hackathon.Entities.GoogleAccountEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AccessToken")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<long>("ExpiresAt")
                        .HasColumnType("bigint");

                    b.Property<long>("ExpiresIn")
                        .HasColumnType("bigint");

                    b.Property<long>("FirstIssuedAt")
                        .HasColumnType("bigint");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("GivenName")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("LoginHint")
                        .HasColumnType("text");

                    b.Property<string>("TokenId")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("GoogleAccounts", (string)null);
                });

            modelBuilder.Entity("Hackathon.Entities.NotificationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .HasColumnType("jsonb");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Notifications", (string)null);
                });

            modelBuilder.Entity("Hackathon.Entities.ProjectEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("TeamId");

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("Hackathon.Entities.TeamEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.ToTable("Teams", (string)null);
                });

            modelBuilder.Entity("Hackathon.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("GoogleAccountId")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ProfileImageId")
                        .HasColumnType("uuid");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GoogleAccountId")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("MembersTeams", b =>
                {
                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.HasKey("MemberId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("MembersTeams", (string)null);
                });

            modelBuilder.Entity("EventsTeams", b =>
                {
                    b.HasOne("Hackathon.Entities.EventEntity", null)
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.Entities.TeamEntity", null)
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hackathon.Entities.EventEntity", b =>
                {
                    b.HasOne("Hackathon.Entities.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Hackathon.Entities.ProjectEntity", b =>
                {
                    b.HasOne("Hackathon.Entities.EventEntity", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.Entities.TeamEntity", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Hackathon.Entities.TeamEntity", b =>
                {
                    b.HasOne("Hackathon.Entities.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Hackathon.Entities.UserEntity", b =>
                {
                    b.HasOne("Hackathon.Entities.GoogleAccountEntity", "GoogleAccount")
                        .WithOne("User")
                        .HasForeignKey("Hackathon.Entities.UserEntity", "GoogleAccountId");

                    b.Navigation("GoogleAccount");
                });

            modelBuilder.Entity("MembersTeams", b =>
                {
                    b.HasOne("Hackathon.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.Entities.TeamEntity", null)
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hackathon.Entities.GoogleAccountEntity", b =>
                {
                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}

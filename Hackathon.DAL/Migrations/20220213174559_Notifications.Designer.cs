﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Hackathon.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hackathon.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220213174559_Notifications")]
    partial class Notifications
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hackathon.DAL.Entities.EventEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<ICollection<ChangeEventStatusMessage>>("ChangeEventStatusMessages")
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

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TeamPresentationMinutes")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Events", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.NotificationEntity", b =>
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

            modelBuilder.Entity("Hackathon.DAL.Entities.ProjectEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("TeamEventId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TeamEventId")
                        .IsUnique();

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEntity", b =>
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

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEventEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ProjectId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamEvents", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("UserTeam", b =>
                {
                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("TeamId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTeam", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.EventEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.ProjectEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.TeamEventEntity", "TeamEvent")
                        .WithOne("Project")
                        .HasForeignKey("Hackathon.DAL.Entities.ProjectEntity", "TeamEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TeamEvent");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEventEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.EventEntity", "Event")
                        .WithMany("TeamEvents")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.TeamEntity", "Team")
                        .WithMany("TeamEvents")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("UserTeam", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.TeamEntity", null)
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.EventEntity", b =>
                {
                    b.Navigation("TeamEvents");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEntity", b =>
                {
                    b.Navigation("TeamEvents");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEventEntity", b =>
                {
                    b.Navigation("Project");
                });
#pragma warning restore 612, 618
        }
    }
}

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
    [Migration("20240121111055_AddBlocks")]
    partial class AddBlocks
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

            modelBuilder.Entity("Hackathon.DAL.Entities.ApprovalApplications.ApprovalApplicationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<byte>("ApplicationStatus")
                        .HasColumnType("smallint");

                    b.Property<long>("AuthorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<DateTimeOffset?>("DecisionAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("RequestedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("SignerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("SignerId");

                    b.ToTable("ApprovalApplications", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Block.BlockingEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("ActionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("AssignmentUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("TargetUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TargetUserId")
                        .IsUnique();

                    b.ToTable("Blocks", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.EmailConfirmationRequestEntity", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConfirmationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("boolean");

                    b.HasKey("UserId");

                    b.ToTable("EmailConfirmations", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventAgreementEntity", b =>
                {
                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<bool>("RequiresConfirmation")
                        .HasColumnType("boolean");

                    b.Property<string>("Rules")
                        .HasColumnType("text");

                    b.HasKey("EventId");

                    b.ToTable("EventAgreements", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventAgreementUserEntity", b =>
                {
                    b.Property<long>("AgreementId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("AgreementId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("EventAgreementUsers", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("ApprovalApplicationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Award")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<ICollection<ChangeEventStatusMessage>>("ChangeEventStatusMessages")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<long>("CurrentStageId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCreateTeamsAutomatically")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("MaxEventMembers")
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

                    b.Property<string>("Tags")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<EventTaskItem[]>("Tasks")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalApplicationId")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.HasIndex("Tags");

                    b.ToTable("Events", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventStageEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventStages", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.FriendshipEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<long>("ProposerId")
                        .HasColumnType("bigint");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProposerId", "UserId")
                        .IsUnique();

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.GoogleAccountEntity", b =>
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

            modelBuilder.Entity("Hackathon.DAL.Entities.MemberTeamEntity", b =>
                {
                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateTimeAdd")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<byte>("Role")
                        .HasColumnType("smallint");

                    b.HasKey("MemberId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("MembersTeams", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.ProjectEntity", b =>
                {
                    b.Property<long>("EventId")
                        .HasColumnType("bigint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid[]>("FileIds")
                        .HasColumnType("uuid[]");

                    b.Property<string>("LinkToGitBranch")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("EventId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<byte>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.ToTable("Teams", (string)null);
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamJoinRequestEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ModifyAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<long>("TeamId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("TeamId", "UserId");

                    b.ToTable("TeamJoinRequests");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.User.UserEntity", b =>
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

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

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

            modelBuilder.Entity("Hackathon.DAL.Entities.User.UserReactionEntity", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("TargetUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Reaction")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "TargetUserId");

                    b.ToTable("UserReactions", (string)null);
                });

            modelBuilder.Entity("EventsTeams", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.Event.EventEntity", null)
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.TeamEntity", null)
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.ApprovalApplications.ApprovalApplicationEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "Signer")
                        .WithMany()
                        .HasForeignKey("SignerId");

                    b.Navigation("Author");

                    b.Navigation("Signer");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Block.BlockingEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "TargetUser")
                        .WithOne("Block")
                        .HasForeignKey("Hackathon.DAL.Entities.Block.BlockingEntity", "TargetUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TargetUser");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.EmailConfirmationRequestEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "User")
                        .WithOne("EmailConfirmationRequest")
                        .HasForeignKey("Hackathon.DAL.Entities.EmailConfirmationRequestEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventAgreementEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.Event.EventEntity", "Event")
                        .WithOne("Agreement")
                        .HasForeignKey("Hackathon.DAL.Entities.Event.EventAgreementEntity", "EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventAgreementUserEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.Event.EventAgreementEntity", "Agreement")
                        .WithMany()
                        .HasForeignKey("AgreementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agreement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.ApprovalApplications.ApprovalApplicationEntity", "ApprovalApplication")
                        .WithOne("Event")
                        .HasForeignKey("Hackathon.DAL.Entities.Event.EventEntity", "ApprovalApplicationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovalApplication");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventStageEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.Event.EventEntity", "Event")
                        .WithMany("Stages")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.MemberTeamEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "Member")
                        .WithMany("Teams")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.TeamEntity", "Team")
                        .WithMany("Members")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.ProjectEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.Event.EventEntity", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.TeamEntity", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamJoinRequestEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.TeamEntity", "Team")
                        .WithMany("JoinRequests")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackathon.DAL.Entities.User.UserEntity", "User")
                        .WithMany("JoinRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.User.UserEntity", b =>
                {
                    b.HasOne("Hackathon.DAL.Entities.GoogleAccountEntity", "GoogleAccount")
                        .WithOne("User")
                        .HasForeignKey("Hackathon.DAL.Entities.User.UserEntity", "GoogleAccountId");

                    b.Navigation("GoogleAccount");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.ApprovalApplications.ApprovalApplicationEntity", b =>
                {
                    b.Navigation("Event");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.Event.EventEntity", b =>
                {
                    b.Navigation("Agreement");

                    b.Navigation("Stages");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.GoogleAccountEntity", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.TeamEntity", b =>
                {
                    b.Navigation("JoinRequests");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("Hackathon.DAL.Entities.User.UserEntity", b =>
                {
                    b.Navigation("Block");

                    b.Navigation("EmailConfirmationRequest");

                    b.Navigation("JoinRequests");

                    b.Navigation("Teams");
                });
#pragma warning restore 612, 618
        }
    }
}

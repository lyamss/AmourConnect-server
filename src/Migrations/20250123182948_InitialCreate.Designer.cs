﻿// <auto-generated />
using System;
using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(BackendDbContext))]
    [Migration("20250123182948_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("API.Entities.Message", b =>
                {
                    b.Property<int>("Id_Message")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_Message"));

                    b.Property<DateTime>("Date_of_request")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("IdUserIssuer")
                        .HasColumnType("integer");

                    b.Property<int>("Id_UserReceiver")
                        .HasColumnType("integer");

                    b.Property<string>("message_content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id_Message");

                    b.HasIndex("IdUserIssuer");

                    b.HasIndex("Id_UserReceiver");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("API.Entities.RequestFriends", b =>
                {
                    b.Property<int>("Id_RequestFriends")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_RequestFriends"));

                    b.Property<DateTime>("Date_of_request")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("IdUserIssuer")
                        .HasColumnType("integer");

                    b.Property<int>("Id_UserReceiver")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id_RequestFriends");

                    b.HasIndex("IdUserIssuer");

                    b.HasIndex("Id_UserReceiver");

                    b.ToTable("RequestFriends");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Property<int>("Id_User")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id_User"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("EmailGoogle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<byte[]>("Profile_picture")
                        .HasColumnType("bytea");

                    b.Property<string>("Pseudo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime>("account_created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("date_of_birth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("date_token_session_expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("sex")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("token_session_user")
                        .HasColumnType("text");

                    b.Property<string>("userIdGoogle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id_User");

                    b.ToTable("User");
                });

            modelBuilder.Entity("API.Entities.Message", b =>
                {
                    b.HasOne("API.Entities.User", "UserIssuer")
                        .WithMany("MessagesSent")
                        .HasForeignKey("IdUserIssuer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "UserReceiver")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("Id_UserReceiver")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserIssuer");

                    b.Navigation("UserReceiver");
                });

            modelBuilder.Entity("API.Entities.RequestFriends", b =>
                {
                    b.HasOne("API.Entities.User", "UserIssuer")
                        .WithMany("RequestsSent")
                        .HasForeignKey("IdUserIssuer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.User", "UserReceiver")
                        .WithMany("RequestsReceived")
                        .HasForeignKey("Id_UserReceiver")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserIssuer");

                    b.Navigation("UserReceiver");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");

                    b.Navigation("RequestsReceived");

                    b.Navigation("RequestsSent");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231009073316_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Directors.Entities.Director", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BornOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("bornOn");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdOn");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("isDeleted");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modifiedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pK_directors");

                    b.ToTable("directors", (string)null);
                });

            modelBuilder.Entity("Domain.Movies.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Actors")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("actors");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdOn");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("DirectorId")
                        .HasColumnType("integer")
                        .HasColumnName("directorId");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("isDeleted");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("language");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modifiedOn");

                    b.Property<DateTime>("ReleasedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("releasedOn");

                    b.Property<string>("Writers")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("writers");

                    b.HasKey("Id")
                        .HasName("pK_movies");

                    b.HasIndex("DirectorId")
                        .HasDatabaseName("iX_movies_directorId");

                    b.ToTable("movies", (string)null);
                });

            modelBuilder.Entity("Domain.Ratings.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdOn");

                    b.Property<int>("CreatorId")
                        .HasColumnType("integer")
                        .HasColumnName("creatorId");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("isDeleted");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modifiedOn");

                    b.Property<int>("MovieId")
                        .HasColumnType("integer")
                        .HasColumnName("movieId");

                    b.Property<int>("SourPopcorns")
                        .HasColumnType("integer")
                        .HasColumnName("sourPopcorns");

                    b.HasKey("Id")
                        .HasName("pK_ratings");

                    b.HasIndex("CreatorId")
                        .HasDatabaseName("iX_ratings_creatorId");

                    b.HasIndex("MovieId")
                        .HasDatabaseName("iX_ratings_movieId");

                    b.ToTable("ratings", (string)null);
                });

            modelBuilder.Entity("Domain.Users.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdOn");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("displayName");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("isDeleted");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modifiedOn");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passwordHash");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("roles");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pK_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Domain.Votes.Entities.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdOn");

                    b.Property<int>("CreatorId")
                        .HasColumnType("integer")
                        .HasColumnName("creatorId");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("isDeleted");

                    b.Property<bool>("IsPositive")
                        .HasColumnType("boolean")
                        .HasColumnName("isPositive");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modifiedOn");

                    b.Property<int>("RatingId")
                        .HasColumnType("integer")
                        .HasColumnName("ratingId");

                    b.HasKey("Id")
                        .HasName("pK_votes");

                    b.HasIndex("CreatorId")
                        .HasDatabaseName("iX_votes_creatorId");

                    b.HasIndex("RatingId")
                        .HasDatabaseName("iX_votes_ratingId");

                    b.ToTable("votes", (string)null);
                });

            modelBuilder.Entity("Domain.Movies.Entities.Movie", b =>
                {
                    b.HasOne("Domain.Directors.Entities.Director", "Director")
                        .WithMany("Movies")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fK_movies_directors_directorId");

                    b.Navigation("Director");
                });

            modelBuilder.Entity("Domain.Ratings.Entities.Rating", b =>
                {
                    b.HasOne("Domain.Users.Entities.User", "Creator")
                        .WithMany("Ratings")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fK_ratings_users_creatorId");

                    b.HasOne("Domain.Movies.Entities.Movie", "Movie")
                        .WithMany("Ratings")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fK_ratings_movies_movieId");

                    b.Navigation("Creator");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Domain.Votes.Entities.Vote", b =>
                {
                    b.HasOne("Domain.Users.Entities.User", "Creator")
                        .WithMany("Votes")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fK_votes_users_creatorId");

                    b.HasOne("Domain.Ratings.Entities.Rating", "Rating")
                        .WithMany("Votes")
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fK_votes_ratings_ratingId");

                    b.Navigation("Creator");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("Domain.Directors.Entities.Director", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("Domain.Movies.Entities.Movie", b =>
                {
                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("Domain.Ratings.Entities.Rating", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Domain.Users.Entities.User", b =>
                {
                    b.Navigation("Ratings");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}

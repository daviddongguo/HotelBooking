﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using david.hotelbooking.domain.Concretes;

namespace david.hotelbooking.mvc.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class EFDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("david.hotelbooking.domain.Entities.RBAC.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "",
                            Name = "readOrder"
                        },
                        new
                        {
                            Id = 2,
                            Description = "",
                            Name = "writeOrder"
                        },
                        new
                        {
                            Id = 3,
                            Description = "",
                            Name = "readUser"
                        },
                        new
                        {
                            Id = 4,
                            Description = "",
                            Name = "writeUser"
                        });
                });

            modelBuilder.Entity("david.hotelbooking.domain.Entities.RBAC.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "administrator",
                            Name = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Description = "",
                            Name = "marketing"
                        },
                        new
                        {
                            Id = 3,
                            Description = "",
                            Name = "receptionist"
                        },
                        new
                        {
                            Id = 4,
                            Description = "",
                            Name = "customer"
                        });
                });

            modelBuilder.Entity("david.hotelbooking.domain.Entities.RBAC.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermissionId = 1
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 2
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 3
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 4
                        },
                        new
                        {
                            RoleId = 4,
                            PermissionId = 3
                        },
                        new
                        {
                            RoleId = 4,
                            PermissionId = 4
                        });
                });

            modelBuilder.Entity("david.hotelbooking.domain.Entities.RBAC.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "Admin@hotel.com",
                            Password = "aaa"
                        },
                        new
                        {
                            Id = 2,
                            Email = "Sis@hotel.com",
                            Password = "aaa"
                        });
                });

            modelBuilder.Entity("david.hotelbooking.domain.Entities.RBAC.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            RoleId = 1
                        },
                        new
                        {
                            UserId = 2,
                            RoleId = 4
                        });
                });

            modelBuilder.Entity("david.hotelbooking.domain.Entities.RBAC.RolePermission", b =>
                {
                    b.HasOne("david.hotelbooking.domain.Entities.RBAC.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("david.hotelbooking.domain.Entities.RBAC.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("david.hotelbooking.domain.Entities.RBAC.UserRole", b =>
                {
                    b.HasOne("david.hotelbooking.domain.Entities.RBAC.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("david.hotelbooking.domain.Entities.RBAC.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

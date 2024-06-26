﻿
using API_CONDOMINIO_2.Data.Mappings;
using API_CONDOMINIO_2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_CONDOMINIO_2.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Block> Blocks { get; set; }
    public DbSet<Resident> Residents { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Sex> Sex { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                Name = "admin",
                Slug = "admin"
            },
            new Role
            {
                Id = 2,
                Name = "operacional",
                Slug = "operacional"
            },
            new Role
            {
                Id = 3,
                Name = "usuario",
                Slug = "usuario"
            }
            );

        modelBuilder.ApplyConfiguration(new ResidentMap());
        modelBuilder.ApplyConfiguration(new UserMap());
        modelBuilder.ApplyConfiguration(new UnitMap());
        modelBuilder.ApplyConfiguration(new BlockMap());


    }
}


﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelSell.Models.AppDbContext
{
    public class WheelSellDbContext : IdentityDbContext<IdentityUser>
    {
        public WheelSellDbContext(DbContextOptions<WheelSellDbContext> options):
            base(options)
        {

        }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}

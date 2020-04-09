using Microsoft.EntityFrameworkCore;
using OTP.Repository.Entities;
using OTP.Repository.ModelBuilderConfigurations;
using System;

namespace OTP.Repository.Context
{
    public class OTPContext : DbContext
    {
        public OTPContext(DbContextOptions<OTPContext> options)
        : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSecretKey> UserSecretKeys { get; set; }
        public virtual DbSet<GeneratedOTP> GeneratedOTPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserSecretKeyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GeneratedOTPEntityConfiguration());
        }
    }
}

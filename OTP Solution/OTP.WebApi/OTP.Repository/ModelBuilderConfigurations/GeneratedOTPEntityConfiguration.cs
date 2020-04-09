using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTP.Repository.Entities;

namespace OTP.Repository.ModelBuilderConfigurations
{
    public class GeneratedOTPEntityConfiguration : IEntityTypeConfiguration<GeneratedOTP>
    {
        public void Configure(EntityTypeBuilder<GeneratedOTP> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        }
    }
}

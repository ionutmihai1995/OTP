using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTP.Repository.Entities;

namespace OTP.Repository.ModelBuilderConfigurations
{
    public class UserSecretKeyEntityConfiguration : IEntityTypeConfiguration<UserSecretKey>
    {
        public void Configure(EntityTypeBuilder<UserSecretKey> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        }
    }
}

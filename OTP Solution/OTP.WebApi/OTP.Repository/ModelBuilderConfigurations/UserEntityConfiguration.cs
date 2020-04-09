using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OTP.Repository.Entities;

namespace OTP.Repository.ModelBuilderConfigurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        }
    }
}

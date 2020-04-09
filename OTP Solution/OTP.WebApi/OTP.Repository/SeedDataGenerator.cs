using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OTP.Repository.Context;
using OTP.Repository.Entities;
using System;
using System.Linq;

namespace OTP.Repository
{
    public class SeedDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OTPContext(
                serviceProvider.GetRequiredService<DbContextOptions<OTPContext>>()))
            {
                //Adding seed data
                if (context.Users.Any())
                {
                    return;   // Data was already seeded
                }

                context.Users.AddRange(
                    new User { UserID = "122212MA" },
                    new User { UserID = "122123MI" }
                );

                context.SaveChanges();
            }
        }
    }
}

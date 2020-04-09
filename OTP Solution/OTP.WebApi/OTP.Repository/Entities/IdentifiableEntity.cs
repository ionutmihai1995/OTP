using System;

namespace OTP.Repository.Entities
{
    public class IdentifiableEntity
    {
        public IdentifiableEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}

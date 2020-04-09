namespace OTP.Repository.Entities
{
    public class UserSecretKey : IdentifiableEntity
    {
        public string UserId { get; set; }
        public string SecretKey { get; set; }
    }
}

namespace CRUD_Radenta.Model.Entities
{
    public class JwtToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt {  get; set; }
    }
}

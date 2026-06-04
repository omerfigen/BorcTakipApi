namespace BorcTakipApi.Models
{
    /// <summary>
    /// Sisteme giriş yapan kullanıcıları temsil eden model sınıfı.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Kullanıcının sisteme giriş yaparken kullandığı benzersiz ad.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// Kullanıcının hesap güvenliği için şifresi.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}

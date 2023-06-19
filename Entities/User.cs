namespace core7_cassandra_angular14.Entities
{
    [System.ComponentModel.DataAnnotations.Schema.Table("users")]
    public class User {

        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Username { get; set; }
        public string Pasword { get; set; }
        public string Role { get; set; }
        public string Profilepic { get; set; }
        public int Isactivated { get; set; }
        public int Isblocked { get; set; }
        public int Mailtoken { get; set; }
        public string Qrcodeurl { get; set; }
        public string Secretkey { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; }
    }
    
}
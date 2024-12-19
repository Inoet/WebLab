namespace gestion.Models
{
    public class Users
    {
        public int Id { get; set; }
       // public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
       // public int Goods { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Fam { get; set; }
        public string Fat { get; set; }
        public string Phone { get; set; }
        public static int In { get; set; } = 0;  
    }
}

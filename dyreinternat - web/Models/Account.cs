namespace dyreinternat___web.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Tlf { get; set; }

        public Account(int accountID, string name, string email, string tlf)
        {
            AccountID = accountID;
            Name = name;
            Email = email;
            Tlf = tlf;
        }

    }
}

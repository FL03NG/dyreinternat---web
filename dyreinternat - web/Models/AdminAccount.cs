using System.ComponentModel.DataAnnotations;

namespace dyreinternat___web.Models
{
    public class AdminAccount : Account
    {
        public string AdminLogIn {  get; set; }

        public AdminAccount(int accountID, string name, string email, string tlf, string adminLogIn) : base(accountID, name, email, tlf) 
        {
            AdminLogIn = adminLogIn;
        }
    }
}

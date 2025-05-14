namespace dyreinternat___web.Models
{
    public class MemberAccount : Account
    {
        public string _memberLogIn { get; set; }

        public MemberAccount(int AccountID, string Name, string Email, string Tlf, string MemberLogIn) : base(AccountID, Name, Email, Tlf)
        {
            _memberLogIn = MemberLogIn;
        }
    }
}

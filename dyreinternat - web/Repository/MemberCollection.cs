using dyreinternat___web.Models;

namespace dyreinternat___web.Repository
{
    public class MemberCollection : IMember
    {

        private List<MemberAccount> _members;

        public MemberCollection()
        {
            _members = new List<MemberAccount>();
            Seed();
        }

        public void Add(MemberAccount memberAccount)
        {
            _members.Add(memberAccount);
        }

        public void Delete(int id)
        {
            foreach (MemberAccount a in _members)
            {
                if (a.AccountID == id)
                {
                    _members.Remove(a);
                    break;
                }
            }
        }

        public List<MemberAccount> GetAll()
        {
            return _members;
        }

        private void Seed()
        {
            _members.Add(new MemberAccount(1, "", "", "", ""));
            _members.Add(new MemberAccount(2, "", "", "", ""));
            _members.Add(new MemberAccount(3, "", "", "", ""));
            _members.Add(new MemberAccount(4, "", "", "", ""));
            _members.Add(new MemberAccount(5, "", "", "", ""));
            _members.Add(new MemberAccount(6, "", "", "", ""));
            _members.Add(new MemberAccount(7, "", "", "", ""));
            _members.Add(new MemberAccount(8, "", "", "", ""));

        }
    }
}

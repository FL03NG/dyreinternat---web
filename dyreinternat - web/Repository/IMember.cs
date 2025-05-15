using dyreinternat___web.Models;

namespace dyreinternat___web.Repository
{
    public interface IMember
    {
        public List<MemberAccount> GetAll();

        public void Add(MemberAccount memberAccount);

        public void Delete(int id);
    }
}

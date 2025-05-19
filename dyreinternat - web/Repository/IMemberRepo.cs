using dyreinternat___web.Models;

namespace dyreinternat___web.Repository
{
    public interface IMemberRepo
    {
        public List<MemberAccount> GetAll();

        public void Add(MemberAccount memberAccount);

        public void Delete(int id);
    }
}

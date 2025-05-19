using dyreinternat___web.Models;

namespace dyreinternat___web.Repository
{
    public interface IAnimalRepo
    {
        public List<Animal> GetAll();

        public void Add(Animal animal);

        public void Delete(int id);
        
    }
}

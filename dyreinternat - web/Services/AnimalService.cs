using dyreinternat___web.Repository;
using dyreinternat___web.Models;
using System.Security.Cryptography.X509Certificates;
namespace dyreinternat___web.Services
{
    public class AnimalService
    {
        private IAnimalRepo _animalrepo;

        public AnimalService(IAnimalRepo animalRepo)
        {
            _animalrepo = animalRepo;
        }

        public void Add(Animal animal)
        {
            _animalrepo.Add(animal);
        }

        public List<Animal> GetAll()
        {
            return _animalrepo.GetAll();
        }

        public void Delete(int id)
        {
            _animalrepo.Delete(id);
        }
    }
}

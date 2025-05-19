
using System.Diagnostics;
using dyreinternat___web.Models;

namespace dyreinternat___web.Repository
{
    public class AnimalCollectionRepo : IAnimalRepo
    {
        public List<Animal> _animals;

        public AnimalCollectionRepo()
        {
            _animals = new List<Animal>();
            
        }

        public virtual void Add(Animal animal)
        {
            Debug.WriteLine("add2");
            _animals.Add(animal);
        }

        public void Delete(int id)
        {
            foreach (Animal a in _animals)
            {
                if (a.AnimalID == id)
                {
                    _animals.Remove(a);
                    break;
                }
            }
        }

        public List<Animal> GetAll()
        {
            return _animals;
        }

        

    }
}

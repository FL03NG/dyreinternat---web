using dyreinternat___web.Models;

namespace dyreinternat___web.Repository
{
    public class AnimalCollection : IAnimal
    {
        public List<Animal> _animals;
        //hej
        public AnimalCollection()
        {
            _animals = new List<Animal>();
            Seed();
        }

        public void Add(Animal animal)
        {
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

        private void Seed()
        {
        

        }

    }
}

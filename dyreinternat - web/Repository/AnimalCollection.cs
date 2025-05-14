using dyreinternat___web.Models;

namespace dyreinternat___web.Repository
{
    public class AnimalCollection : IAnimal
    {
        private List<Animal> _animals;

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
            _animals.Add(new Animal(1, "Torben", "Bulldog", "Hund", 4, 8, "Han", ""));
            _animals.Add(new Animal(2, "Torbine", "Bulldog", "Hund", 4, 6, "Hun", ""));
            _animals.Add(new Animal(3, "Garfield", "Huskat", "Kat", 4, 4, "Han", ""));
            _animals.Add(new Animal(4, "Snoop Dogg", "Huskat", "Kat", 4, 5, "Hun", ""));

        }
    }
}

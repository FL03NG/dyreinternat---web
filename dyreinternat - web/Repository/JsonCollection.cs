using System.Text.Json;
using dyreinternat___web.Models;
using dyreinternat___web.Pages;

namespace dyreinternat___web.Repository
{
    public class JsonCollection : AnimalCollection
    {
        public JsonCollection() 
        {
            LoadFile();
        }

        private void LoadFile()
        {
            string path = "Animal.Json";
            string json = File.ReadAllText(path);
            _animals = JsonSerializer.Deserialize<List<Animal>>(json);
        }

        public void Add(Animal animal)
        {
            base.Add(animal);
            SaveFile();
        }

        private void SaveFile()
        {
            string path = "pets.json";
            File.WriteAllText(path, JsonSerializer.Serialize(_animals));
        }
    }
}

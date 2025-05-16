namespace dyreinternat___web.Models
{
    //hej med dig 
    public class Animal
    {
        public int AnimalID {  get; set; }
        public string Name { get; set; }
        public string Race { get; set; }
        public string Species { get; set; }
        public int Age { get; set; }
        public int Size { get; set; }
        public string Gender { get; set; }
        public string ImagePath { get; set; }

        public Animal(int animalID, string name, string race, string species, int age, int size, string gender, string imagePath)
        {
            AnimalID = animalID;
            Name = name;
            Race = race;
            Species = species;
            Age = age;
            Size = size;
            Gender = gender;
            ImagePath = imagePath;
        }
    }
}

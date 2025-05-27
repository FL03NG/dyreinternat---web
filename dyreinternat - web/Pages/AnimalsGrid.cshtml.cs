using dyreinternat___library.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using dyreinternat___library.Services;

namespace dyreinternat___web.Pages
{
    // PageModel til visning, s�gning og oprettelse af dyr
    public class AnimalsGridModel : PageModel
    {
        // Liste over alle dyr, der vises p� siden
        public List<Animal> Animal { get; set; } = new List<Animal>();

        // Sti til JSON-filen med alle dyrene
        private readonly string _animalFilePathJson;

        // Service til h�ndtering af dyredata
        private readonly AnimalService _animalService;

        // Constructor � s�tter filstien ud fra projektets rodmappe og initialiserer service
        public AnimalsGridModel(IWebHostEnvironment environment, AnimalService animalService)
        {
            _animalFilePathJson = Path.Combine(environment.ContentRootPath, "Animal.Json");
            _animalService = animalService;
        }

        // Filter til art/typen af dyr
        [BindProperty(SupportsGet = true)]
        public string FilterType { get; set; }

        // S�geterm til navnes�gning
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        // Dyr som brugeren kan tilf�je via formular
        [BindProperty]
        public Animal NewAnimal { get; set; } = new Animal();

        // GET-metode � henter og filtrerer dyr
        public void OnGet()
        {
            // Liste med alle dyr
            List<Animal> allAnimals = new List<Animal>();

            // Henter alle dyr fra service
            Animal = _animalService.GetAll();

            // L�s fra JSON-filen hvis den eksisterer
            if (System.IO.File.Exists(_animalFilePathJson))
            {
                string json = System.IO.File.ReadAllText(_animalFilePathJson);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<Animal>? deserialized = JsonSerializer.Deserialize<List<Animal>>(json);
                    if (deserialized != null)
                    {
                        allAnimals = deserialized;
                    }
                }
            }

            // Filtrer efter s�geterm og type
            List<Animal> filteredAnimals = new List<Animal>(); //ny tom liste med "Animal" objekter
            foreach (Animal a in allAnimals) //g�r igennem alle objekter i "allAnimals"
            {
                bool nameMatch = string.IsNullOrEmpty(SearchTerm) || a.Name.ToLower().Contains(SearchTerm.ToLower()); //checker om dyrenavnet matcher searchtermet hvis der er noget. 
                bool typeMatch = string.IsNullOrEmpty(FilterType) || a.Species.ToLower() == FilterType.ToLower(); //checker om dyrearten matcher filtertermet hvis der er noget

                if (nameMatch && typeMatch) //hvis searchtermet og filtertermet matcher,
                {
                    filteredAnimals.Add(a); //bliver den tilf�jet til "filteredAnimals".
                }
            }

            // Opdater visningslisten med filtrerede dyr
            Animal = filteredAnimals;
        }

        // POST-metode � tilf�jer nyt dyr
        public IActionResult OnPost()
        {
            // Tilf�j nyt dyr via service
            _animalService.Add(NewAnimal);

            // Hent eksisterende liste fra JSON
            List<Animal> animals = new List<Animal>();
            if (System.IO.File.Exists(_animalFilePathJson))
            {
                string jsonContent = System.IO.File.ReadAllText(_animalFilePathJson);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    List<Animal>? deserialized = JsonSerializer.Deserialize<List<Animal>>(jsonContent);
                    if (deserialized != null)
                    {
                        animals = deserialized;
                    }
                }
            }

            // OBS: Tilf�jelse til listen er udkommenteret
            //animals.Add(NewAnimal);

            // Gem den opdaterede liste til JSON-filen
            string updatedJson = JsonSerializer.Serialize(animals, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_animalFilePathJson, updatedJson);

            // Genindl�s siden
            return RedirectToPage();
        }

        // POST-metode � slet et dyr ud fra ID
        public IActionResult OnPostDelete(int animalID)
        {
            _animalService.Delete(animalID);
            return RedirectToPage(); // Opdaterer siden
        }
    }
}

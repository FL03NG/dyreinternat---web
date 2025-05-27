using dyreinternat___library.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using dyreinternat___library.Services;

namespace dyreinternat___web.Pages
{
    // PageModel til visning, søgning og oprettelse af dyr
    public class AnimalsGridModel : PageModel
    {
        // Liste over alle dyr, der vises på siden
        public List<Animal> Animal { get; set; } = new List<Animal>();

        // Sti til JSON-filen med alle dyrene
        private readonly string _animalFilePathJson;

        // Service til håndtering af dyredata
        private readonly AnimalService _animalService;

        // Constructor – sætter filstien ud fra projektets rodmappe og initialiserer service
        public AnimalsGridModel(IWebHostEnvironment environment, AnimalService animalService)
        {
            _animalFilePathJson = Path.Combine(environment.ContentRootPath, "Animal.Json");
            _animalService = animalService;
        }

        // Filter til art/typen af dyr
        [BindProperty(SupportsGet = true)]
        public string FilterType { get; set; }

        // Søgeterm til navnesøgning
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        // Dyr som brugeren kan tilføje via formular
        [BindProperty]
        public Animal NewAnimal { get; set; } = new Animal();

        // GET-metode – henter og filtrerer dyr
        public void OnGet()
        {
            // Liste med alle dyr
            List<Animal> allAnimals = new List<Animal>();

            // Henter alle dyr fra service
            Animal = _animalService.GetAll();

            // Læs fra JSON-filen hvis den eksisterer
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

            // Filtrer efter søgeterm og type
            List<Animal> filteredAnimals = new List<Animal>();
            foreach (Animal a in allAnimals)
            {
                bool nameMatch = string.IsNullOrEmpty(SearchTerm) || a.Name.ToLower().Contains(SearchTerm.ToLower());
                bool typeMatch = string.IsNullOrEmpty(FilterType) || a.Species.ToLower() == FilterType.ToLower();

                if (nameMatch && typeMatch)
                {
                    filteredAnimals.Add(a);
                }
            }

            // Opdater visningslisten med filtrerede dyr
            Animal = filteredAnimals;
        }

        // POST-metode – tilføjer nyt dyr
        public IActionResult OnPost()
        {
            // Tilføj nyt dyr via service
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

            // OBS: Tilføjelse til listen er udkommenteret
            //animals.Add(NewAnimal);

            // Gem den opdaterede liste til JSON-filen
            string updatedJson = JsonSerializer.Serialize(animals, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_animalFilePathJson, updatedJson);

            // Genindlæs siden
            return RedirectToPage();
        }

        // POST-metode – slet et dyr ud fra ID
        public IActionResult OnPostDelete(int animalID)
        {
            _animalService.Delete(animalID);
            return RedirectToPage(); // Opdaterer siden
        }
    }
}

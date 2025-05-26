using dyreinternat___library.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using dyreinternat___library.Services;

namespace dyreinternat___web.Pages
{
    public class AnimalsGridModel : PageModel
    {
        // Liste over alle dyr, der vises på siden
        public List<Animal> Animal { get; set; } = new List<Animal>();

        // Sti til JSON-filen med alle dyrene
        private readonly string _animalFilePathJson;
        private readonly AnimalService _animalService;

        // Constructor – sætter filstien ud fra projektets rodmappe
        public AnimalsGridModel(IWebHostEnvironment environment, AnimalService animalService)
        {
            _animalFilePathJson = Path.Combine(environment.ContentRootPath, "Animal.Json");
            _animalService = animalService;
        }

        // Brugeren kan søge efter navn og filtrere på dyretype
        [BindProperty(SupportsGet = true)]
        public string FilterType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        // Dyr som brugeren kan tilføje
        [BindProperty]
        public Animal NewAnimal { get; set; } = new Animal();

        // Kører når siden hentes (GET)
        public void OnGet()
        {
            // Liste til at holde alle læste dyr
            List<Animal> allAnimals = new List<Animal>();
            Animal = _animalService.GetAll();

            // Hvis JSON-filen findes, læs og konverter den til en liste
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

            // Filtrer dyr efter navn og art
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

            // Opdater visningslisten
            Animal = filteredAnimals;
        }

        // Kører når formularen postes (POST)
        public IActionResult OnPost()
        {
            // Læs eksisterende liste
            List<Animal> animals = new List<Animal>();
            _animalService.Add(NewAnimal);

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

            // Tilføj det nye dyr fra formularen
            //animals.Add(NewAnimal);

            // Gem den opdaterede liste tilbage til filen
            string updatedJson = JsonSerializer.Serialize(animals, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_animalFilePathJson, updatedJson);

            // Genindlæs siden med opdateret liste
            return RedirectToPage();
        }
        public IActionResult OnPostDelete(int animalID)
        {
            _animalService.Delete(animalID);
            return RedirectToPage(); // Refresh page
        }
    }
}
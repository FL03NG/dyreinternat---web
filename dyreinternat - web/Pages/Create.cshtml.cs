using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dyreinternat___library.Models;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace dyreinternat___web.Pages
{
    // PageModel til oprettelse af et nyt dyr, inkl. billedupload
    public class CreateModel : PageModel
    {
        // Det dyr der skal oprettes
        [BindProperty]
        public Animal Animal { get; set; } // aggregering

        // Det billede der uploades til dyret
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        // Service til håndtering af dyr
        private AnimalService _animalService;

        // Miljøvariabel til at finde stier i projektet (f.eks. wwwroot)
        private readonly IWebHostEnvironment _env;

        // Constructor – modtager dyreservice og miljøinformation
        public CreateModel(AnimalService animalService, IWebHostEnvironment env) // assosition
        {
            Animal = new Animal(); // composition
            _animalService = animalService;
            _env = env;
        }

        // GET-metode – bruges ved visning af siden
        public void OnGet()
        {
        }

        // POST-metode – gemmer dyret og eventuelt billede
        public IActionResult OnPost()
        {
            // Hvis brugeren har uploadet et billede
            if (ImageFile != null && ImageFile.Length > 0) // checks if a file is uploaded and is not empty
            {
                // Opret et unikt filnavn og find filsti
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName); // Creates a unique name to avoid the same name
                string filePath = Path.Combine(_env.WebRootPath, "Img", fileName);

                // Gem billedet på serveren
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                // Gem filnavnet (relativ sti) i modellen
                Animal.ImagePath = fileName;
            }

            // Tilføj dyret til servicen
            Debug.WriteLine("test");
            _animalService.Add(Animal);

            // Gå tilbage til oversigten
            return RedirectToPage("/AnimalsGrid");
        }
    }
}

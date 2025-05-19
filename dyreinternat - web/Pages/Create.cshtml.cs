using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dyreinternat___library.Models;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace dyreinternat___web.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Animal Animal { get; set; } //aggregering

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        private AnimalService _animalService;
        private readonly IWebHostEnvironment _env;

        public CreateModel(AnimalService animalService, IWebHostEnvironment env) //assosition
        {
            Animal = new Animal(); //composition
            _animalService = animalService;
            _env = env;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ImageFile != null && ImageFile.Length > 0) //checks if a file is uploaded and is not empty
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName); // Creates a unique name to avoid the same name
                string filePath = Path.Combine(_env.WebRootPath, "Img", fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                // Save relative path in the JSON
                Animal.ImagePath = fileName;
            }

            Debug.WriteLine("test");
            _animalService.Add(Animal);

            return RedirectToPage("/AnimalsGrid");
        }
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dyreinternat___web.Models;
using dyreinternat___web.Services;

namespace dyreinternat___web.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Animal Animal { get; set; }

        private AnimalService _animalService;

        public CreateModel(AnimalService animalService)
        {
            Animal = new Animal();
            _animalService = animalService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            Debug.WriteLine("test");
            _animalService.Add(Animal);

            return RedirectToPage("/Index");
        }
    }
}

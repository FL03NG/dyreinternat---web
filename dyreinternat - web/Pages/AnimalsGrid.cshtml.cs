using dyreinternat___web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;

namespace dyreinternat___web.Pages
{
    public class AnimalsGridModel : PageModel
    {
        public List<Animal> animal = new List<Animal>();

        public void OnGet()
        {
            Seed(); // Fylder listen når siden loades
        }

        private void Seed()
        {
            animal.Add(new Animal(1, "Torben", "Bulldog", "Hund", 4, 8, "Han", "bulldog.jpg"));
            animal.Add(new Animal(2, "Torbine", "Bulldog", "Hund", 4, 6, "Hun", "bulldog2.jpg"));
            animal.Add(new Animal(3, "Garfield", "Huskat", "Kat", 4, 4, "Han", "cat.jpg"));
            animal.Add(new Animal(4, "Snoop Dogg", "Huskat", "Kat", 4, 5, "Hun", "orangeCat.jpg"));
        }
    }
}

    


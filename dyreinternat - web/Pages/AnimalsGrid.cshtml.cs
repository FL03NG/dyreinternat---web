using dyreinternat___web.Models;
using dyreinternat___web.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace dyreinternat___web.Pages
{
    public class AnimalsGridModel : PageModel
    {// Den liste, der vises på siden
        
        public List<Animal> Animal = new List<Animal>();

        [BindProperty(SupportsGet = true)]
        //filter
        public string FilterType { get; set; }
        // Søgeord fra brugeren (f.eks. "Garfield")
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            // 1. Vi laver en liste med alle dyr
            List<Animal> allAnimals = new List<Animal>
            { 
            new Animal(1, "Torben", "Bulldog", "Hund", 4, 8, "Han", "bulldog.jpg"),
            new Animal(2, "Torbine", "Bulldog", "Hund", 4, 6, "Hun", "bulldog2.jpg"),
            new Animal(3, "Garfield", "Huskat", "Kat", 4, 4, "Han", "cat.jpg"),
            new Animal(4, "Snoop Dogg", "Huskat", "Kat", 4, 5, "Hun", "orangeCat.jpg"),
            new Animal(5, "Egon", "Bulldog", "Hund", 4, 8, "Han", "bulldog.jpg"),
            new Animal(6, "Torbine", "Bulldog", "Hund", 4, 6, "Hun", "bulldog2.jpg"),
            new Animal(7, "Garfield", "Huskat", "Kat", 4, 4, "Han", "orangecat.jpg"),
            new Animal(8, "Snoop Dogg", "Huskat", "Kat", 4, 5, "Hun", "Cat.jpg")
            };
            

            foreach (Animal a in allAnimals)
            {
                bool nameMatch = true;
                // 2. Hvis brugeren har skrevet noget i søgefeltet
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    // 3. Vi går igennem alle dyr én for én
                    nameMatch = a.Name.ToLower().Contains(SearchTerm.ToLower());
                }

                bool typeMatch = true;
                if (!string.IsNullOrEmpty(FilterType))
                {
                    typeMatch = a.Species.ToLower() == FilterType.ToLower();
                }

                if (nameMatch && typeMatch)
                {
                    Animal.Add(a);
                }
            }

           

        }

        

        
        

    }
}

    


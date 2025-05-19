using dyreinternat___library.Models;
using dyreinternat___library.Repository;
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

        public List<Animal> Animal { get; set; } = new();
        private readonly string _animalFilePathJson;
        public AnimalsGridModel(IWebHostEnvironment environment)
        {
            _animalFilePathJson = Path.Combine(environment.ContentRootPath, "Animal.Json");
        }

        [BindProperty(SupportsGet = true)]
        //filter
        public string FilterType { get; set; }
        // Søgeord fra brugeren (f.eks. "Garfield")
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        
        public void OnGet()
        {
            Debug.WriteLine("OnGet method started.");

            // Check if the file exists
            Debug.WriteLine($"Checking if file exists: {_animalFilePathJson}");
            if (System.IO.File.Exists(_animalFilePathJson))
            {
                Debug.WriteLine("File found. Reading content...");
                var json = System.IO.File.ReadAllText(_animalFilePathJson);

                // Log the content of the file (optional, avoid for large files)
                Debug.WriteLine($"File content: {json}");

                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        // Attempt to deserialize the JSON
                        Debug.WriteLine("Deserializing JSON...");
                        Animal = JsonSerializer.Deserialize<List<Animal>>(json) ?? new();
                        Debug.WriteLine($"Deserialization successful. Loaded {Animal.Count} boats.");
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Debug.WriteLine($"Error deserializing JSON: {ex.Message}");
                        Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                        Animal = new();
                    }
                }
                else
                {
                    Debug.WriteLine("File content is empty or whitespace.");
                    Animal= new();
                }
            }
            else
            {
                Debug.WriteLine("File not found.");
                Animal = new();
            }

            Debug.WriteLine("OnGet method completed.");



            foreach (Animal a in Animal)
            {
                bool nameMatch = true;
                 //2. Hvis brugeren har skrevet noget i søgefeltet
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

                //if (nameMatch && typeMatch)
                //{
                //    Animal.Add(a);
                //}
            }



        }

        

        
        

    }
}

    


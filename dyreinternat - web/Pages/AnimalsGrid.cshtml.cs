using dyreinternat___web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;

namespace dyreinternat___web.Pages
{
    public class AnimalsGridModel : PageModel
    {

        public List<Animal> animal { get; set; } = new();

        private readonly string boatFilePathJson;

        public AnimalsGridModel(IWebHostEnvironment environment)
        {
            boatFilePathJson = Path.Combine(environment.ContentRootPath, "BoatData.json");
            Debug.WriteLine($"Boat file path: {boatFilePathJson}"); // Log the file path for debugging
        }
        public void OnGet()
        {
            Debug.WriteLine("OnGet method started.");

            // Check if the file exists
            Debug.WriteLine($"Checking if file exists: {boatFilePathJson}");
            if (System.IO.File.Exists(boatFilePathJson))
            {
                Debug.WriteLine("File found. Reading content...");
                var json = System.IO.File.ReadAllText(boatFilePathJson);

                // Log the content of the file (optional, avoid for large files)
                Debug.WriteLine($"File content: {json}");

                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        // Attempt to deserialize the JSON
                        Debug.WriteLine("Deserializing JSON...");
                        animal = JsonSerializer.Deserialize<List<Animal>>(json) ?? new();
                        Debug.WriteLine($"Deserialization successful. Loaded {animal.Count} boats.");
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Debug.WriteLine($"Error deserializing JSON: {ex.Message}");
                        Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                        animal = new();
                    }
                }
                else
                {
                    Debug.WriteLine("File content is empty or whitespace.");
                    animal = new();
                }
            }
            else
            {
                Debug.WriteLine("File not found.");
                animal = new();
            }

            Debug.WriteLine("OnGet method completed.");
        }
    }
}

    


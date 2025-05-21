using dyreinternat___library.Services;
using dyreinternat___library.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    public class DocJournalModel : PageModel
    {
        [BindProperty]
        public DocJournal DocJournal { get; set; } = new DocJournal(); // Aggregation

        [BindProperty]
        public DocJournal NewDocJournal { get; set; } = new DocJournal(); // Tilføj ny lægelog

        private readonly DocJournalService _docJournalService;
        private readonly string _docJournalFilePathJson;

        public List<DocJournal> Lægelog { get; set; } = new List<DocJournal>();

        // Konstruktor – modtager service og miljø
        public DocJournalModel(DocJournalService docJournalService, IWebHostEnvironment environment)
        {
            _docJournalService = docJournalService;
            _docJournalFilePathJson = Path.Combine(environment.ContentRootPath, "docJournal.json");
        }
        // Kører ved GET – henter lægelogs fra fil
        public void OnGet()
        {
            List<DocJournal> allDocJournals = new List<DocJournal>();

            if (System.IO.File.Exists(_docJournalFilePathJson))
            {
                string json = System.IO.File.ReadAllText(_docJournalFilePathJson);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<DocJournal>? deserialized = JsonSerializer.Deserialize<List<DocJournal>>(json);
                    if (deserialized != null)
                    {
                        allDocJournals = deserialized;
                    }
                }
            }
            Lægelog = allDocJournals;
        }

        public IActionResult OnPost()
        {
            // Gem i service (hvis den gør noget)
            _docJournalService.Add(NewDocJournal);

            // Læs eksisterende lægelogs
            List<DocJournal> docJournals = new List<DocJournal>();

            if (System.IO.File.Exists(_docJournalFilePathJson))
            {
                string jsonContent = System.IO.File.ReadAllText(_docJournalFilePathJson);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    List<DocJournal>? deserialized = JsonSerializer.Deserialize<List<DocJournal>>(jsonContent);
                    if (deserialized != null)
                    {
                        docJournals = deserialized;
                    }
                }
            }

            


            // Skriv hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(docJournals, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_docJournalFilePathJson, updatedJson);

            return RedirectToPage(); // Genindlæs siden
        }


    }
}

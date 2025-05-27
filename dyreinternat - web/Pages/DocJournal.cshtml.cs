using dyreinternat___library.Services;
using dyreinternat___library.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    // PageModel til visning og oprettelse af lægelogs (DocJournal)
    public class DocJournalModel : PageModel
    {
        // Binding til eksisterende lægelog (visning/redigering)
        [BindProperty]
        public DocJournal DocJournal { get; set; } = new DocJournal(); // Aggregation

        // Binding til ny lægelog fra formular
        [BindProperty]
        public DocJournal NewDocJournal { get; set; } = new DocJournal(); // Tilføj ny lægelog

        // Service til håndtering af lægelogs
        private readonly DocJournalService _docJournalService;

        // Sti til JSON-filen med lægelogs
        private readonly string _docJournalFilePathJson;

        // Liste til visning i grid
        public List<DocJournal> DocJournalGrid { get; set; } = new List<DocJournal>();

        // Konstruktor – modtager service og miljø
        public DocJournalModel(DocJournalService docJournalService, IWebHostEnvironment environment)
        {
            _docJournalService = docJournalService;
            _docJournalFilePathJson = Path.Combine(environment.ContentRootPath, "docJournal.json");
        }

        // GET-metode – henter lægelogs fra fil og sætter listen til visning
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

            DocJournalGrid = allDocJournals;
        }

        // POST-metode – tilføjer ny lægelog og opdaterer filen
        public IActionResult OnPost()
        {
            // Tilføj via service
            _docJournalService.Add(NewDocJournal);

            // Læs eksisterende lægelogs fra fil
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

        // POST-metode – sletter lægelog via ID og opdaterer filen
        public IActionResult OnPostDelete(int docJournalID)
        {
            _docJournalService.Delete(docJournalID);
            return RedirectToPage(); // Refresh page
        }
    }
}

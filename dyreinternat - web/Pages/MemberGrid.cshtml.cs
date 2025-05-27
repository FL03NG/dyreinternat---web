using dyreinternat___library.Models;
using dyreinternat___library.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    // PageModel til visning og oprettelse af medlemmer (Account)
    public class MemberGridModel : PageModel
    {
        // Binding til eksisterende konto
        [BindProperty]
        public Account Account { get; set; } = new Account(); // new = Composition

        // Binding til ny konto fra formular
        [BindProperty]
        public Account NewAccount { get; set; } = new Account(); // Tilføj ny lægelog

        // Service til håndtering af konto-data
        private readonly MemberService _accountService;

        // Sti til JSON-filen med medlemmer
        private readonly string _accountFilePathJson;

        // Liste over konti der vises på siden
        public List<Account> AccountGrid { get; set; } = new List<Account>();

        // Konstruktor – modtager service og miljø
        public MemberGridModel(MemberService accountService, IWebHostEnvironment environment)
        {
            _accountService = accountService;
            _accountFilePathJson = Path.Combine(environment.ContentRootPath, "Member.json");
        }

        // GET-metode – henter konti fra JSON-fil og viser dem
        public void OnGet()
        {
            List<Account> allAccounts = new List<Account>();

            if (System.IO.File.Exists(_accountFilePathJson))
            {
                string json = System.IO.File.ReadAllText(_accountFilePathJson);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<Account>? deserialized = JsonSerializer.Deserialize<List<Account>>(json);
                    if (deserialized != null)
                    {
                        allAccounts = deserialized;
                    }
                }
            }

            AccountGrid = allAccounts;
        }

        // POST-metode – tilføjer ny konto og gemmer opdateret liste
        public IActionResult OnPost()
        {
            // Tilføj konto via service
            _accountService.Add(NewAccount);

            // Læs eksisterende konti fra fil
            List<Account> accounts = new List<Account>();

            if (System.IO.File.Exists(_accountFilePathJson))
            {
                string jsonContent = System.IO.File.ReadAllText(_accountFilePathJson);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    List<Account>? deserialized = JsonSerializer.Deserialize<List<Account>>(jsonContent);
                    if (deserialized != null)
                    {
                        accounts = deserialized;
                    }
                }
            }

            // Gem hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_accountFilePathJson, updatedJson);

            return RedirectToPage(); // Genindlæs siden
        }

        // POST-metode – sletter konto baseret på ID
        public IActionResult OnPostDelete(int accountID)
        {
            _accountService.Delete(accountID);
            return RedirectToPage(); // Refresh page
        }
    }
}

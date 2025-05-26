using dyreinternat___library.Models;
using dyreinternat___library.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    public class MemberGridModel : PageModel
    {
        [BindProperty]
        public Account Account { get; set; } = new Account(); // Aggregation

        [BindProperty]
        public Account NewAccount { get; set; } = new Account(); // Tilføj ny lægelog

        private readonly MemberService _accountService;
        private readonly string _accountFilePathJson;

        public List<Account> AccountGrid { get; set; } = new List<Account>();

        // Konstruktor – modtager service og miljø
        public MemberGridModel(MemberService accountService, IWebHostEnvironment environment)
        {
            _accountService = accountService;
            _accountFilePathJson = Path.Combine(environment.ContentRootPath, "Member.json");
        }
        // Kører ved GET – henter lægelogs fra fil
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

        public IActionResult OnPost()
        {
            // Gem i service (hvis den gør noget)
            _accountService.Add(NewAccount);

            // Læs eksisterende lægelogs
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




            // Skriv hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_accountFilePathJson, updatedJson);

            return RedirectToPage(); // Genindlæs siden
        }
    }
}

using System;
using System.Text.Json;
using dyreinternat___library.Models;
using dyreinternat___library.Repository;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    // PageModel til visning og oprettelse af aktiviteter
    public class ActivityModel : PageModel
    {
        // Aktivitet til binding (f.eks. visning)
        [BindProperty]
        public Activity Activity { get; set; } = new Activity(); // Aggregation

        // Aktivitet der skal tilføjes
        [BindProperty]
        public Activity NewActivity { get; set; } = new Activity(); // Tilføj ny aktivitet

        // Service til håndtering af aktiviteter
        private readonly ActivityService _activityService;

        // Sti til JSON-filen med aktiviteter
        private readonly string _activityFilePathJson;

        // Liste til visning af aktiviteter
        public List<Activity> ActivityGrid { get; set; } = new List<Activity>();

        // Konstruktor – modtager service og miljø
        public ActivityModel(ActivityService activityService, IWebHostEnvironment environment)
        {
            _activityService = activityService;
            _activityFilePathJson = Path.Combine(environment.ContentRootPath, "Activity.json");
        }

        // Håndterer GET-anmodninger – henter og deserialiserer aktiviteter fra fil
        public void OnGet()
        {
            List<Activity> allActivities = new List<Activity>();

            if (System.IO.File.Exists(_activityFilePathJson))
            {
                string json = System.IO.File.ReadAllText(_activityFilePathJson);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<Activity>? deserialized = JsonSerializer.Deserialize<List<Activity>>(json);
                    if (deserialized != null)
                    {
                        allActivities = deserialized;
                    }
                }
            }
        }

        // Håndterer POST-anmodninger – tilføjer ny aktivitet og opdaterer filen
        public IActionResult OnPost()
        {
            // Gem ny aktivitet via service
            _activityService.Add(NewActivity);

            // Læs eksisterende aktiviteter fra fil
            List<Activity> activities = new List<Activity>();

            if (System.IO.File.Exists(_activityFilePathJson))
            {
                string jsonContent = System.IO.File.ReadAllText(_activityFilePathJson);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    List<Activity>? deserialized = JsonSerializer.Deserialize<List<Activity>>(jsonContent);
                    if (deserialized != null)
                    {
                        activities = deserialized;
                    }
                }
            }

          

            // Skriv hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(activities, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_activityFilePathJson, updatedJson);

            // Omdirigerer til forsiden
            return RedirectToPage("/Index");
        }
    }
}

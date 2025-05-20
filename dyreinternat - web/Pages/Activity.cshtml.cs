using System;
using System.Text.Json;
using dyreinternat___library.Models;
using dyreinternat___library.Repository;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    public class ActivityModel : PageModel
    {
        [BindProperty]
        public Activity Activity { get; set; } = new Activity(); // Aggregation

        [BindProperty]
        public Activity NewActivity { get; set; } = new Activity(); // Tilføj ny aktivitet

        private readonly ActivityService _activityService;
        private readonly string _activityFilePathJson;

        public List<Activity> ActivityGrid { get; set; } = new List<Activity>();

        // Konstruktor – modtager service og miljø
        public ActivityModel(ActivityService activityService, IWebHostEnvironment environment)
        {
            _activityService = activityService;
            _activityFilePathJson = Path.Combine(environment.ContentRootPath, "Activity.json");
        }
        // Kører ved GET – henter aktiviteter fra fil
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

        public IActionResult OnPost()
        {
            // Gem i service (hvis den gør noget)
            _activityService.Add(NewActivity);

            // Læs eksisterende aktiviteter
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

            // Tilføj den nye aktivitet
            activities.Add(NewActivity);

            // Skriv hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(activities, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_activityFilePathJson, updatedJson);

            return RedirectToPage("/Index"); // Genindlæs siden
        }




    }
}

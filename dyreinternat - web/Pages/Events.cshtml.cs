using System;
using System.Text.Json;
using dyreinternat___library.Models;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    public class EventsModel : PageModel
    {
        [BindProperty]
        public Event Event { get; set; } //aggregering

        private EventService _eventService;

        public List<Event> EventGrid { get; set; } = new List<Event>();
        private readonly string _eventFilePathJson;

        [BindProperty]
        public Event NewEvent { get; set; } = new Event();
        public EventsModel(EventService eventService, IWebHostEnvironment environment) //assosition
        {
            Event = new Event(); //composition
            _eventService = eventService;
            _eventFilePathJson = Path.Combine(environment.ContentRootPath, "Event.Json");

        }
        public void OnGet()
        {
            List<Event> allEvents = new List<Event>();

            // Hvis JSON-filen findes, læs og konverter den til en liste
            if (System.IO.File.Exists(_eventFilePathJson))
            {
                string json = System.IO.File.ReadAllText(_eventFilePathJson);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<Event>? deserialized = JsonSerializer.Deserialize<List<Event>>(json);
                    if (deserialized != null)
                    {
                        allEvents = deserialized;
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            _eventService.Add(Event);

            List<Event> events = new List<Event>();

            if (System.IO.File.Exists(_eventFilePathJson))
            {
                string jsonContent = System.IO.File.ReadAllText(_eventFilePathJson);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    List<Event>? deserialized = JsonSerializer.Deserialize<List<Event>>(jsonContent);
                    if (deserialized != null)
                    {
                        events = deserialized;
                    }
                }
            }

            // Tilføj det nye dyr fra formularen
            events.Add(NewEvent);

            // Gem den opdaterede liste tilbage til filen
            string updatedJson = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_eventFilePathJson, updatedJson);

            // Genindlæs siden med opdateret liste
            return RedirectToPage();

        }

        

        
    }
}

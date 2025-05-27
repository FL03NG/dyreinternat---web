using System.Text.Json;
using dyreinternat___library.Models;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    // PageModel til visning og oprettelse af bookinger
    public class BookModel : PageModel
    {
        // Binding til eksisterende booking (f.eks. til visning/redigering)
        [BindProperty]
        public Booking Booking { get; set; } = new Booking(); // Aggregation

        // Binding til ny booking fra formular
        [BindProperty]
        public Booking NewBooking { get; set; } = new Booking(); // Tilføj ny aktivitet

        // Booking service til datahåndtering
        private readonly BookingService _bookingService;

        // Sti til JSON-filen med bookinger
        private readonly string _bookingFilePathJson;

        // Liste der vises i et grid
        public List<Booking> BookingGrid { get; set; } = new List<Booking>();

        // Constructor – modtager service og miljøvariabel
        public BookModel(BookingService bookingService, IWebHostEnvironment environment)
        {
            _bookingService = bookingService;
            _bookingFilePathJson = Path.Combine(environment.ContentRootPath, "Booking.json");
        }

        // Liste til visning af bookinger
        public List<Booking> Bookings { get; set; } = new List<Booking>();

        // GET-metode – henter booking-data fra fil og service
        public void OnGet()
        {
            List<Booking> allBooking = new List<Booking>();

            // Henter bookinger fra JSON hvis filen findes
            if (System.IO.File.Exists(_bookingFilePathJson))
            {
                string json = System.IO.File.ReadAllText(_bookingFilePathJson);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<Booking>? deserialized = JsonSerializer.Deserialize<List<Booking>>(json);
                    if (deserialized != null)
                    {
                        allBooking = deserialized;
                    }
                }
            }

            // Henter bookinger fra service
            Bookings = _bookingService.GetAll();
        }

        // POST-metode – tilføjer ny booking
        public IActionResult OnPost()
        {
            // Tilføj booking via service
            _bookingService.Add(NewBooking);

            // Læs eksisterende bookinger fra fil
            List<Booking> bookings = new List<Booking>();

            if (System.IO.File.Exists(_bookingFilePathJson))
            {
                string jsonContent = System.IO.File.ReadAllText(_bookingFilePathJson);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    List<Booking>? deserialized = JsonSerializer.Deserialize<List<Booking>>(jsonContent);
                    if (deserialized != null)
                    {
                        bookings = deserialized;
                    }
                }
            }

            
            //bookings.Add(NewBooking);

            // Skriv opdateret liste tilbage til filen
            string updatedJson = JsonSerializer.Serialize(bookings, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_bookingFilePathJson, updatedJson);

            // Genindlæs siden
            return RedirectToPage();
        }

        // POST-metode – sletter booking baseret på ID
        public IActionResult OnPostDelete(int animalID)
        {
            _bookingService.Delete(animalID);
            return RedirectToPage(); // Refresh page
        }
    }
}

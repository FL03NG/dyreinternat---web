using System.Text.Json;
using dyreinternat___library.Models;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    public class BookModel : PageModel
    {
        [BindProperty]
        public Booking Booking { get; set; } = new Booking(); // Aggregation

        [BindProperty]
        public Booking NewBooking { get; set; } = new Booking(); // Tilføj ny aktivitet

        private readonly BookingService _bookingService;
        private readonly string _bookingFilePathJson;

        public List<Booking> BookingGrid { get; set; } = new List<Booking>();

        // Konstruktor – modtager service og miljø
        public BookModel(BookingService bookingService, IWebHostEnvironment environment)
        {
            _bookingService = bookingService;
            _bookingFilePathJson = Path.Combine(environment.ContentRootPath, "Booking.json");
        }

        

        public List<Booking> Bookings { get; set; } = new List<Booking>();
        // Kører ved GET – henter aktiviteter fra fil
        public void OnGet()
        {
            List<Booking> allBooking = new List<Booking>();

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
            Bookings = _bookingService.GetAll();
        }

        public IActionResult OnPost()
        {
            // Gem i service (hvis den gør noget)
            _bookingService.Add(NewBooking);

            // Læs eksisterende aktiviteter
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

            // Tilføj den nye aktivitet
            //bookings.Add(NewBooking);

            // Skriv hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(bookings, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_bookingFilePathJson, updatedJson);

            return RedirectToPage(); // Genindlæs siden
        }
        public IActionResult OnPostDelete(int animalID)
        {
            _bookingService.Delete(animalID);
            return RedirectToPage(); // Refresh page
        }

    }
}

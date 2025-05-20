using dyreinternat___library.Models;
using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dyreinternat___web.Pages
{
    public class BookModel : PageModel
    {
        [BindProperty]
        public Booking Booking { get; set; } //aggregering
        public List<Booking> BookingGrid { get; set; } = new List<Booking>();

        private readonly string _bookingFilePathJson;

        private BookingService _bookingService;

        public BookModel(BookingService bookingService, IWebHostEnvironment environment) //assosition
        {
            Booking = new Booking(); //composition
            _bookingService = bookingService;
            _bookingFilePathJson = Path.Combine(environment.ContentRootPath, "Booking.Json");
        }

        public IActionResult OnPost()
        {
            _bookingService.Add(Booking);

            return RedirectToPage();
        }

        public void OnGet()
        {
        }
    }
}

using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dyreinternat___library.Models;


namespace dyreinternat___web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ActivityService _activityService;

        public IndexModel(ActivityService activityService)
        {
            _activityService = activityService;
        }

        public List<Activity> Activities { get; set; } = new List<Activity>();

        public void OnGet()
        {
            Activities = _activityService.GetAll();
        }
    }
}

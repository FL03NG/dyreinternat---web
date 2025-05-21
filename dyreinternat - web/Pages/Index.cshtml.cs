using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dyreinternat___library.Models;
using System.Text.Json;


namespace dyreinternat___web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ActivityService _activityService;

        public IndexModel(ActivityService activityService, BlogService blogService, IWebHostEnvironment environment)
        {
            _activityService = activityService;
            _blogService = blogService;
            _blogFilePathJson = Path.Combine(environment.ContentRootPath, "Blog.json");
        }

        public List<Activity> Activities { get; set; } = new List<Activity>();

        public void OnGet()
        {
            Activities = _activityService.GetAll();
            Blogs = _blogService.GetAll();
            List<Blog> allBlogs = new List<Blog>();

            if (System.IO.File.Exists(_blogFilePathJson))
            {
                string json = System.IO.File.ReadAllText(_blogFilePathJson);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    List<Blog>? deserialized = JsonSerializer.Deserialize<List<Blog>>(json);
                    if (deserialized != null)
                    {
                        allBlogs = deserialized;
                    }
                }
            }
        }
        public IActionResult OnPostLikes(int activityID)
        {
            Activity activity = _activityService.Get(activityID);
            if (activity != null)
            {
                activity.Tilmeldt++;
                _activityService.Update(activity);
            }
            return RedirectToPage();
        }
        //----------------------------------Blog------------------------------------------------//

        private readonly BlogService _blogService;

        

        public List<Blog> Blogs { get; set; } = new List<Blog>();

        [BindProperty]
        public Blog Blog { get; set; } = new Blog(); // Aggregation

        [BindProperty]
        public Blog NewBlog { get; set; } = new Blog(); // Tilføj ny aktivitet

        
        private readonly string _blogFilePathJson;

        public List<Blog> BlogGrid { get; set; } = new List<Blog>();

        public IActionResult OnPost()
        {
            // Gem i service (hvis den gør noget)
            _blogService.Add(NewBlog);

            // Læs eksisterende aktiviteter
            List<Blog> blogs = new List<Blog>();

            if (System.IO.File.Exists(_blogFilePathJson))
            {
                string jsonContent = System.IO.File.ReadAllText(_blogFilePathJson);
                if (!string.IsNullOrWhiteSpace(jsonContent))
                {
                    List<Blog>? deserialized = JsonSerializer.Deserialize<List<Blog>>(jsonContent);
                    if (deserialized != null)
                    {
                        blogs = deserialized;
                    }
                }
            }

            // Tilføj den nye aktivitet
            //activities.Add(NewActivity);

            // Skriv hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(blogs, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_blogFilePathJson, updatedJson);

            return RedirectToPage(); // Genindlæs siden
        }
        public IActionResult OnPostDelete(int blogID)
        {
            _blogService.Delete(blogID);
            return RedirectToPage(); // Refresh page
        }


    }
}

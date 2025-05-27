using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dyreinternat___library.Models;
using System.Text.Json;

namespace dyreinternat___web.Pages
{
    // PageModel til forsiden, der viser aktiviteter og blogindl�g
    public class IndexModel : PageModel
    {
        // Giver adgang til aktivitetsdata gennem servicelaget
        private readonly ActivityService _activityService;

        // Giver adgang til blogdata gennem servicelaget
        private readonly BlogService _blogService;

        // Fuld sti til JSON-filen for blogs
        private readonly string _blogFilePathJson;

        // Liste over aktiviteter til visning
        public List<Activity> Activities { get; set; } = new List<Activity>();

        // Liste over blogs til visning
        public List<Blog> Blogs { get; set; } = new List<Blog>();

        // Binding til en blog (til oprettelse eller redigering)
        [BindProperty]
        public Blog Blog { get; set; } = new Blog(); // Aggregation

        // Binding til ny blog fra formular
        [BindProperty]
        public Blog NewBlog { get; set; } = new Blog(); // Tilf�j ny aktivitet

        // Bruges til at vise alle blogs p� siden (fx i et grid)
        public List<Blog> BlogGrid { get; set; } = new List<Blog>();

        // Konstruktor � modtager n�dvendige services og milj�data
        public IndexModel(ActivityService activityService, BlogService blogService, IWebHostEnvironment environment)
        {
            _activityService = activityService;
            _blogService = blogService;
            _blogFilePathJson = Path.Combine(environment.ContentRootPath, "Blog.json"); // Bygger stien til blogfilen
        }

        // GET-metode � k�res automatisk ved indl�sning af siden
        public void OnGet()
        {
            // Hent aktiviteter og blogs via deres services
            Activities = _activityService.GetAll();
            Blogs = _blogService.GetAll();

            // Midlertidig liste til blogdata fra JSON
            List<Blog> allBlogs = new List<Blog>();

            // L�s og deserialiser blogdata fra JSON-filen
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

        // POST-metode � h�ndterer tilmelding til en aktivitet
        public IActionResult OnPostJoin(int activityID)
        {
            // Find aktivitet ud fra ID
            Activity activity = _activityService.Get(activityID);

            // Hvis den findes, opdater deltagerantal og gem
            if (activity != null)
            {
                activity.Tilmeldt++;
                _activityService.Update(activity);
            }

            return RedirectToPage(); // Genindl�s siden
        }

        // POST-metode � h�ndterer tilf�jelse af ny blog
        public IActionResult OnPost()
        {
            // Tilf�j ny blog via service
            _blogService.Add(NewBlog);

            // L�s eksisterende blogs fra JSON
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

            // OBS: Tilf�jelse til listen er udkommenteret
            // blogs.Add(NewBlog);

            // Gem den opdaterede liste til JSON-filen
            string updatedJson = JsonSerializer.Serialize(blogs, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(_blogFilePathJson, updatedJson);

            return RedirectToPage(); // Genindl�s siden
        }

        // POST-metode � sletter blog via ID
        public IActionResult OnPostDeleteBlog(int blogID)
        {
            _blogService.Delete(blogID);
            return RedirectToPage(); // Genindl�s siden
        }

        // POST-metode � sletter aktivitet via ID
        public IActionResult OnPostDeleteActivity(int activityID)
        {
            _activityService.Delete(activityID);
            return RedirectToPage(); // Genindl�s siden
        }
    }
}

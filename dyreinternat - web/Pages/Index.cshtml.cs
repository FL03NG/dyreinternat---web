using dyreinternat___library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dyreinternat___library.Models;
using System.Text.Json;


namespace dyreinternat___web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ActivityService _activityService; //giver adgang til aktivitetsdata

        public IndexModel(ActivityService activityService, BlogService blogService, IWebHostEnvironment environment) //h�ndterer logik
        {
            _activityService = activityService;
            _blogService = blogService;
            _blogFilePathJson = Path.Combine(environment.ContentRootPath, "Blog.json"); //bygger stien til json filen
        }

        public List<Activity> Activities { get; set; } = new List<Activity>(); //en property som indenholder listen af aktiviteter p� siden

        public void OnGet() //k�rer automatisk
        {
            Activities = _activityService.GetAll(); //f�r fat i alle aktiviteterne igennem deres servicelag
            Blogs = _blogService.GetAll(); //f�r fat i alle blogs igennem deres servicelag
            List<Blog> allBlogs = new List<Blog>(); //en midlertidig liste til at holde de loadede blogs

            if (System.IO.File.Exists(_blogFilePathJson)) //tjekker om "Blog.Json" eksisterer. hvis den g�r s� l�ser den det og inds�tter det ind i en liste af objekter
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
        public IActionResult OnPostJoin(int activityID) //h�ndterer tilmeldning af aktiviteter
        {
            Activity activity = _activityService.Get(activityID); //finder aktiviteten fra id,
            if (activity != null) //hvis den eksisterer,
            {
                activity.Tilmeldt++; //t�lder den "Tilmeldt" t�lleren op.
                _activityService.Update(activity); //opdaterer aktivitet
            }
            return RedirectToPage(); //genindl�ser siden
        }
        //----------------------------------Blog------------------------------------------------//

        private readonly BlogService _blogService; //giver adgang til blogdata



        public List<Blog> Blogs { get; set; } = new List<Blog>(); //en property som indenholder listen af blogs p� siden

        [BindProperty] //properties bundet til at forme data til at l�se og skrive blogs
        public Blog Blog { get; set; } = new Blog(); // Aggregation

        [BindProperty]
        public Blog NewBlog { get; set; } = new Blog(); // Tilf�j ny aktivitet

        
        private readonly string _blogFilePathJson; //den fulde sti til Json filen for blogs

        public List<Blog> BlogGrid { get; set; } = new List<Blog>(); //brugt til at vise alle blogs p� indexsiden

        public IActionResult OnPost() //h�ndterer generale blog opdateringer
        {
            // Gemmer ny blog i service (hvis den g�r noget)
            _blogService.Add(NewBlog);

            // L�s eksisterende blogs fra json fil
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

            // Tilf�j den nye aktivitet
            //activities.Add(NewActivity);

            // Skriv hele listen tilbage til filen
            string updatedJson = JsonSerializer.Serialize(blogs, new JsonSerializerOptions { WriteIndented = true }); //udskriver den opdaterede blogliste og udskriver det i Json filen
            System.IO.File.WriteAllText(_blogFilePathJson, updatedJson);

            return RedirectToPage(); // Genindl�s siden
        }
        public IActionResult OnPostDeleteBlog(int blogID) //sletter en blog ved brug af service
        {
            _blogService.Delete(blogID);
            return RedirectToPage(); // Genindl�s siden
        }
        public IActionResult OnPostDeleteActivity(int activityID) //sletter en aktivitet ved brug af service
        {
            
            _activityService.Delete(activityID);
            return RedirectToPage(); // Genindl�s siden
        }


    }
}

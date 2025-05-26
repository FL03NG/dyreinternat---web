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

        public IndexModel(ActivityService activityService, BlogService blogService, IWebHostEnvironment environment) //håndterer logik
        {
            _activityService = activityService;
            _blogService = blogService;
            _blogFilePathJson = Path.Combine(environment.ContentRootPath, "Blog.json"); //bygger stien til json filen
        }

        public List<Activity> Activities { get; set; } = new List<Activity>(); //en property som indenholder listen af aktiviteter på siden

        public void OnGet() //kører automatisk
        {
            Activities = _activityService.GetAll(); //får fat i alle aktiviteterne igennem deres servicelag
            Blogs = _blogService.GetAll(); //får fat i alle blogs igennem deres servicelag
            List<Blog> allBlogs = new List<Blog>(); //en midlertidig liste til at holde de loadede blogs

            if (System.IO.File.Exists(_blogFilePathJson)) //tjekker om "Blog.Json" eksisterer. hvis den gør så læser den det og indsætter det ind i en liste af objekter
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
        public IActionResult OnPostJoin(int activityID) //håndterer tilmeldning af aktiviteter
        {
            Activity activity = _activityService.Get(activityID); //finder aktiviteten fra id,
            if (activity != null) //hvis den eksisterer,
            {
                activity.Tilmeldt++; //tælder den "Tilmeldt" tælleren op.
                _activityService.Update(activity); //opdaterer aktivitet
            }
            return RedirectToPage(); //genindlæser siden
        }
        //----------------------------------Blog------------------------------------------------//

        private readonly BlogService _blogService; //giver adgang til blogdata



        public List<Blog> Blogs { get; set; } = new List<Blog>(); //en property som indenholder listen af blogs på siden

        [BindProperty] //properties bundet til at forme data til at læse og skrive blogs
        public Blog Blog { get; set; } = new Blog(); // Aggregation

        [BindProperty]
        public Blog NewBlog { get; set; } = new Blog(); // Tilføj ny aktivitet

        
        private readonly string _blogFilePathJson; //den fulde sti til Json filen for blogs

        public List<Blog> BlogGrid { get; set; } = new List<Blog>(); //brugt til at vise alle blogs på indexsiden

        public IActionResult OnPost() //håndterer generale blog opdateringer
        {
            // Gemmer ny blog i service (hvis den gør noget)
            _blogService.Add(NewBlog);

            // Læs eksisterende blogs fra json fil
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
            string updatedJson = JsonSerializer.Serialize(blogs, new JsonSerializerOptions { WriteIndented = true }); //udskriver den opdaterede blogliste og udskriver det i Json filen
            System.IO.File.WriteAllText(_blogFilePathJson, updatedJson);

            return RedirectToPage(); // Genindlæs siden
        }
        public IActionResult OnPostDeleteBlog(int blogID) //sletter en blog ved brug af service
        {
            _blogService.Delete(blogID);
            return RedirectToPage(); // Genindlæs siden
        }
        public IActionResult OnPostDeleteActivity(int activityID) //sletter en aktivitet ved brug af service
        {
            
            _activityService.Delete(activityID);
            return RedirectToPage(); // Genindlæs siden
        }


    }
}

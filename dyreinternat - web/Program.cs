using dyreinternat___library.Services;
using dyreinternat___library.Repository;
namespace dyreinternat___web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Dependency injection
            builder.Services.AddSingleton<IAnimalRepo, AnimalJsonRepo>();
            builder.Services.AddSingleton<AnimalService>();

            builder.Services.AddSingleton<IActivityRepo, ActivityJsonRepo>(); // repo til aktiviteter
            builder.Services.AddSingleton<ActivityService>(); // service til aktiviteter (scoped er korrekt)

            builder.Services.AddSingleton<IBookingRepo, BookingJsonRepo>();// repo til bookinger
            builder.Services.AddSingleton<BookingService>(); // service til bookinger

            builder.Services.AddSingleton<IBlogRepo, BlogJsonRepo>();// repo til blog
            builder.Services.AddSingleton<BlogService>(); // service til blog

            builder.Services.AddSingleton<IDocJournalRepo, DocJournalJsonRepo>();// repo til DocJournal
            builder.Services.AddSingleton<DocJournalService>(); // service til DocJournal

            builder.Services.AddSingleton<IMemberRepo, MemberJsonRepo>();// repo til Member
            builder.Services.AddSingleton<MemberService>(); // service til Member

            // Add Razor Pages
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();
            app.Run();
        }
    }
}

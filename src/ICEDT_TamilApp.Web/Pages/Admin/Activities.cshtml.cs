using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ICEDT_TamilApp.Web.Pages.Admin
{
    public class ActivitiesModel : PageModel
    {
        // Property to hold the lessonId from the query string
        public int LessonId { get; set; }

        public void OnGet()
        {
            // TryParse is safer than direct casting
            if (int.TryParse(Request.Query["lessonId"], out int lessonId))
            {
                LessonId = lessonId;
            }
            // You can add logic here to handle cases where lessonId is missing or invalid
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdsServer.Pages
{
    public class ArticlesModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public List<string> ArticleNumbers { get; set; }



        public IActionResult OnGet()
        {
            return Page();
        }
    }
}

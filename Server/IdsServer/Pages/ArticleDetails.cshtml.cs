using IdsServer.Database.Models;
using IdsServer.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdsServer.Pages
{
    public class ArticleDetailsModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleDetailsModel"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public ArticleDetailsModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public string ArticleNumber { get; set; }

        public FakeArticle Article { get; private set; }

        public IActionResult OnGet()
        {
            Article = _dbContext.Articles.FirstOrDefault(a => a.ArticleNumber == ArticleNumber);
            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}

using IdsServer.Database;
using IdsServer.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdsServer.Pages
{
    public class BasketDetailsModel : PageModel
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketDetailsModel"/> class.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public BasketDetailsModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public Basket Basket { get; private set; }

        public IActionResult OnGet()
        {
            Basket = _dbContext.Baskets.FirstOrDefault(a => a.Id == Id);
            if (Basket == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}

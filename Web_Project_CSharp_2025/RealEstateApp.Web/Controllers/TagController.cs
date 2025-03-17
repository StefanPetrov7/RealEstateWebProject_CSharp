using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RealEstateApp.Data;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Web.ViewModels.Tag;

namespace RealEstateApp.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ITagService tagService;

        public TagController(ApplicationDbContext dbContext, ITagService tagService)
        {
            this.dbContext = dbContext;
            this.tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<TagView> tagViewModels = await this.dbContext.Tags
                .Select(x => new TagView()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Importance = x.Importance,
                })
                .ToArrayAsync();

            return this.View(tagViewModels);
        }

        // HttpGet >> return the view with the form.
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddTagFormInputModel tagFormModel)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.View(tagFormModel);
            }

            await this.tagService.AddTag(tagFormModel.Name, tagFormModel.Importance);
            return this.RedirectToAction(nameof(Index));

        }
    }
}

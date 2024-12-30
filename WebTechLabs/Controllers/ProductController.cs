using Microsoft.AspNetCore.Mvc;
using WebTechLabs.DAL.Data;
using WebTechLabs.DAL.Entities;
using WebTechLabs.Models;

namespace WebTechLabs.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext _context;

        int _pageSize;

        public ProductController(ApplicationDbContext context)
        {
            _pageSize = 3;
            _context = context;
        }

        [Route("Catalog")]
        [Route("Catalog/Page_{pageNo}")]
        public IActionResult Index(int? group, int pageNo = 1)
        {
            var dishesFiltered = _context.Dishes
                .Where(d => !group.HasValue || d.DishGroupId == group.Value);

            ViewData["Groups"] = _context.DishGroups;
            ViewData["CurrentGroup"] = group ?? 0;

            var model = ListViewModel<Dish>.GetModel(dishesFiltered, pageNo,
            _pageSize);
            if (Request.Headers.XRequestedWith.ToString().ToLower().Equals("xmlhttprequest"))
                return PartialView("_listpartial", model);
            else
                return View(model);
        }
    }
}

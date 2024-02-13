using Microsoft.AspNetCore.Mvc;
using MvcNetCoreLinqToSql.Models;
using MvcNetCoreLinqToSql.Repositories;

namespace MvcNetCoreLinqToSql.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;

        public EnfermosController()
        {
            this.repo = new RepositoryEnfermos();
        }

        public IActionResult Index()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            return View(enfermos);
        }

        public IActionResult Details(int id)
        {
            Enfermo enfermo = this.repo.FindEnfermo(id);
            return View(enfermo);
        }

        public IActionResult Delete(int id)
        {
            this.repo.DeleteEnfermo(id);
            return RedirectToAction("Index");
        }
    }
}

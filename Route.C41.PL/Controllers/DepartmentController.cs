using Microsoft.AspNetCore.Mvc;
using Route.C41.BLL.Interfaces;
using Route.C41.BLL.Repositories;

namespace Route.C41.PL.Controllers
{
    // Inhertiance : DepartmentController is a Controller
    // Composition : DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentsRepo;

        public DepartmentController(IDepartmentRepository departmentsRepo) // Ask CLR For Creating an object from class implemnting  IDepartmentRepository
        {
            _departmentsRepo = departmentsRepo;
        }

        // /Dpartment/Index
        public IActionResult Index()
        {
            var departments = _departmentsRepo.GetAll();
            return View(departments);
        }
    }
}

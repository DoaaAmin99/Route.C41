using Microsoft.AspNetCore.Mvc;
using Route.C41.BLL.Interfaces;
using Route.C41.BLL.Repositories;
using Route.C41.DAL.Models;

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

        // /Dpartment/Create
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var count = _departmentsRepo.Add(department);

                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // /Department/Details/10
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var department = _departmentsRepo.Get(id.Value);
            
            if (department is null)
                return NotFound(); // 404

            return View(department);

        }


    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Route.C41.BLL.Interfaces;
using Route.C41.BLL.Repositories;
using Route.C41.DAL.Models;
using System;

namespace Route.C41.PL.Controllers
{
    // Inhertiance : DepartmentController is a Controller
    // Composition : DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentsRepo;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository departmentsRepo , IWebHostEnvironment env) // Ask CLR For Creating an object from class implemnting  IDepartmentRepository
        {
            _departmentsRepo = departmentsRepo;
            _env = env;
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
        //[HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); // 400

            var department = _departmentsRepo.Get(id.Value);
            
            if (department is null)
                return NotFound(); // 404

            return View(viewName,department);

        }

        // /Department/Edit/10
        //[HttpGet]
        public IActionResult Edit(int? id)
        {
            ///if(!id.HasValue)
            ///    return BadRequest();
            ///var deparment = _departmentsRepo.Get(id.Value);
            ///if(deparment is null)
            ///    return NotFound();
            ///return View(deparment);
            
            return Details(id , "Edit");

        }

        // /Department/Edit/10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Department department)
        {
            if(id != department.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(department);

            try
            {
                _departmentsRepo.Update(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exeption
                // 2. Friendly Message

                if(_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occured during Updating the Department");

                return View(department);
            } 
            
        }

        // /Department/Delete/10
        //[HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id , "Delete");
        }

        // /Department/Delete/10
        [HttpPost]
        public IActionResult Delete(Department department)
        {

            try
            {
                _departmentsRepo.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exeption
                // 2. Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error Occured during Deleting this Department");

                return View(department);
            }
        }


    }
}

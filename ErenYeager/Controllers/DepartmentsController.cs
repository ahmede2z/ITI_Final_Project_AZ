using ErenYeager.Data;
using ErenYeager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErenYeager.Controllers {
    public class DepartmentsController : Controller {
        ApplicationDbContext context;
        public DepartmentsController(ApplicationDbContext cont) {
            context = cont;
        }

        public IActionResult GetIndexView() {
            return View("Index", context.Departments);
        }

        public IActionResult GetCreateView() {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//prevents CSRF=>cross-site request forgary
        // bind prevent overposting attacks
        // bn3ml 
        public IActionResult AddDepartment([Bind("Id,Name,Description")] Department dep) {
            if (ModelState.IsValid) {
                context.Departments.Add(dep);
                context.SaveChanges();
                return RedirectToAction("GetIndexView");
            } else {
                return View("Create", dep);
            }

        }

        public IActionResult GetDetailsView(int id) {
            Department department = context.Departments.Include(d => d.Teachers).FirstOrDefault(e => e.Id == id);
            if (department == null) { return NotFound(); }
            return View("Details", department);
        }

        [HttpGet]
        public IActionResult GetDeleteDepartment(int id) {
            Department department = context.Departments.FirstOrDefault(e => e.Id == id);
            if (department == null) { return NotFound(); }
            return View("Delete", department);
        }


        [HttpPost]
        public IActionResult DeleteDepartment(int id) {
            Department department = context.Departments.FirstOrDefault(e => e.Id == id);
            context.Remove(department);
            context.SaveChanges();
            return RedirectToAction("GetIndexView");
        }

        [HttpGet]
        public IActionResult GetModifyView(int id) {
            Department department = context.Departments.FirstOrDefault(e => e.Id == id);
            if (department == null) { return NotFound(); }
            return View("Modify", department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifyDepartment([Bind("Id,Name,Description")] Department dep, int id) {
            if (id != dep.Id) {
                return BadRequest();
            }
            if (ModelState.IsValid) {
                context.Departments.Update(dep);
                context.SaveChanges();
                return RedirectToAction("GetIndexView");
            } else {
                return View("Modify", dep);
            }
        }

    }
}

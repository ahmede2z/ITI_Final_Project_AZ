using ErenYeager.Data;
using ErenYeager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ErenYeager.Controllers {
    public class TeachersController : Controller {
        ApplicationDbContext context;
        IWebHostEnvironment webHostEnvironment;
        public TeachersController(ApplicationDbContext cont, IWebHostEnvironment webHostEnv) {
            context = cont;
            webHostEnvironment = webHostEnv;
        }

        public IActionResult GetIndexView(string? search, string? sortType, string? sortOrder, int pageSize = 10, int pageNumber = 1) {

            List<Teacher> teachers = new List<Teacher>();
            if (string.IsNullOrEmpty(search)) {
                teachers = context.Teachers.ToList();
            } else {
                ViewBag.CurrentSearch = search;
                teachers = context.Teachers.Where(tec => tec.FullName.Contains(search)).ToList();
            }
            if (sortType != null && sortOrder != null) {
                if (sortType == "FullName") {
                    if (sortOrder == "asc")
                        teachers = teachers.OrderBy(tec => tec.FullName).ToList();
                    else
                        teachers = teachers.OrderByDescending(tec => tec.FullName).ToList();
                } else if (sortType == "Degree") {
                    if (sortOrder == "asc")
                        teachers = teachers.OrderBy(tec => tec.Degree).ToList();
                    else
                        teachers = teachers.OrderByDescending(tec => tec.Degree).ToList();
                } else if (sortType == "Salary") {
                    if (sortOrder == "asc")
                        teachers = teachers.OrderBy(tec => tec.Salary).ToList();
                    else
                        teachers = teachers.OrderByDescending(tec => tec.Salary).ToList();
                }

            }
            ViewBag.CurrentpageSize = pageSize;
            ViewBag.CurrentpageNumber = pageNumber;
            teachers = teachers.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            return View("index", teachers);
        }

        public IActionResult GetCreateView() {
            ViewBag.AllDepartments = context.Departments.ToList();
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTeacher([Bind("Id,FullName,Degree,Salary,DepartmentID")]
         Teacher tec, IFormFile? imageFile) {
            if (ModelState.IsValid) {
                if (imageFile != null) {
                    string imgExtension = Path.GetExtension(imageFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtension;
                    string imgUrl = "\\assests\\images\\" + imgName;
                    tec.ImageUrl = imgUrl;
                    string imgPath = webHostEnvironment.WebRootPath + imgUrl;
                    FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();
                } else {
                    tec.ImageUrl = "\\assests\\images\\No_img.jpeg";
                }
                _ViewModel _ViewModel = new _ViewModel() {
                    Departments = context.Departments.ToList(),
                    Teachers = context.Teachers.ToList(),
                    Teacher = tec
                };
                context.Teachers.Add(tec);
                context.SaveChanges();
                return RedirectToAction("GetIndexView");
            } else {
                ViewBag.AllDepartments = context.Departments.ToList();

                return View("Create", tec);
            }

        }

        public IActionResult GetDetailsView(int id) {
            Teacher teacher = context.Teachers.Include(tec => tec.Department)
                .FirstOrDefault(tec => tec.Id == id);
            if (teacher == null) { return NotFound(); }
            return View("Details", teacher);
        }

        [HttpGet]
        public IActionResult GetDeleteTeacher(int id) {
            Teacher teacher = context.Teachers.Include(tec => tec.Department)
                .FirstOrDefault(tec => tec.Id == id);
            if (teacher == null) { return NotFound(); }
            return View("Delete", teacher);
        }


        [HttpPost]
        public IActionResult DeleteTeacher(int id) {
            Teacher teacher = context.Teachers.FirstOrDefault(tec => tec.Id == id);
            if (teacher.ImageUrl != "\\assests\\images\\No_img.jpeg") {
                string imgPath = webHostEnvironment.WebRootPath + teacher.ImageUrl;
                if (System.IO.File.Exists(imgPath)) {
                    System.IO.File.Delete(imgPath);
                }
            }
            context.Remove(teacher);
            context.SaveChanges();
            return RedirectToAction("GetIndexView");
        }

        [HttpGet]
        public IActionResult GetModifyTeacher(int id) {
            Teacher teacher = context.Teachers.FirstOrDefault(tec => tec.Id == id);
            if (teacher == null) { return NotFound(); }
            ViewBag.AllDepartments = context.Departments.ToList();

            return View("Modify", teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModifyTeacher([Bind("Id,FullName,Degree,Salary,DepartmentID,ImageUrl")]
         Teacher tec, IFormFile? imageFile, int id) {
            if (tec.Id != id) {
                return BadRequest();
            }

            if (ModelState.IsValid) {
                if (imageFile != null) {
                    if (tec.ImageUrl != "\\assests\\images\\No_img.jpeg") {
                        string oldImgPath = webHostEnvironment.WebRootPath + tec.ImageUrl;
                        if (System.IO.File.Exists(oldImgPath)) {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    string imgExtension = Path.GetExtension(imageFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtension;
                    string imgUrl = "\\assests\\images\\" + imgName;
                    tec.ImageUrl = imgUrl;
                    string imgPath = webHostEnvironment.WebRootPath + imgUrl;
                    FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();
                }

                context.Teachers.Update(tec);
                context.SaveChanges();
                return RedirectToAction("GetIndexView");
            } else {
                ViewBag.AllDepartments = context.Departments.ToList();
                return View("Modify", tec);
            }

        }
    }
}

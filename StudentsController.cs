using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _db;
        private const int PageSize = 10;

        public StudentsController(SchoolDbContext db)
        {
            _db = db;
        }

        // GET: /Students?page=1
        public async Task<IActionResult> Index(int page = 1)
        {
            var total = await _db.Students.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)PageSize);
            page = Math.Clamp(page, 1, Math.Max(1, totalPages));

            var students = await _db.Students
                .Include(s => s.School)
                .OrderBy(s => s.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = total;

            return View(students);
        }

        // GET: /Students/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _db.Students.Include(s => s.School).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // GET: /Students/Create
        public IActionResult Create()
        {
            PopulateSchoolsDropDown();
            return View();
        }

        // POST: /Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            PopulateSchoolsDropDown(student.SchoolId);
            if (!ModelState.IsValid) return View(student);

            // Validation: uniqueness checks
            if (await _db.Students.AnyAsync(s => s.StudentId == student.StudentId))
            {
                ModelState.AddModelError(nameof(student.StudentId), "Student ID already exists. Please enter a unique Student ID.");
                return View(student);
            }
            if (await _db.Students.AnyAsync(s => s.Email == student.Email))
            {
                ModelState.AddModelError(nameof(student.Email), "Email already exists. Please use a different email.");
                return View(student);
            }

            // Phone digits validation if present
            if (!string.IsNullOrWhiteSpace(student.Phone))
            {
                var digits = new string(student.Phone.Where(char.IsDigit).ToArray());
                if (digits.Length < 10 || digits.Length > 11)
                {
                    ModelState.AddModelError(nameof(student.Phone), "Phone number must contain 10 to 11 digits.");
                    return View(student);
                }
            }

            // School exists check
            if (!await _db.Schools.AnyAsync(s => s.Id == student.SchoolId))
            {
                ModelState.AddModelError(nameof(student.SchoolId), "Selected school does not exist.");
                return View(student);
            }

            student.CreatedAt = DateTime.UtcNow;
            student.UpdatedAt = DateTime.UtcNow;

            _db.Students.Add(student);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Students/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();
            PopulateSchoolsDropDown(student.SchoolId);
            return View(student);
        }

        // POST: /Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student input)
        {
            if (id != input.Id) return BadRequest();
            PopulateSchoolsDropDown(input.SchoolId);

            if (!ModelState.IsValid) return View(input);

            // Uniqueness: email (ignore current)
            var conflict = await _db.Students.FirstOrDefaultAsync(s => s.Email == input.Email && s.Id != id);
            if (conflict != null)
            {
                ModelState.AddModelError(nameof(input.Email), "Email already exists. Please use a different email.");
                return View(input);
            }

            if (!await _db.Schools.AnyAsync(s => s.Id == input.SchoolId))
            {
                ModelState.AddModelError(nameof(input.SchoolId), "Selected school does not exist.");
                return View(input);
            }

            if (!string.IsNullOrWhiteSpace(input.Phone))
            {
                var digits = new string(input.Phone.Where(char.IsDigit).ToArray());
                if (digits.Length < 10 || digits.Length > 11)
                {
                    ModelState.AddModelError(nameof(input.Phone), "Phone number must contain 10 to 11 digits.");
                    return View(input);
                }
            }

            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.FullName = input.FullName;
            // StudentId not editable by requirement; keep original
            student.Email = input.Email;
            student.Phone = input.Phone;
            student.SchoolId = input.SchoolId;
            student.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Students/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _db.Students.Include(s => s.School).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private void PopulateSchoolsDropDown(int? selectedId = null)
        {
            var schools = _db.Schools.OrderBy(s => s.Name).ToList();
            ViewBag.Schools = new SelectList(schools, "Id", "Name", selectedId);
        }
    }
}
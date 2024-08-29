using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechTest_KawanLama.Data;
using TechTest_KawanLama.Models;
using TechTest_KawanLama.ViewModels;

namespace TechTest_KawanLama.Controllers
{
    public class ToDoesController : Controller
    {
        private readonly DBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToDoesController(DBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("id");
            if (userId == null)
                return RedirectToAction("Login", "Users");

            return _context.ToDos != null ? 
                          View(await _context.ToDos.Where(x => x.UserId == userId.Value).ToListAsync()) :
                          Problem("Entity set 'DBContext.ToDos'  is null.");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoViewModel toDo)
        {
            string username = _httpContextAccessor.HttpContext.Session.GetString("username");
            int? userId = _httpContextAccessor.HttpContext.Session.GetInt32("id");
            if(!string.IsNullOrEmpty(username))
            {
                try
                {
                    foreach (var item in toDo.TodoViewModel)
                    {
                        var seqNum = await _context.SequenceNumbers.FirstOrDefaultAsync(x => x.SequenceName == "ToDo");
                        seqNum.LastRunNo += 1;
                        var lastId = "0000" + (seqNum.LastRunNo).ToString();
                        var actNo = seqNum.Format[..3] + lastId[^4..];

                        var todoData = new ToDo
                        {
                            activities_no = actNo,
                            Subject = item.Subject,
                            Description = item.Description,
                            Status = "Unmarked",
                            UserId = userId.Value
                        };

                        await _context.ToDos.AddAsync(todoData);

                        _context.SequenceNumbers.Update(seqNum);

                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return RedirectToAction("Index","ToDoes");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            return View(toDo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string activities_no, [Bind("activities_no,Subject,Description,Status")] ToDo toDo)
        {
            if (activities_no != toDo.activities_no)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(toDo.activities_no))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDo);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDos
                .FirstOrDefaultAsync(m => m.activities_no == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string activities_no)
        {
            if (_context.ToDos == null)
            {
                return Problem("Entity set 'DBContext.ToDos'  is null.");
            }
            var toDo = await _context.ToDos.FindAsync(activities_no);
            if (toDo != null)
            {
                _context.ToDos.Remove(toDo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoExists(string id)
        {
          return (_context.ToDos?.Any(e => e.activities_no == id)).GetValueOrDefault();
        }
    }
}

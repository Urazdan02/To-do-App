using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;
using WebMatrix.WebData;

namespace ToDoApp.Controllers
{

    [Authorize]
    public class TaskModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskModels
        public async Task<IActionResult> Index()
        {
            var userID = await _context.Users
                .Where(m => m.Email.Equals(User.Identity.Name))
                .Select(m => m.Id)
                .SingleOrDefaultAsync();
            var userTasks = await _context.TaskModel
                               .Where(t => userID.Equals(t.UserID)).ToListAsync();

            return View(userTasks);
        }

        // GET: TaskModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var taskModel = await _context.TaskModel
                .FirstOrDefaultAsync(m => m.ID == id);

            var userID = await _context.Users
                 .Where(m => m.Email.Equals(User.Identity.Name))
                 .Select(m => m.Id)
                 .SingleOrDefaultAsync();

            if (taskModel == null || taskModel.UserID != userID)
            {
                return View("NotFound");
            }

            return View(taskModel);
        }

        // GET: TaskModels/Create
        public IActionResult Create()
        {
            var dateNow = DateTime.Now.ToLocalTime();
            dateNow = dateNow.AddMilliseconds(-dateNow.Millisecond);

            TaskModel taskModel = new TaskModel
            {
                Insert_Date = dateNow,
                Task_Date = dateNow
            };

            return View(taskModel);
        }

        // POST: TaskModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Subject,Content,Insert_Date,Task_Date,Location,Image,Completed,UserID")] TaskModel taskModel, IFormFile Image)
        {
            var userID = await _context.Users
                           .Where(m => m.Email.Equals(User.Identity.Name))
                           .Select(m => m.Id)
                           .SingleOrDefaultAsync();

            taskModel.Completed = false;
            taskModel.UserID = userID;

            if (ModelState.IsValid)
            {
                if (Image != null)

                {
                    if (Image.Length > 0)

                    //Convert Image to byte and save to database

                    {

                        byte[] p1 = null;
                        using (var fs1 = Image.OpenReadStream())
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                        taskModel.Image = p1;

                    }
                }

                _context.Add(taskModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskModel);
        }

        // GET: TaskModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.TaskModel.FindAsync(id);
            var userID = await _context.Users
             .Where(m => m.Email.Equals(User.Identity.Name))
             .Select(m => m.Id)
             .SingleOrDefaultAsync();

            if (taskModel == null || taskModel.UserID != userID)
            {
                return NotFound();
            }
            return View(taskModel);
        }

        // POST: TaskModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Subject,Content,Insert_Date,Task_Date,Location,Completed,UserID")] TaskModel taskModel)
        {          
            if (id != taskModel.ID)
            {
                return NotFound();
            }

            var userID = await _context.Users
             .Where(m => m.Email.Equals(User.Identity.Name))
             .Select(m => m.Id)
             .SingleOrDefaultAsync();

            if (taskModel.UserID != userID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskModelExists(taskModel.ID))
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
            return View(taskModel);
        }

        // GET: TaskModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.TaskModel
                .FirstOrDefaultAsync(m => m.ID == id);
            var userID = await _context.Users
             .Where(m => m.Email.Equals(User.Identity.Name))
             .Select(m => m.Id)
             .SingleOrDefaultAsync();

            if (taskModel == null || taskModel.UserID != userID)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        // POST: TaskModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskModel = await _context.TaskModel.FindAsync(id);
            _context.TaskModel.Remove(taskModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskModelExists(int id)
        {
            return _context.TaskModel.Any(e => e.ID == id);
        }
    }
}

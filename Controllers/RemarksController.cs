using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OreoHealth.Data;
using OreoHealth.Models;

namespace OreoHealth.Controllers
{
    public class RemarksController : Controller
    {
        private readonly ClinicContext _context;

        public RemarksController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Remarks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Remarks.ToListAsync());
        }

        public async Task<IActionResult> Select(string name)
        {
            var patient = await _context.Remarks
                 .FirstOrDefaultAsync(m => m.FullName == name);
            return View(patient);
        }

        // GET: Remarks/Details/5
        public async Task<IActionResult> Details(string name)
        {

            var remarks = await _context.Remarks
                .FirstOrDefaultAsync(m => m.FullName == name);
            
                remarks.description = remarks.description.ToString().Replace("\n", "<br />");
                return View(remarks);
        }

        // GET: Remarks/Create
        public async Task<IActionResult> Create(string name)
        {
            var remark = await _context.Remarks.FirstOrDefaultAsync(m => m.FullName == name);

            //if (remark != null)
                ViewData["FullName"] = remark.FullName;

            //else
            //{
            //    ViewData["FullName"] = name;
            //} 
            return View();
        }

        // POST: Remarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, [Bind("Id,FullName,description")] Remarks remarks)
        {
            if (ModelState.IsValid)
            {
                var remark = await _context.Remarks.FirstOrDefaultAsync(m => m.FullName == remarks.FullName);

                    remark.description += remarks.description + "\n";
                   // _context.Remarks.Add(remarks);
                    await _context.SaveChangesAsync();
                return View("Select", remark);
            }
            return View();
        }

        // GET: Remarks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remarks = await _context.Remarks.FindAsync(id);
            if (remarks == null)
            {
                return NotFound();
            }
            return View(remarks);
        }

        // POST: Remarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,description")] Remarks remarks)
        {
            if (id != remarks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(remarks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RemarksExists(remarks.Id))
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
            return View(remarks);
        }

        // GET: Remarks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var remarks = await _context.Remarks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (remarks == null)
            {
                return NotFound();
            }

            return View(remarks);
        }

        // POST: Remarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var remarks = await _context.Remarks.FindAsync(id);
            _context.Remarks.Remove(remarks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RemarksExists(int id)
        {
            return _context.Remarks.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OreoHealth.Data;
using OreoHealth.Models;
using OreoHealth.ViewModel;

namespace OreoHealth.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ClinicContext _context;

        public DoctorsController(ClinicContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctors.ToListAsync());
        }

        //Once the admin(doctor) clicks the admin menu, they are redirected to the admin login page
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            int doctorId = 0;
            AdminSession[] session = _context.AdminSessions.ToArray();

            if (session.Length != 0)
            {
                System.Diagnostics.Debug.Write(session[0].SessionId);
                doctorId = session[0].DoctorId;

                var doctor = await _context.Doctors
               .FirstOrDefaultAsync(q => q.Id.Equals(doctorId));

                return View("Index", doctor);
            }
            return View();
        }

        //login post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView p)
        {

            //if admin input is valid
            if (ModelState.IsValid)
            {
                //Get the doctor details from the database
                var doctor = await _context.Doctors
                .FirstOrDefaultAsync(q => q.Email.Equals(p.Email)
                && q.Password.Equals(p.Password));

                //if doctor exists, the index page is displayed
                if (doctor != null)
                {
                    AdminSession aSession = new AdminSession();
                    aSession.SessionId = doctor.FirstName + "123";
                    aSession.DoctorId = doctor.Id;
                    aSession.LoginDate = System.DateTime.Now;

                    _context.AdminSessions.Add(aSession);

                    await _context.SaveChangesAsync();
                    return View("Index", doctor);
                }
                //if doctor does not exist, the error message is displayed
                else
                {
                    ModelState.AddModelError("Failure", "Wrong Email or Password");
                    return View();
                }
            }
            //if user input is not valid, then the validation errors are displayed on the login page
            return View();
        }

        public async Task<ActionResult> Logout(int? id)
        {
            var session = await _context.AdminSessions
                   .FirstOrDefaultAsync(q => q.DoctorId.Equals(id));

            _context.AdminSessions.Remove(session);
            await _context.SaveChangesAsync();
            return View("Login");
        }

        //On the admin index page, once the admin clicks account, the details pf the admin is displayed
        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }


        //Code below is not implemented yet
        //****************************************************************************************************************************************************

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,Address,Gender,Available,Specialization,Email,Password")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("Index", doctor);
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}

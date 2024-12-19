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
    public class AppointmentsController : Controller
    {
        private readonly ClinicContext _context;

        public AppointmentsController(ClinicContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            //var clinicContext = _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient);
            var name = await _context.Doctors.FindAsync(id);
            List<Appointment> app = new List<Appointment>();
            foreach(Appointment a in _context.Appointments)
            {
                if(a.DoctorName == name.FullName)
                {

                    app.Add(a);
                }
            }
            return View(app.ToList());
        }

        public async Task<IActionResult> AllPatient(int? id)
        {
            //var clinicContext = _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient);
            var name = await _context.Doctors.FindAsync(id);
            //var name = await _context.Patients.FindAsync(id);
            List<Appointment> app = new List<Appointment>();
            foreach (Appointment a in _context.Appointments)
            {
                if (a.DoctorName == name.FullName)
                {
                    var exist = app.FirstOrDefault(n => n.PatientName == a.PatientName);
                    if(exist == null)
                    app.Add(a);
                }
            }
           
            return View(app.ToList());
        }

        // GET: Appointments
        //public async Task<IActionResult> Index()
        //{
        //    //var clinicContext = _context.Appointments.Include(a => a.Doctor).Include(a => a.Patient);
        //    return View(await _context.Appointments.ToListAsync());
        //}

        //Once patient clicks on the link book an appointment, the create page is displayed
        // GET: Appointments/Create
        public async Task<IActionResult> Create(int? id)
        {
            var name = await _context.Patients.FindAsync(id);

            //the list of doctors is sent to the form
            ViewData["DoctorName"] = new SelectList(_context.Doctors, "Id", "FullName");

            //the patient name is also sent to the form
            ViewData["PatientName"] = name.FirstName + ", " + name.LastName;
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Appointment a)
        {
            //if patient input is valid we add the appointment details to the database
            if (ModelState.IsValid)
            {
                var name = await _context.Doctors.FindAsync(int.Parse(a.DoctorName));

                var appointment = new Appointment[]{
                        new Appointment{StartDateTime = a.StartDateTime, EndDateTime = a.EndDateTime, DoctorName = name.FullName, PatientName = a.PatientName}
                    };
                foreach(Appointment ap in appointment)
                {
                    _context.Appointments.Add(ap);
                }
                 
                    await _context.SaveChangesAsync();

                //then the page is redirected back to patients index
                    //return RedirectToAction("Index");            

            }
            //if input is invalid then error message is displayed
            ModelState.AddModelError("Failure", "Unable to book appointment");
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        //Code below is not implemented yet
        //****************************************************************************************************************************************************


        //public async Task<IActionResult> Create([Bind("Id,StartDateTime,EndDateTime,PatientName,DoctorName")] Appointment appointment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(appointment);
        //        await _context.SaveChangesAsync();
        //        ViewData["success"] = "Appointment Booked";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DoctorName"] = new SelectList(_context.Doctors, "Id", "FirstName", appointment.DoctorName);
        //    ViewData["PatientName"] = appointment.PatientName;
        //    return View(appointment);
        //}

        // GET: Appointments/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var appointment = await _context.Appointments
        //        .Include(a => a.Doctor)
        //        .Include(a => a.Patient)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (appointment == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(appointment);
        //}

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["DoctorName"] = new SelectList(_context.Doctors, "Id", "Id", appointment.DoctorName);
            ViewData["PatientName"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientName);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDateTime,EndDateTime,PatientId,DoctorId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", appointment.DoctorName);
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", appointment.PatientName);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}

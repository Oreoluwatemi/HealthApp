using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OreoHealth.Data;
using OreoHealth.Models;
using OreoHealth.ViewModel;

namespace OreoHealth.Controllers
{
    public class PatientsController : Controller
    {
        private SignInManager<Patient> _signManager;
        private UserManager<Patient> _userManager;
        private ClinicContext _context;

        public PatientsController(ClinicContext context)
        {
            _context = context;
        }

        
        // GET: Patients
        public async Task<IActionResult> Index(int? id)
        {
            return View(_context.Patients.Find(id));
        }


        //Once user clicks login on the menu, the login page is displayed
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            int patientId = 0;
            Session[] session = _context.Sessions.ToArray();
            
            if (session.Length != 0)
            {
                System.Diagnostics.Debug.Write(session[0].SessionId);
                patientId = session[0].PatientId;

                    var patient = await _context.Patients
                   .FirstOrDefaultAsync(q => q.Id.Equals(patientId));

                    return View("Index", patient);
            }
            
            return View();
        }

        public async Task<ActionResult> Logout(int? id)
        {
            var session = await _context.Sessions
                   .FirstOrDefaultAsync(q => q.PatientId.Equals(id));

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return View("Login");
        }

        //using post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginView p)
        {
            //if user input is valid
                if (ModelState.IsValid) {

                //Get the patient details from the database
                var patient = await _context.Patients
                .FirstOrDefaultAsync(q => q.Email.Equals(p.Email)
                && q.Password.Equals(p.Password));

                //if patient exists, the index page is displayed
                if (patient != null)
                {
                    Session session = new Session();
                    session.SessionId = patient.FirstName + "123";
                    session.PatientId = patient.Id;
                    session.LoginDate = System.DateTime.Now;

                    _context.Sessions.Add(session);

                    await _context.SaveChangesAsync();

                    return View("Index",patient);
                }
                //if patient does not exist, the error message is displayed
                else
                {
                    ModelState.AddModelError("Failure", "Wrong Email or Password");
                    return View();
                }
            }
            //if user input is not valid, then the validation errors are displayed on the login page
            return View();
        }

        //Once user(patient) clicks the signup menu, the register page is displayed
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //using post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistrationView r)
        {
            //if user input is valid
            if (ModelState.IsValid)
            {
                //We first check if user exists in the database
                var doesUserExist = _context.Patients.FirstOrDefault(p => p.Email == r.Email);

                //Also check if password and confirm passwrord are the same
                if(r.Password != r.ConfirmPassword)
                {
                    ModelState.AddModelError("Failure", "Password and Confrim Password don't match");
                    return View();
                }

                //If user does not exist then we add user to the database
                if(doesUserExist == null)
                {
                    var patient = new Patient[]{
                        new Patient{FirstName = r.FirstName, LastName = r.LastName, PhoneNumber = r.PhoneNumber, Address= r.Address,
                        Gender= r.Gender, Email=r.Email, Password = r.Password, BirthDate = r.BirthDate}
                    };
                    var remark = new Remarks[]
                    {
                        new Remarks{FullName = r.FirstName + ", " + r.LastName, description = ""}
                    };
                    foreach (Patient p in patient)
                    {
                        _context.Patients.Add(p);
                    }
                    foreach (Remarks re in remark)
                    {
                        _context.Remarks.Add(re);
                    }
                    
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction("Login", "Patients");

                }
                else
                {
                    //if user input is not valid then we send that user already exists
                    ModelState.AddModelError("Failure", "User Already exists");
                    return View();
                }               
            }
            return View();
        }


        //Once user clicks the link Account in the index page the patient details are displayed
            // GET: Patients/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("Details", patient);
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }


        //Code below is not implemented yet
        //****************************************************************************************************************************************************




        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,Address,Gender,Email,Password,BirthDate")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

    }
}

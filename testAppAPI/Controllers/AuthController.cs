using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using testAppAPI.Modals;
using System.Threading.Tasks;
using testAppAPI.Models;

namespace testAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly testAppAPIDBContext _context;

        public AuthController(testAppAPIDBContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(Login cust)
        {
            var customer = _context.Customers.Where(c => c.Email == cust.Email
                            && c.Password == cust.Password && c.IsAdmin == cust.IsAdmin).FirstOrDefault();

            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound();
        }

        [HttpPost("signup")]
        public IActionResult Signup(Customer cust)
        {
            _context.Customers.Add(cust);
            _context.SaveChanges();
            return Ok(cust);
        }
    }
}

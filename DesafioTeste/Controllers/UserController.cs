using DesafioTeste.Data;
using DesafioTeste.Models;
using DesafioTeste.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DesafioTeste.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly RandomUserService _randomUserService;

        public UserController(AppDbContext context, RandomUserService randomUserService)
        {
            _context = context;
            _randomUserService = randomUserService;
        }

        // Página inicial usando User.cshtml
        public async Task<IActionResult> User()
        {
            var users = await _context.Users.ToListAsync();
            return View("~/Views/User.cshtml", users);
        }
        [HttpPost] 
        public async Task<IActionResult> Generate()
        {
            var user = await _randomUserService.GetRandomUserAsync();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("User"); 
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View("~/Views/Edit.cshtml", user);
        }
        // Ação para obter os dados do usuário
        [HttpGet]
        public IActionResult GetUserDetails(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                gender = user.Gender,
                avatar = user.Avatar
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                var users = await _context.Users.ToListAsync();
                return View("~/Views/User.cshtml", users);
            }
            return View(user);  
        }
    }
}

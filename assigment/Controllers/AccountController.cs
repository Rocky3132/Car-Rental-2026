
using assigment.Servicess;
using Microsoft.AspNetCore.Mvc;
namespace assigment.Controllers;
public class AccountController : Controller
{
    private readonly UserService _userService = new UserService();

    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        if (_userService.RegisterUser(username, password))
            return RedirectToAction("Login");

        return View("Error");
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (_userService.Login(username, password))
            return Content("Welcome!"); 
        
        return Unauthorized("Invalid credentials");
    }
}

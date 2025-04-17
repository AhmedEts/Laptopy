using Laptopy.DTOs.Request;
using Laptopy.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] DTOs.Request.RegisterRequest registerRequest)
        {
            ApplicationUser applicationUser = registerRequest.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(applicationUser, registerRequest.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(applicationUser, false);
                if (_roleManager.Roles.IsNullOrEmpty())
                {
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Company"));
                    await _roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                await _userManager.AddToRoleAsync(applicationUser, "Customer");

                return Created();
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]DTOs.Request.LoginRequest loginRequest)
        {
            var appUser=await _userManager.FindByEmailAsync(loginRequest.Email);
            if (appUser!=null)
            {
              var result= await _userManager.CheckPasswordAsync(appUser,loginRequest.Password);
                if (result)
                {
                    await _signInManager.SignInAsync(appUser, loginRequest.RememberMe);
                    return NoContent();
                }
                else
                {
                    ModelStateDictionary keyValuePairs = new();
                    keyValuePairs.AddModelError("Error", "Invalid Data");
                    return BadRequest(keyValuePairs);
                }
            }
            else
            {
                return NotFound();
            }
             
        }


        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {

           await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}

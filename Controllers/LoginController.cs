using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using protecta.laft.api.DTO;
using protecta.laft.api.Services;

namespace protecta.laft.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private LoginService LoginService;
        public LoginController()
        {
            this.LoginService = new LoginService();
        }
        // GET api/historia
        [HttpPost]
        public ActionResult<userResponseDTO> ValidateUser(LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                var modelUser = this.LoginService.ValExistUser(login.username.ToUpper(), login.password);
                return modelUser;
            }
            return BadRequest(ModelState);
        }

        [Route("getcaptcha")]
        [HttpGet]
        public IActionResult Captcha()
        {
            Dictionary<string, string> item = new Dictionary<string, string>();
            const string _Chars = "abcdfghijklmnopqrstuvwxyz0123456789";
            Random _Random = new Random();
            string _Captcha = new string(Enumerable.Repeat(_Chars, 5).Select(s => s[_Random.Next(s.Length)]).ToArray());
            item.Add("captcha", _Captcha);
            return Ok(item);
        }
    }
}
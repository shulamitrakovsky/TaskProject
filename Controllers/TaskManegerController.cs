using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TaskProject.Models;
using TaskProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using TaskProject.Services;

namespace  TaskProject.Controllers{
     [ApiController]
    [Route("[controller]")]
        // [Authorize(Policy = "Manager")]

    public class  TaskManegerController:ControllerBase
    {
        private List<User> users;
        private IUserService userService;
        public TaskManegerController(IUserService userService){
            this.userService=userService;          
        }
    [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User User)
        {
            System.Console.WriteLine("kkk");
            var dt = DateTime.Now;

             var user = this.userService.GetAll().FirstOrDefault(c =>
            c.Username == User.Username && c.Password == User.Password);


            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.TaskManager ? "Manager" : "Agent"),
                new Claim("userId", user.UserId.ToString())

            };
            if(user.TaskManager)
                claims.Add(new Claim("UserType", "Agent"));

            var token = TaskTokevServices.GetToken(claims);
            return new OkObjectResult(TaskTokevServices.WriteToken(token));
        }
    }
}
    


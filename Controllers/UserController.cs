using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskProject.Models;
using TaskProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TaskProject.Controllers{
  using TaskProject.Models;
 using TaskProject.Interfaces;

    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "Manager")]
    public class UserController : ControllerBase
    {
       private readonly long userId;
        private IUserService userService ;
        public UserController(IUserService userService,IHttpContextAccessor httpContextAccessor)
        {
                        this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);

            this.userService = userService;
           
            

        }
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<User>> GetAll() =>
            userService.GetAll();


        [HttpGet]
        [Route("Get")]
        [Authorize(Policy = "Agent")]
        public ActionResult<User> Get()
        {
            var task = userService.Get(userId);

            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost] 
        public IActionResult Create(User  task)
        {
            userService.Add(task);
            return CreatedAtAction(nameof(Create), new {id=task.UserId}, task);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User task)
        {
            if (id != task.UserId)
                return BadRequest();

            var existingTask = userService.Get(userId);
            if (existingTask is null)
                return  NotFound();

            userService.Update(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = userService.Get(userId);
            if (task is null)
                return  NotFound();

            userService.Delete(userId,id);

            return Content(userService.Count().ToString());
        }
    }
}


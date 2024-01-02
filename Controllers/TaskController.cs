using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskProject.Models;
using TaskProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TaskProject.Controllers
{
    using TaskProject.Models;
    using TaskProject.Interfaces;

    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "Agent")]
    public class TaskController : ControllerBase
    {
        private readonly long userId;

        private ITaskService TaskService;
        public TaskController(ITaskService TaskServices,IHttpContextAccessor httpContextAccessor)
        {
            this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);

            this.TaskService = TaskServices;



        }
        [HttpGet]
        public ActionResult<List<Task>> GetAll() =>
            TaskService.GetAll(userId);


        [HttpGet("{id}")]
        public ActionResult<Task> Get(int id)
        {
            var task = TaskService.Get(userId,id);

            if (task == null)
                return NotFound();

            return task;
        }

        [HttpPost]
        public IActionResult Create(Task task)
        {
            TaskService.Add(userId,task);
            return CreatedAtAction(nameof(Create), new { id = task.Id }, task);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest();

            var existingTask = TaskService.Get(userId,id);
            if (existingTask is null)
                return NotFound();

            TaskService.Update(userId,task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = TaskService.Get(userId,id);
            if (task is null)
                return NotFound();

            TaskService.Delete(userId,id);

            return Content(TaskService.Count().ToString());
        }
    }
}


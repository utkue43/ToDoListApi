using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private static List<Table.Item> tasks = new List<Table.Item>();
        private static int taskIdCounter = 1;

        [Authorize]
        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(tasks);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetTask(string id)
        {
            var task = tasks.FirstOrDefault(t => t.TaskId == id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostTask([FromBody] Table.Item task)
        {
            if (task == null)
                return BadRequest("Invalid task data.");

            task.TaskId = taskIdCounter.ToString();
            tasks.Add(task);
            taskIdCounter++;

            return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, task);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutTask(string id, [FromBody] Table.Item updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.TaskId == id);
            if (task == null)
                return NotFound();

            
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.TaskStatus = updatedTask.TaskStatus;
            task.TaskCategory = updatedTask.TaskCategory;
            task.CreatedBy = updatedTask.CreatedBy;

            return Ok(task);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(string id)
        {
            var task = tasks.FirstOrDefault(t => t.TaskId == id);
            if (task == null)
                return NotFound();

            tasks.Remove(task);

            return Ok(task);
        }
    }
}

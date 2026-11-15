using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DTO;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService_cs _taskService;

        public TasksController(ITaskService_cs taskService)
        {
            _taskService = taskService;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasks();
            return Ok(tasks);
        }

        // GET: api/Task/{username}
        [HttpGet("{username}")]
        public async Task<IActionResult> GetTasksByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username is required.");
            }

            var tasks = await _taskService.GetTasksByUsername(username);
            return Ok(tasks);
        }
        [HttpGet]
        [Route("Getbyedit/{Userid}")]
        public async Task<IActionResult> GetTasksByID(string Userid)
        {
            if (string.IsNullOrWhiteSpace(Userid))
            {
                return BadRequest("ID is required.");
            }

            var tasks = await _taskService.GetTasksByID(Userid);
            return Ok(tasks);
        }
        // POST: api/Task/AddTask
        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromBody] TaskListDTO taskDto)
        {
            if (taskDto == null || string.IsNullOrWhiteSpace(taskDto.Title))
            {
                return BadRequest(new TaskResponseDTO
                {
                    IsSuccess = false,
                    Message = "Task title is required."
                });
            }

            var result = await _taskService.AddTask(taskDto);

            if (!result.IsSuccess)
            {
                return StatusCode(500, result);
            }

            return Ok(result);
        }

        // PUT: api/Task/EditTask
        [HttpPut("EditTask")]
        public async Task<IActionResult> EditTask([FromBody] TaskListDTO taskDto)
        {
            if (taskDto == null || taskDto.ID <= 0)
            {
                return BadRequest(new TaskResponseDTO
                {
                    IsSuccess = false,
                    Message = "Valid task ID is required."
                });
            }

            var result = await _taskService.EditTask(taskDto);

            if (!result.IsSuccess)
            {
                if (result.Message == "Task not found.")
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        // DELETE: api/Task/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new TaskResponseDTO
                {
                    IsSuccess = false,
                    Message = "Valid task ID is required."
                });
            }

            var result = await _taskService.DeleteTask(id);

            if (!result.IsSuccess)
            {
                if (result.Message == "Task not found.")
                {
                    return NotFound(result);
                }

                return StatusCode(500, result);
            }

            return Ok(result);
        }
    }
}

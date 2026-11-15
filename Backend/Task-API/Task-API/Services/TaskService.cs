using Microsoft.EntityFrameworkCore;
using Serilog;
using Task_API.Data;
using Task_API.DTO;
using Task_API.Models;

namespace Task_API.Services
{
    public class TaskService : ITaskService_cs
    {
        private readonly TaskDbContext _context;

        public TaskService(TaskDbContext context)
        {
            _context = context;
        }

        // GET ALL TASKS
        public async Task<List<TaskListDTO>> GetAllTasks()
        {
            try
            {
                return await _context.task
                    .Select(t => new TaskListDTO
                    {
                        ID = t.ID,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
                        Priority = t.Priority,
                        CreatedDate = t.CreatedDate,
                        DueDate = t.DueDate,
                        Remark = t.Remark,
                        Username = t.Username
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching all tasks.");
                return new List<TaskListDTO>();
            }
        }

        // GET TASKS BY USERNAME
        public async Task<List<TaskListDTO>> GetTasksByUsername(string username)
        {
            try
            {
                return await _context.task
                    .Where(t => t.Username == username)
                    .Select(t => new TaskListDTO
                    {
                        ID = t.ID,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
                        Priority = t.Priority,
                        CreatedDate = t.CreatedDate,
                        DueDate = t.DueDate,
                        Remark = t.Remark,
                        Username = t.Username
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching tasks for user: {Username}", username);
                return new List<TaskListDTO>();
            }
        }

        public async Task<List<TaskListDTO>> GetTasksByID(string IDS)
        {
            try
            {
                return await _context.task
                    .Where(t => t.ID == Convert.ToInt32(IDS))
                    .Select(t => new TaskListDTO
                    {
                        ID = t.ID,
                        Title = t.Title,
                        Description = t.Description,
                        IsCompleted = t.IsCompleted,
                        Priority = t.Priority,
                        CreatedDate = t.CreatedDate,
                        DueDate = t.DueDate,
                        Remark = t.Remark,
                        Username = t.Username
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching tasks for user: {ID}", IDS);
                return new List<TaskListDTO>();
            }
        }

        // ADD TASK
        public async Task<TaskResponseDTO> AddTask(TaskListDTO taskDto)
        {
            try
            {
                var newTask = new Tasks
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    IsCompleted = string.IsNullOrEmpty(taskDto.IsCompleted) ? "false" : taskDto.IsCompleted,
                    Priority = taskDto.Priority,
                    CreatedDate = DateTime.UtcNow,
                    DueDate = taskDto.DueDate,
                    Remark = taskDto.Remark,
                    Username = taskDto.Username
                };

                await _context.task.AddAsync(newTask);
                await _context.SaveChangesAsync();

                taskDto.ID = newTask.ID;
                taskDto.CreatedDate = newTask.CreatedDate;

                return new TaskResponseDTO
                {
                    IsSuccess = true,
                    Message = "Task added successfully.",
                    Data = taskDto
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding task: {Title}", taskDto.Title);
                return new TaskResponseDTO
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the task."
                };
            }
        }

        // EDIT TASK
        public async Task<TaskResponseDTO> EditTask(TaskListDTO taskDto)
        {
            try
            {
                var existingTask = await _context.task.FirstOrDefaultAsync(t => t.ID == taskDto.ID);

                if (existingTask == null)
                {
                    return new TaskResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Task not found."
                    };
                }

                existingTask.Title = taskDto.Title;
                existingTask.Description = taskDto.Description;
                existingTask.IsCompleted = taskDto.IsCompleted;
                existingTask.Priority = taskDto.Priority;
                existingTask.DueDate = taskDto.DueDate;
                existingTask.Remark = taskDto.Remark;
                existingTask.Username = taskDto.Username;

                await _context.SaveChangesAsync();

                taskDto.CreatedDate = existingTask.CreatedDate;

                return new TaskResponseDTO
                {
                    IsSuccess = true,
                    Message = "Task updated successfully.",
                    Data = taskDto
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating task ID: {ID}", taskDto.ID);
                return new TaskResponseDTO
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the task."
                };
            }
        }

        // DELETE TASK
        public async Task<TaskResponseDTO> DeleteTask(int id)
        {
            try
            {
                var task = await _context.task.FirstOrDefaultAsync(t => t.ID == id);

                if (task == null)
                {
                    return new TaskResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Task not found."
                    };
                }

                _context.task.Remove(task);
                await _context.SaveChangesAsync();

                return new TaskResponseDTO
                {
                    IsSuccess = true,
                    Message = "Task deleted successfully."
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting task ID: {ID}", id);
                return new TaskResponseDTO
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the task."
                };
            }
        }
    }
}

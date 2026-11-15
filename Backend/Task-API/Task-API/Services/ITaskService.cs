using Task_API.DTO;

namespace Task_API.Services
{
    public interface ITaskService_cs
    {
        Task<List<TaskListDTO>> GetAllTasks();
        Task<List<TaskListDTO>> GetTasksByUsername(string username);
        Task<List<TaskListDTO>> GetTasksByID(string IDS);
        Task<TaskResponseDTO> AddTask(TaskListDTO taskDto);
        Task<TaskResponseDTO> EditTask(TaskListDTO taskDto);
        Task<TaskResponseDTO> DeleteTask(int id);
    }
}

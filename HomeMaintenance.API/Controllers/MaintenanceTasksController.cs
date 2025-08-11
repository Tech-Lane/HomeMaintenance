using Microsoft.AspNetCore.Mvc;
using HomeMaintenance.Core.Interfaces;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.API.Controllers
{
    /// <summary>
    /// Controller for managing maintenance tasks
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceTasksController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        /// <summary>
        /// Initializes a new instance of the MaintenanceTasksController
        /// </summary>
        /// <param name="maintenanceService">The maintenance service</param>
        public MaintenanceTasksController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? throw new ArgumentNullException(nameof(maintenanceService));
        }

        /// <summary>
        /// Gets all maintenance tasks
        /// </summary>
        /// <returns>Collection of maintenance tasks</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetMaintenanceTasks()
        {
            try
            {
                var tasks = await _maintenanceService.GetAllMaintenanceTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a maintenance task by ID
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <returns>The maintenance task if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaintenanceTask>> GetMaintenanceTask(int id)
        {
            try
            {
                var task = await _maintenanceService.GetMaintenanceTaskByIdAsync(id);
                if (task == null)
                {
                    return NotFound($"Maintenance task with ID {id} not found");
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets overdue maintenance tasks
        /// </summary>
        /// <returns>Collection of overdue maintenance tasks</returns>
        [HttpGet("overdue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetOverdueTasks()
        {
            try
            {
                var tasks = await _maintenanceService.GetOverdueMaintenanceTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets upcoming maintenance tasks
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead (default: 7)</param>
        /// <returns>Collection of upcoming maintenance tasks</returns>
        [HttpGet("upcoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetUpcomingTasks([FromQuery] int daysAhead = 7)
        {
            try
            {
                var tasks = await _maintenanceService.GetUpcomingMaintenanceTasksAsync(daysAhead);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets maintenance tasks by priority
        /// </summary>
        /// <param name="priority">The priority level</param>
        /// <returns>Collection of maintenance tasks with the specified priority</returns>
        [HttpGet("priority/{priority}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetTasksByPriority(string priority)
        {
            try
            {
                var tasks = await _maintenanceService.GetMaintenanceTasksByPriorityAsync(priority);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new maintenance task
        /// </summary>
        /// <param name="task">The maintenance task to create</param>
        /// <returns>The created maintenance task</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaintenanceTask>> CreateMaintenanceTask(MaintenanceTask task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdTask = await _maintenanceService.CreateMaintenanceTaskAsync(task);
                return CreatedAtAction(nameof(GetMaintenanceTask), new { id = createdTask.Id }, createdTask);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing maintenance task
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <param name="task">The updated maintenance task data</param>
        /// <returns>The updated maintenance task</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaintenanceTask>> UpdateMaintenanceTask(int id, MaintenanceTask task)
        {
            try
            {
                if (id != task.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTask = await _maintenanceService.UpdateMaintenanceTaskAsync(task);
                return Ok(updatedTask);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a maintenance task
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMaintenanceTask(int id)
        {
            try
            {
                var result = await _maintenanceService.DeleteMaintenanceTaskAsync(id);
                if (!result)
                {
                    return NotFound($"Maintenance task with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Marks a maintenance task as completed
        /// </summary>
        /// <param name="id">The task ID</param>
        /// <param name="completion">The completion details</param>
        /// <returns>The updated maintenance task</returns>
        [HttpPost("{id}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MaintenanceTask>> CompleteMaintenanceTask(int id, TaskCompletion completion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTask = await _maintenanceService.CompleteMaintenanceTaskAsync(id, completion);
                return Ok(updatedTask);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets maintenance tasks that need reminders
        /// </summary>
        /// <returns>Collection of maintenance tasks that need reminders</returns>
        [HttpGet("reminders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetTasksNeedingReminders()
        {
            try
            {
                var tasks = await _maintenanceService.GetMaintenanceTasksNeedingRemindersAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 
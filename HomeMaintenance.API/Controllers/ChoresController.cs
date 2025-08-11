using Microsoft.AspNetCore.Mvc;
using HomeMaintenance.Core.Interfaces;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.API.Controllers
{
    /// <summary>
    /// Controller for managing chores
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChoresController : ControllerBase
    {
        private readonly IChoreService _choreService;

        /// <summary>
        /// Initializes a new instance of the ChoresController
        /// </summary>
        /// <param name="choreService">The chore service</param>
        public ChoresController(IChoreService choreService)
        {
            _choreService = choreService ?? throw new ArgumentNullException(nameof(choreService));
        }

        /// <summary>
        /// Gets all chores
        /// </summary>
        /// <returns>Collection of chores</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Chore>>> GetChores()
        {
            try
            {
                var chores = await _choreService.GetAllChoresAsync();
                return Ok(chores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a chore by ID
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <returns>The chore if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Chore>> GetChore(int id)
        {
            try
            {
                var chore = await _choreService.GetChoreByIdAsync(id);
                if (chore == null)
                {
                    return NotFound($"Chore with ID {id} not found");
                }

                return Ok(chore);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets overdue chores
        /// </summary>
        /// <returns>Collection of overdue chores</returns>
        [HttpGet("overdue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Chore>>> GetOverdueChores()
        {
            try
            {
                var chores = await _choreService.GetOverdueChoresAsync();
                return Ok(chores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets upcoming chores
        /// </summary>
        /// <param name="daysAhead">Number of days to look ahead (default: 7)</param>
        /// <returns>Collection of upcoming chores</returns>
        [HttpGet("upcoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Chore>>> GetUpcomingChores([FromQuery] int daysAhead = 7)
        {
            try
            {
                var chores = await _choreService.GetUpcomingChoresAsync(daysAhead);
                return Ok(chores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets chores by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>Collection of chores in the specified category</returns>
        [HttpGet("category/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Chore>>> GetChoresByCategory(string category)
        {
            try
            {
                var chores = await _choreService.GetChoresByCategoryAsync(category);
                return Ok(chores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets chores by assigned person
        /// </summary>
        /// <param name="assignedTo">The assigned person</param>
        /// <returns>Collection of chores assigned to the specified person</returns>
        [HttpGet("assigned/{assignedTo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Chore>>> GetChoresByAssignedPerson(string assignedTo)
        {
            try
            {
                var chores = await _choreService.GetChoresByAssignedPersonAsync(assignedTo);
                return Ok(chores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new chore
        /// </summary>
        /// <param name="chore">The chore to create</param>
        /// <returns>The created chore</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Chore>> CreateChore(Chore chore)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdChore = await _choreService.CreateChoreAsync(chore);
                return CreatedAtAction(nameof(GetChore), new { id = createdChore.Id }, createdChore);
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
        /// Updates an existing chore
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <param name="chore">The updated chore data</param>
        /// <returns>The updated chore</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Chore>> UpdateChore(int id, Chore chore)
        {
            try
            {
                if (id != chore.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedChore = await _choreService.UpdateChoreAsync(chore);
                return Ok(updatedChore);
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
        /// Deletes a chore
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteChore(int id)
        {
            try
            {
                var result = await _choreService.DeleteChoreAsync(id);
                if (!result)
                {
                    return NotFound($"Chore with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Marks a chore as completed
        /// </summary>
        /// <param name="id">The chore ID</param>
        /// <param name="completion">The completion details</param>
        /// <returns>The updated chore</returns>
        [HttpPost("{id}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Chore>> CompleteChore(int id, ChoreCompletion completion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedChore = await _choreService.CompleteChoreAsync(id, completion);
                return Ok(updatedChore);
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
    }
} 
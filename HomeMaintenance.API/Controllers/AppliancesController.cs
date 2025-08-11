using Microsoft.AspNetCore.Mvc;
using HomeMaintenance.Core.Interfaces;
using HomeMaintenance.Core.Models;

namespace HomeMaintenance.API.Controllers
{
    /// <summary>
    /// Controller for managing appliances
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AppliancesController : ControllerBase
    {
        private readonly IApplianceService _applianceService;

        /// <summary>
        /// Initializes a new instance of the AppliancesController
        /// </summary>
        /// <param name="applianceService">The appliance service</param>
        public AppliancesController(IApplianceService applianceService)
        {
            _applianceService = applianceService ?? throw new ArgumentNullException(nameof(applianceService));
        }

        /// <summary>
        /// Gets all appliances
        /// </summary>
        /// <returns>Collection of appliances</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Appliance>>> GetAppliances()
        {
            try
            {
                var appliances = await _applianceService.GetAllAppliancesAsync();
                return Ok(appliances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets an appliance by ID
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>The appliance if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Appliance>> GetAppliance(int id)
        {
            try
            {
                var appliance = await _applianceService.GetApplianceByIdAsync(id);
                if (appliance == null)
                {
                    return NotFound($"Appliance with ID {id} not found");
                }

                return Ok(appliance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets appliances by category
        /// </summary>
        /// <param name="category">The category to filter by</param>
        /// <returns>Collection of appliances in the specified category</returns>
        [HttpGet("category/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Appliance>>> GetAppliancesByCategory(string category)
        {
            try
            {
                var appliances = await _applianceService.GetAppliancesByCategoryAsync(category);
                return Ok(appliances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets appliances with expiring warranties
        /// </summary>
        /// <param name="daysThreshold">Number of days to check ahead (default: 30)</param>
        /// <returns>Collection of appliances with expiring warranties</returns>
        [HttpGet("warranty/expiring")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Appliance>>> GetAppliancesWithExpiringWarranties([FromQuery] int daysThreshold = 30)
        {
            try
            {
                var appliances = await _applianceService.GetAppliancesWithExpiringWarrantiesAsync(daysThreshold);
                return Ok(appliances);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new appliance
        /// </summary>
        /// <param name="appliance">The appliance to create</param>
        /// <returns>The created appliance</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Appliance>> CreateAppliance(Appliance appliance)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdAppliance = await _applianceService.CreateApplianceAsync(appliance);
                return CreatedAtAction(nameof(GetAppliance), new { id = createdAppliance.Id }, createdAppliance);
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
        /// Updates an existing appliance
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <param name="appliance">The updated appliance data</param>
        /// <returns>The updated appliance</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Appliance>> UpdateAppliance(int id, Appliance appliance)
        {
            try
            {
                if (id != appliance.Id)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedAppliance = await _applianceService.UpdateApplianceAsync(appliance);
                return Ok(updatedAppliance);
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
        /// Deletes an appliance
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAppliance(int id)
        {
            try
            {
                var result = await _applianceService.DeleteApplianceAsync(id);
                if (!result)
                {
                    return NotFound($"Appliance with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets maintenance tasks for an appliance
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>Collection of maintenance tasks</returns>
        [HttpGet("{id}/maintenance-tasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetMaintenanceTasksForAppliance(int id)
        {
            try
            {
                var appliance = await _applianceService.GetApplianceByIdAsync(id);
                if (appliance == null)
                {
                    return NotFound($"Appliance with ID {id} not found");
                }

                var tasks = await _applianceService.GetMaintenanceTasksForApplianceAsync(id);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets manuals for an appliance
        /// </summary>
        /// <param name="id">The appliance ID</param>
        /// <returns>Collection of appliance manuals</returns>
        [HttpGet("{id}/manuals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ApplianceManual>>> GetManualsForAppliance(int id)
        {
            try
            {
                var appliance = await _applianceService.GetApplianceByIdAsync(id);
                if (appliance == null)
                {
                    return NotFound($"Appliance with ID {id} not found");
                }

                var manuals = await _applianceService.GetManualsForApplianceAsync(id);
                return Ok(manuals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 
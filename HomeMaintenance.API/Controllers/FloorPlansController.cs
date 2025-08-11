using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HomeMaintenance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FloorPlansController : ControllerBase
    {
        private static readonly string StorageRoot = Path.Combine(AppContext.BaseDirectory, "App_Data", "FloorPlans");

        public class FloorPlanSummary
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public DateTime UpdatedAt { get; set; }
        }

        public class FloorPlanDto
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Json { get; set; } = "{}";
            public DateTime UpdatedAt { get; set; }
        }

        private static string GetFilePath(string id) => Path.Combine(StorageRoot, $"{id}.json");

        [HttpGet]
        public ActionResult<IEnumerable<FloorPlanSummary>> List()
        {
            Directory.CreateDirectory(StorageRoot);
            var files = Directory.GetFiles(StorageRoot, "*.json");
            var list = files.Select(f =>
            {
                try
                {
                    var dto = JsonSerializer.Deserialize<FloorPlanDto>(System.IO.File.ReadAllText(f));
                    if (dto == null) return null;
                    return new FloorPlanSummary { Id = dto.Id, Name = dto.Name, UpdatedAt = dto.UpdatedAt };
                }
                catch { return null; }
            }).Where(x => x != null)!.ToList();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<FloorPlanDto> Get(string id)
        {
            Directory.CreateDirectory(StorageRoot);
            var path = GetFilePath(id);
            if (!System.IO.File.Exists(path)) return NotFound();
            var dto = JsonSerializer.Deserialize<FloorPlanDto>(System.IO.File.ReadAllText(path));
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<FloorPlanDto> Create([FromBody] FloorPlanDto input)
        {
            Directory.CreateDirectory(StorageRoot);
            var id = string.IsNullOrWhiteSpace(input.Id) ? Guid.NewGuid().ToString("N") : input.Id;
            var dto = new FloorPlanDto
            {
                Id = id,
                Name = string.IsNullOrWhiteSpace(input.Name) ? "Untitled" : input.Name,
                Json = string.IsNullOrWhiteSpace(input.Json) ? "{}" : input.Json,
                UpdatedAt = DateTime.UtcNow
            };
            System.IO.File.WriteAllText(GetFilePath(id), JsonSerializer.Serialize(dto));
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public ActionResult<FloorPlanDto> Update(string id, [FromBody] FloorPlanDto input)
        {
            Directory.CreateDirectory(StorageRoot);
            var path = GetFilePath(id);
            if (!System.IO.File.Exists(path)) return NotFound();
            var existing = JsonSerializer.Deserialize<FloorPlanDto>(System.IO.File.ReadAllText(path)) ?? new FloorPlanDto { Id = id };
            existing.Name = string.IsNullOrWhiteSpace(input.Name) ? existing.Name : input.Name;
            if (!string.IsNullOrWhiteSpace(input.Json)) existing.Json = input.Json;
            existing.UpdatedAt = DateTime.UtcNow;
            System.IO.File.WriteAllText(path, JsonSerializer.Serialize(existing));
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var path = GetFilePath(id);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            return NoContent();
        }
    }
}



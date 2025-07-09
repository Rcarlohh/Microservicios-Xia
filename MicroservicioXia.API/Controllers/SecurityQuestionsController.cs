using Microsoft.AspNetCore.Mvc;
using MicroservicioXia.Core.Entities;
using MicroservicioXia.Application.Services;

namespace MicroservicioXia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityQuestionsController : ControllerBase
    {
        private readonly ISecurityQuestionService _securityQuestionService;

        public SecurityQuestionsController(ISecurityQuestionService securityQuestionService)
        {
            _securityQuestionService = securityQuestionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SecurityQuestionTemplate>>> GetAllTemplates()
        {
            try
            {
                var templates = await _securityQuestionService.GetAllTemplatesAsync();
                return Ok(templates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SecurityQuestionTemplate>>> GetActiveTemplates()
        {
            try
            {
                var templates = await _securityQuestionService.GetActiveTemplatesAsync();
                return Ok(templates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SecurityQuestionTemplate>> GetTemplateById(string id)
        {
            try
            {
                var template = await _securityQuestionService.GetTemplateByIdAsync(id);
                if (template == null)
                    return NotFound(new { message = "Plantilla no encontrada" });

                return Ok(template);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SecurityQuestionTemplate>> CreateTemplate([FromBody] SecurityQuestionTemplate template)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdTemplate = await _securityQuestionService.CreateTemplateAsync(template);
                return CreatedAtAction(nameof(GetTemplateById), new { id = createdTemplate.Id }, createdTemplate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTemplate(string id, [FromBody] SecurityQuestionTemplate template)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                template.Id = id;
                var result = await _securityQuestionService.UpdateTemplateAsync(template);
                if (!result)
                    return NotFound(new { message = "Plantilla no encontrada" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTemplate(string id)
        {
            try
            {
                var result = await _securityQuestionService.DeleteTemplateAsync(id);
                if (!result)
                    return NotFound(new { message = "Plantilla no encontrada" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
} 
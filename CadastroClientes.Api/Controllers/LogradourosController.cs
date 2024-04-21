using CadastroClientes.Api.Models;
using CadastroClientes.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogradourosController : ControllerBase
    {
        private readonly ILogradouroService _logradouroService;

        public LogradourosController(ILogradouroService logradouroService)
        {
            _logradouroService = logradouroService;
        }

        #region Listar Logradouros
        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<Logradouro>>> GetLogradouros(int clienteId)
        {
            try
            {
                var logradouros = await _logradouroService.GetLogradouroById(clienteId);
                return Ok(logradouros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar logradouros: {ex.Message}");
            }
        }
        #endregion

        #region Consultar detalhes do Logradouro
        [HttpGet("{clienteId}/{id}")]
        public async Task<ActionResult<Logradouro>> GetLogradouro(int clienteId, int id)
        {
            try
            {
                var logradouro = await _logradouroService.GetLogradouroDetails(id, clienteId);
                if (logradouro == null)
                {
                    return NotFound();
                }
                return Ok(logradouro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar logradouro: {ex.Message}");
            }
        }
        #endregion

        #region Adicionar Logradouro        
        [HttpPost("{clienteId}")]
        public async Task<ActionResult<Logradouro>> CreateLogradouro(int clienteId, [FromBody] Logradouro logradouro)
        {
            if (ModelState.IsValid && clienteId != 0)
            {
                try
                {
                    await _logradouroService.AddLogradouro(logradouro, clienteId);
                    return CreatedAtAction(nameof(GetLogradouro), new { clienteId, id = logradouro.Id }, logradouro);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro ao adicionar logradouro: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region Editar Logradouro
        [HttpPut("{clienteId}/{id}")]
        public async Task<IActionResult> UpdateLogradouro(int clienteId, int id, [FromBody] Logradouro logradouro)
        {
            if (id != logradouro.Id)
            {
                return BadRequest();
            }

            try
            {
                await _logradouroService.UpdateLogradouro(logradouro, id, clienteId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar logradouro: {ex.Message}");
            }
        }
        #endregion

        #region Deletar Logradouro
        [HttpDelete("{clienteId}/{id}")]
        public async Task<IActionResult> DeleteLogradouro(int clienteId, int id)
        {
            try
            {
                await _logradouroService.DeleteLogradouro(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir logradouro: {ex.Message}");
            }
        }
        #endregion
    }
}

using CadastroClientes.Api.Data;
using CadastroClientes.Api.Models;
using CadastroClientes.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CadastroClientes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IClienteService _clienteService;
        private readonly ILogradouroService _logradouroService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ClientesController(AppDbContext dbContext, IClienteService clienteService, ILogradouroService logradouroService, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _clienteService = clienteService;
            _logradouroService = logradouroService;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Consultar Todos os Clientes
        [HttpGet]
        public async Task<IActionResult> GetAllClientes()
        {
            try
            {
                var clientes = await _clienteService.GetClientes();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar clientes: {ex.Message}");
            }
        }
        #endregion

        #region Consultar detalhes do Cliente
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(id);

                if (cliente == null)
                {
                    return NotFound();
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar os detalhes do cliente: {ex.Message}");
            }
        }
        #endregion

        #region Adicionar Cliente
        [HttpPost]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Adiciona um logotipo ao cliente.")]
        public async Task<IActionResult> AddCliente([FromForm] Cliente cliente, [FromForm(Name = "Logotipo")] IFormFile logotipo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _clienteService.EmailExists(cliente.Email))
                    {
                        return Conflict("O Email do Cliente Informado já Existe no Sistema.");
                    }

                    if (logotipo != null && logotipo.Length > 0)
                    {
                        if (logotipo.Length > 5 * 1024 * 1024)
                        {
                            return BadRequest("O tamanho da imagem do logotipo não pode exceder 5MB.");
                        }

                        var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(logotipo.FileName);

                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await logotipo.CopyToAsync(stream);
                        }

                        cliente.Logotipo = fileName;
                    }

                    await _clienteService.AddCliente(cliente);
                    return Ok(cliente);

                }
                catch(Exception ex)
                {
                    return StatusCode(500, $"Erro ao criar cliente: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region Editar Cliente

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromForm] Cliente cliente, [FromForm] IFormFile Logotipo)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }
            
            try
            {
                var clienteAtual = await _clienteService.GetClienteById(id);

                bool hasChanges =
                    clienteAtual.Email != cliente.Email ||
                    clienteAtual.Logotipo != cliente.Logotipo;

                if (!hasChanges)
                {
                    return NoContent();
                }

                if (Logotipo != null)
                {
                    if (Logotipo.Length > 5 * 1024 * 1024)
                    {
                        return BadRequest("O tamanho da imagem do logotipo não pode exceder 5MB.");
                    }

                    var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Logotipo.FileName);
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);

                    if (!string.IsNullOrEmpty(clienteAtual.Logotipo))
                    {
                        var oldFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", clienteAtual.Logotipo);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Logotipo.CopyToAsync(stream);
                    }
                    cliente.Logotipo = fileName;
                }
                else
                {
                    cliente.Logotipo = clienteAtual.Logotipo;
                }

                if (await _clienteService.EmailExistsExceptCurrent(cliente.Email, id))
                {
                    return Conflict("O Email do(a) Cliente Informado já Existe no Sistema.");
                }
                
                await _clienteService.UpdateCliente(cliente, _dbContext);

                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro ao editar o cliente: " + ex.Message); 
            }
        }
        #endregion

        #region Deletar Cliente        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var clienteId = id;

                var cliente = await _clienteService.GetClienteById(id);

                if (cliente == null)
                {
                    return NotFound();
                }

                await _clienteService.DeleteClienteELogradouros(id, clienteId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir o cliente: {ex.Message}");
            }
        }
        #endregion
    }
}

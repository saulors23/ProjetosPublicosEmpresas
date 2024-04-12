using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using CadastroClientes.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.Api.Controllers
{
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IClienteService _clienteService;
        private readonly ILogradouroService _logradouroService;

        public ClientesController(AppDbContext dbContext, IClienteService clienteService, ILogradouroService logradouroService)
        {
            _dbContext = dbContext;
            _clienteService = clienteService;
            _logradouroService = logradouroService;
        }

        #region Lista Todos os Clientes
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

        #region Lista Cliente por Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
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
                return StatusCode(500, $"Erro ao consultar cliente por ID: {ex.Message}");
            }
        }
        #endregion

        #region Lista Logradouro(s) por Id do Cliente
        [HttpGet("{clienteId}/logradouros")]
        public async Task<IActionResult> GetLogradourosByClienteId(int clienteId)
        {
            try
            {
                var logradouros = await _logradouroService.GetLogradourosByClienteId(clienteId);

                if (logradouros == null || !logradouros.Any())
                {
                    return NotFound($"Nenhum logradouro encontrado para o cliente com ID {clienteId}.");
                }

                return Ok(logradouros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao consultar logradouros para o cliente com ID {clienteId}: {ex.Message}");
            }
        }
        #endregion

        #region Adiciona Logradouro(s) pelo Id do Cliente
        [HttpPost("{clienteId}/logradouro")]
        public async Task<IActionResult> AddLogradouro(int clienteId, Logradouro logradouro)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(clienteId);

                if (cliente == null)
                {
                    return NotFound($"Cliente com ID {clienteId} não encontrado.");
                }

                logradouro.ClienteId = cliente.Id ?? 0;

                await _logradouroService.AddLogradouro(logradouro);

                return Ok(logradouro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar logradouro para o cliente com ID {clienteId}: {ex.Message}");
            }
        }
        #endregion

        #region Adiciona Cliente
        [HttpPost]
        public async Task<IActionResult> AddCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (await _clienteService.EmailExists(cliente.Email))
                {
                    return Conflict("O Email do Cliente Informado já Existe no Sistema.");
                }

                await _clienteService.AddCliente(cliente);
                return Ok(cliente);
            }
            return BadRequest(ModelState);
        }
        #endregion

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            await _clienteService.UpdateCliente(cliente, _dbContext);

            return NoContent();
        }


    }
}

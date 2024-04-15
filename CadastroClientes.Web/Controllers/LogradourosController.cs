using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using CadastroClientes.Core.Services;
using CadastroClientes.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CadastroClientes.Web.Controllers
{
    public class LogradourosController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogradouroService _logradouroService;
        private readonly IClienteService _clienteService;

        public LogradourosController(AppDbContext dbContext, ILogradouroService logradouroService, IClienteService clienteService)
        {
            _dbContext = dbContext;
            _logradouroService = logradouroService;
            _clienteService = clienteService;
        }

        #region Consulta todos os Logradouros do Cliente
        public async Task<IActionResult> Index(int clienteId)
        {
            try
            {
                var id = clienteId;

                var cliente = await _clienteService.GetClienteById(id);

                ViewBag.Clientes = id;

                if (cliente != null)
                {             
                    var logradouros = await _logradouroService.GetLogradouroById(clienteId);
                   
                    if (logradouros != null && logradouros.Any())
                    {
                        ViewBag.ClienteId = cliente.Id;
                        ViewBag.ClienteNome = cliente.Nome;

                        return View(logradouros);
                    }
                    else
                    {
                        ViewBag.ClienteId = cliente.Id;
                        ViewBag.ErrorMessage = "Não há logradouros cadastrados para este cliente.";
                        return View(new List<Logradouro>());
                    }                    
                }
                else
                {                    
                    ViewBag.ErrorMessage = "Cliente não encontrado.";
                    return View(new List<Logradouro>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os logradouros.";
                return View(new List<Logradouro>());
            }
        }
        #endregion

        #region Consulta detalhes do Logradouro
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var logradouro = await _logradouroService.GetLogradouroById(id);

                if (logradouro == null)
                {
                    return NotFound();
                }
                return View(logradouro);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os detalhes do Logradouro.";
                return View();
            }
        }
        #endregion

        [HttpGet]
        public IActionResult Create(int clienteId)
        {
            ViewBag.ClienteId = clienteId;
            var logradouro = new Logradouro { ClienteId = clienteId };
            return View(logradouro);
        }

        #region Adiciona um novo Logradouro ao Cliente
        [HttpPost]
        public async Task<IActionResult> Create(Logradouro logradouro, int clienteId)
        {
            if (ModelState.IsValid && clienteId != 0)
            {
                try
                {                    
                    await _logradouroService.AddLogradouro(logradouro, clienteId);

                    TempData["CreateSuccess"] = true;

                    return RedirectToAction("Index", new { clienteId = clienteId });
                }
                catch (Exception ex)
                {

                    ViewBag.ErrorMessage = "Ocorreu um erro ao criar o logradouro.";
                    return View(logradouro);
                }
            }
            return View(logradouro);
        }
        #endregion
    }
}

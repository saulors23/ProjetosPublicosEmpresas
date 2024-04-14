using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using CadastroClientes.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CadastroClientes.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IClienteService _clienteService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ClientesController(AppDbContext dbContext, IClienteService clienteService, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _clienteService = clienteService;
            _hostingEnvironment = hostingEnvironment;
        }

        #region Consulta todos os Clientes do Cadastro
        public async Task<IActionResult> Index()
        {
            try
            {
                var clientes = await _clienteService.GetClientes();

                return View(clientes ?? new List<Cliente>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os clientes.";
                return View(new List<Cliente>());
            }
        }
        #endregion

        #region Consulta detalhes do cliente no Cadastro
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteService.GetClienteById(id);

                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os detalhes do cliente.";
                return View();
            }
        }
        #endregion


        public IActionResult Create()
        {
            var cliente = new Cliente();
            return View(cliente);
        }

        #region Adiciona um novo cliente no Cadastro
        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente, IFormFile Logotipo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _clienteService.EmailExists(cliente.Email))
                    {
                        ViewBag.EmailExistsError = "O Email do(a) Cliente Informado já Existe no Sistema.";

                        ViewBag.IsReloadedPage = true;

                        return View(cliente);
                    }

                    if (Logotipo != null && Logotipo.Length > 0)
                    {
                        if (Logotipo.Length > 5 * 1024 * 1024)
                        {
                            ViewBag.ErrorMessage = "O tamanho da imagem do logotipo não pode exceder 5MB.";
                            return View(cliente);
                        }

                        // Gerar um nome único para o arquivo
                        var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Logotipo.FileName);

                        // Caminho físico onde o arquivo será armazenado
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);

                        // Salvar o arquivo fisicamente
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Logotipo.CopyToAsync(stream);
                        }

                        // Atribuir o caminho relativo do arquivo ao campo Logotipo
                        cliente.Logotipo = fileName;
                    }

                    await _clienteService.AddCliente(cliente);

                    TempData["CreateSuccess"] = true;

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Ocorreu um erro ao criar o cliente.";
                    return View(cliente);
                }
            }
            return View(cliente);
        }
        #endregion

        #region Edita os dados do cliente no Cadastro
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.GetClienteById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente, IFormFile Logotipo)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            var clienteAtual = await _clienteService.GetClienteById(id);

            bool hasChanges =                
                clienteAtual.Email != cliente.Email ||
                clienteAtual.Logotipo != cliente.Logotipo;

            if (!hasChanges)
            {
                return RedirectToAction("Index");
            }

            try
            {
                if (Logotipo != null)
                {
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
                    ViewBag.EmailExistsError = "O Email do(a) Cliente Informado já Existe no Sistema.";
                    ViewBag.IsReloadedPage = true;
                    return View(cliente);
                }

                await _clienteService.UpdateCliente(cliente, _dbContext);
                TempData["EditSuccess"] = true;

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Erro ao editar o cliente. Entre em contato com o suporte.");
                return View(cliente);
            }
        }
        #endregion

        #region Exclusão de Cadastro de Cliente
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = _clienteService.GetClienteById(id);

                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception(@"[Erro ao obter o cliente por ID: {ex.Message}]", ex);

                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _clienteService.DeleteCliente(id);

                TempData["DeleteSuccess"] = true;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao Excluir o cliente. Entre em contato com o suporte.");
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}

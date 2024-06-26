﻿using CadastroClientes.Core.Data;
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

        #region Consultar Logradouros
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

        #region Consultar detalhes do Logradouro
        public async Task<IActionResult> Details(int id, int clienteId)
        {
            try
            {
                var logradouro = await _logradouroService.GetLogradouroDetails(id, clienteId);

                if (logradouro == null)
                {
                    return NotFound();
                }

                ViewBag.ClienteId = clienteId;

                return View(logradouro);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os detalhes do Logradouro.";
                return View();
            }
        }
        #endregion

        #region Adicionar Logradouro
        [HttpGet]
        public IActionResult Create(int clienteId)
        {
            ViewBag.ClienteId = clienteId;
            var logradouro = new Logradouro { ClienteId = clienteId };
            return View(logradouro);
        }

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

        #region Editar Logradouro
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int clienteId)
        {
            try
            {
                var logradouro = await _logradouroService.GetLogradouroDetails(id, clienteId);

                if (logradouro == null)
                {
                    return NotFound();
                }

                ViewBag.ClienteId = clienteId;

                return View(logradouro);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os detalhes do Logradouro.";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, int clienteId, Logradouro logradouro)
        {
            if (ModelState.IsValid && clienteId != 0)
            {
                try
                {
                    await _logradouroService.UpdateLogradouro(logradouro, id, clienteId);

                    TempData["EditSuccess"] = true;

                    return RedirectToAction("Index", new { clienteId = clienteId });
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Ocorreu um erro ao atualizar o logradouro.";
                    return View(logradouro);
                }
            }
            return View(logradouro);
        }
        #endregion

        #region Deletar Logradouro
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int clienteId)
        {
            try
            {
                var logradouro = await _logradouroService.GetLogradouroDetails(id, clienteId);

                if (logradouro == null)
                {
                    return NotFound();
                }

                ViewBag.ClienteId = clienteId;

                return View(logradouro);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os detalhes do Logradouro.";
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int clienteId)
        {
            try
            {
                await _logradouroService.DeleteLogradouro(id);

                TempData["DeleteSuccess"] = true;

                return RedirectToAction("Index", new { clienteId = clienteId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao excluir o logradouro.";
                return RedirectToAction("Index", new { clienteId = clienteId });
            }
        }
        #endregion
    }
}

using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using CadastroClientes.Core.Repositories.Interfaces;
using CadastroClientes.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Core.Services
{
    public class LogradouroService : ILogradouroService
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly ILogradouroRepository _logradouroRepository;
        public LogradouroService(DbContextOptions<AppDbContext> dbContextOptions, ILogradouroRepository logradouroRepository)
        {
            _dbContextOptions = dbContextOptions;
            _logradouroRepository = logradouroRepository;
        }

        #region Consultar Logradouro por Id
        public async Task<List<Logradouro>> GetLogradouroById(int id)
        {
            return await _logradouroRepository.GetLogradouroById(id);
        }
        #endregion

        #region Consultar detalhes do Logradouro
        public async Task<Logradouro> GetLogradouroDetails(int id, int clienteId)
        {
            return await _logradouroRepository.GetLogradouroDetails(id, clienteId);
        }
        #endregion

        #region Adicionar um Novo Logradouro
        public async Task AddLogradouro(Logradouro logradouro, int clienteId)
        {
            logradouro.ClienteId = clienteId;
            await _logradouroRepository.AddLogradouro(logradouro);
        }
        public async Task AddLogradouroWithClientId(Logradouro logradouro, int clienteId)
        {
            logradouro.ClienteId = clienteId;
            await _logradouroRepository.AddLogradouro(logradouro);
        }
        #endregion

        #region Alterar Logradouro
        public async Task UpdateLogradouro(Logradouro logradouro, int id, int clienteId)
        {
            await _logradouroRepository.UpdateLogradouro(logradouro, id, clienteId);
        }
        #endregion

        #region Deletar Logradouro
        public async Task DeleteLogradouro(int id)
        {
            await _logradouroRepository.DeleteLogradouro(id);
        }
        #endregion

        #region Deletar Logradouros relacionado a um Cliente
        public async Task DeleteLogradourosByClienteId(int clienteId)
        {
            await _logradouroRepository.DeleteLogradourosByClienteId(clienteId);
        }
        #endregion
    }
}

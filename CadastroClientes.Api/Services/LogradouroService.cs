using CadastroClientes.Api.Data;
using CadastroClientes.Api.Models;
using CadastroClientes.Api.Repositories.Interfaces;
using CadastroClientes.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Api.Services
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

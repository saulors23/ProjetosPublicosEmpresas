using CadastroClientes.Core.Models;
using CadastroClientes.Core.Repositories.Interfaces;
using CadastroClientes.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Core.Services
{
    public class LogradouroService : ILogradouroService
    {
        private readonly ILogradouroRepository _logradouroRepository;

        public LogradouroService(ILogradouroRepository logradouroRepository)
        {
            _logradouroRepository = logradouroRepository;
        }

        public async Task AddLogradouro(Logradouro logradouro)
        {
            await _logradouroRepository.AddLogradouro(logradouro);
        }

        public async Task<List<Logradouro>> GetLogradourosByClienteId(int clienteId)
        {
            return await _logradouroRepository.GetLogradourosByClienteId(clienteId);
        }
    }
}

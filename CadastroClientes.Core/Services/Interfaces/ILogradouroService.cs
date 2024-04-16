using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Core.Services.Interfaces
{
    public interface ILogradouroService
    {        
        Task<List<Logradouro>> GetLogradouroById(int id);
        Task<Logradouro> GetLogradouroDetails(int id, int clienteId);        
        Task AddLogradouro(Logradouro logradouro, int clienteId);
        Task AddLogradouroWithClientId(Logradouro logradouro, int clienteId);
        Task UpdateLogradouro(Logradouro logradouro, int id, int clienteId);
        Task DeleteLogradouro(int id);
        Task DeleteLogradourosByClienteId(int clienteId);

    }
}

using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Core.Repositories.Interfaces
{
    public interface ILogradouroRepository
    {        
        Task<List<Logradouro>> GetLogradouroById(int id);
        Task<Logradouro> GetLogradouroDetails(int id, int clienteId);        
        Task AddLogradouro(Logradouro logradouro);
        Task UpdateLogradouro(Logradouro logradouro, int id, int clienteId);
        Task DeleteLogradouro(int id);
        Task DeleteLogradourosByClienteId(int clienteId);
    }
}

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
        Task AddLogradouro(Logradouro logradouro);
        Task<List<Logradouro>> GetLogradourosByClienteId(int clienteId);
    }
}

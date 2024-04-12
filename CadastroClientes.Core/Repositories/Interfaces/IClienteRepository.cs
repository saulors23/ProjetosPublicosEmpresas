using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Core.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetClientes();
        Task<Cliente> GetClienteById(int id);
        Task<bool> EmailExists(string email);
        Task<bool> EmailExistsExceptCurrent(string email, int currentClientId);
        Task AddCliente(Cliente cliente);
        Task UpdateCliente(Cliente cliente, AppDbContext context);
    }
}

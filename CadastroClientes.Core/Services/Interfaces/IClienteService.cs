using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Core.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetClientes();
        Task<Cliente> GetClienteById(int id);
        Task<bool> EmailExists(string email);
        Task<bool> EmailExistsExceptCurrent(string email, int currentClientId);
        Task AddCliente(Cliente cliente);
        Task UpdateCliente(Cliente cliente, AppDbContext context);
        Task DeleteCliente(int id);
    }
}

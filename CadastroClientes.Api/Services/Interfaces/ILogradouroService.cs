using CadastroClientes.Api.Models;

namespace CadastroClientes.Api.Services.Interfaces
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

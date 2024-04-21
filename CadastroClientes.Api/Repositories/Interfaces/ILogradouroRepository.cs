using CadastroClientes.Api.Models;

namespace CadastroClientes.Api.Repositories.Interfaces
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

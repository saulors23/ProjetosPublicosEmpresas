using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using CadastroClientes.Core.Repositories;
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
    public class ClienteService : IClienteService
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly IClienteRepository _clienteRepository;
        private readonly ILogradouroRepository _logradouroRepository;
        public ClienteService(DbContextOptions<AppDbContext> dbContextOptions, IClienteRepository clienteRepository, ILogradouroRepository logradouroRepository)
        {
            _dbContextOptions = dbContextOptions;
            _clienteRepository = clienteRepository;
            _logradouroRepository = logradouroRepository;
        }

        #region Consultar todos os Clientes
        public async Task<List<Cliente>> GetClientes()
        {
            try
            {
                var listaClientes = await _clienteRepository.GetClientes();

                if (listaClientes != null && listaClientes is List<Cliente>)
                {
                    return listaClientes;
                }

                return new List<Cliente>();
            }
            catch (Exception ex)
            {
                return new List<Cliente>();
            }
        }
        #endregion

        #region Consultar detalhes do Cliente
        public async Task<Cliente> GetClienteById(int id)
        {
            return await _clienteRepository.GetClienteById(id);
        }
        #endregion

        #region Adicionar Cliente
        public async Task<bool> EmailExists(string email)
        {
            return await _clienteRepository.EmailExists(email);
        }
        public async Task AddCliente(Cliente cliente)
        {
            await _clienteRepository.AddCliente(cliente);
        }
        #endregion

        #region Alterar Cliente
        public async Task UpdateCliente(Cliente cliente, AppDbContext context)
        {
            try
            {
                var existingCliente = await context.Clientes.FindAsync(cliente.Id);

                if (existingCliente != null)
                {                    
                    context.Entry(existingCliente).CurrentValues.SetValues(cliente);

                    context.Entry(existingCliente).Property(x => x.DataInclusao).IsModified = false;

                    existingCliente.DataAlteracao = DateTime.Now;

                    context.Entry(existingCliente).State = EntityState.Modified;

                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Cliente não encontrado");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao atualizar o cliente no banco de dados.", ex);
            }
        }
        #endregion

        #region Verificar se na Alteração do Cliente o email existe
        public async Task<bool> EmailExistsExceptCurrent(string email, int currentClientId)
        {
            return await _clienteRepository.EmailExistsExceptCurrent(email, currentClientId);
        }
        #endregion

        #region Deletar Cliente
        public async Task DeleteClienteELogradouros(int id, int clienteId)
        {
            await _logradouroRepository.DeleteLogradourosByClienteId(clienteId);

            await _clienteRepository.DeleteCliente(id);            
        }
        #endregion
    }
}

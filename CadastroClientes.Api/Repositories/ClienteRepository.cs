using CadastroClientes.Api.Data;
using CadastroClientes.Api.Models;
using CadastroClientes.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Api.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _dbContext;
        public ClienteRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Consultar todos os Clientes
        public async Task<List<Cliente>> GetClientes()
        {
            var clientes = await _dbContext.Clientes.ToListAsync();

            if (clientes == null) throw new System.Exception("Não existe registros de Clientes!");

            return clientes ?? new List<Cliente>();
        }
        #endregion

        #region Consultar detalhes do Cliente
        public async Task<Cliente> GetClienteById(int id)
        {
            return await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        }
        #endregion

        #region Verificar se email do Cliente já existe
        public async Task<bool> EmailExists(string email)
        {
            return await _dbContext.Clientes.AnyAsync(c => c.Email == email);
        }
        #endregion

        #region Adicionar Cliente
        public async Task AddCliente(Cliente cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente));
            }

            await _dbContext.Clientes.AddAsync(cliente);
            await _dbContext.SaveChangesAsync();
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
                    cliente.DataInclusao = existingCliente.DataInclusao;

                    context.Entry(existingCliente).CurrentValues.SetValues(cliente);

                    context.Entry(existingCliente).Property(x => x.DataInclusao).IsModified = false;

                    await context.SaveChangesAsync();
                }

                cliente.DataAlteracao = DateTime.Now;

                context.Entry(cliente).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao atualizar o cliente no banco de dados.", ex);
            }
        }
        #endregion

        #region Verificar se Alteração do Cliente email existe
        public async Task<bool> EmailExistsExceptCurrent(string email, int currentClientId)
        {
            return await _dbContext.Clientes.AnyAsync(c => c.Email == email && c.Id != currentClientId);
        }
        #endregion

        #region Deletar Cliente
        public async Task DeleteCliente(int id)
        {
            var clienteToRemove = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (clienteToRemove != null)
            {
                _dbContext.Clientes.Remove(clienteToRemove);
                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion
    }
}

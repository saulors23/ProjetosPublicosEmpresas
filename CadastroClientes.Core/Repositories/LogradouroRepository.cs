using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
using CadastroClientes.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Core.Repositories
{
    public class LogradouroRepository : ILogradouroRepository
    {
        private readonly AppDbContext _context;

        public LogradouroRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Adiconar Logradouro
        public async Task AddLogradouro(Logradouro logradouro)
        {
            _context.Logradouros.Add(logradouro);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Listar os Logradouros com referência ao Cliente
        public async Task<List<Logradouro>> GetLogradouroById(int id)
        {
            return await _context.Logradouros.Where(l => l.ClienteId == id).ToListAsync();
        }
        #endregion

        #region Listar os Detalhes de cada Logradouro
        public async Task<Logradouro> GetLogradouroDetails(int id, int clienteId)
        {
            return await _context.Logradouros.FirstOrDefaultAsync(l => l.Id == id && l.ClienteId == clienteId);
        }
        #endregion

        #region Editar Logradouro
        public async Task UpdateLogradouro(Logradouro logradouro, int id, int clienteId)
        {
            var existingLogradouro = await _context.Logradouros
                .FirstOrDefaultAsync(l => l.Id == id && l.ClienteId == clienteId);

            if (existingLogradouro != null)
            {
                existingLogradouro.Endereco = logradouro.Endereco;
                existingLogradouro.Complemento = logradouro.Complemento;
                existingLogradouro.Bairro = logradouro.Bairro;
                existingLogradouro.Cidade = logradouro.Cidade;
                existingLogradouro.Uf = logradouro.Uf;
                var dataInclusao = existingLogradouro.DataInclusao;
                existingLogradouro.DataInclusao = dataInclusao;
                existingLogradouro.DataAlteracao = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Logradouro não encontrado para atualização.");
            }
        }
        #endregion

        #region Deletar Logradouro
        public async Task DeleteLogradouro(int id)
        {
            var logradouroToDelete = await _context.Logradouros.FindAsync(id);

            if (logradouroToDelete != null)
            {
                _context.Logradouros.Remove(logradouroToDelete);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Logradouro não encontrado para exclusão.");
            }
        }
        #endregion

        #region Deletar Logradouros relacionado a um Cliente
        public async Task DeleteLogradourosByClienteId(int clienteId)
        {
            var logradourosToRemove = await _context.Logradouros.Where(l => l.ClienteId == clienteId).ToListAsync();
            if (logradourosToRemove.Any())
            {
                _context.Logradouros.RemoveRange(logradourosToRemove);
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}

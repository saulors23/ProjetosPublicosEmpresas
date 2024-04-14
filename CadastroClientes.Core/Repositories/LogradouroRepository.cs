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

        #region Adicona Logradouro
        public async Task AddLogradouro(Logradouro logradouro)
        {
            _context.Logradouros.Add(logradouro);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Lista os Logradouros com referência ao Cliente
        public async Task<List<Logradouro>> GetLogradouroById(int id)
        {
            return await _context.Logradouros.Where(l => l.ClienteId == id).ToListAsync();
        }
        #endregion
    }
}

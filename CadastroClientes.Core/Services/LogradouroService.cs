using CadastroClientes.Core.Data;
using CadastroClientes.Core.Models;
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
    public class LogradouroService : ILogradouroService
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly ILogradouroRepository _logradouroRepository;
        public LogradouroService(DbContextOptions<AppDbContext> dbContextOptions, ILogradouroRepository logradouroRepository)
        {
            _dbContextOptions = dbContextOptions;
            _logradouroRepository = logradouroRepository;
        }

        //#region Consulta todos os Logradouros do Cadastro
        //public async Task<List<Logradouro>> GetLogradouros()
        //{
        //    try
        //    {
        //        var listaLogradouros = await _logradouroRepository.GetLogradouros();

        //        if (listaLogradouros != null && listaLogradouros is List<Logradouro>)
        //        {
        //            return listaLogradouros;
        //        }

        //        return new List<Logradouro>();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<Logradouro>();
        //    }
        //}
        //#endregion

        #region Consulta detalhes do Logradouro
        public async Task<List<Logradouro>> GetLogradouroById(int id)
        {
            return await _logradouroRepository.GetLogradouroById(id);
        }
        #endregion

        #region Adiciona um Novo Logradouro
        public async Task AddLogradouro(Logradouro logradouro, int clienteId)
        {
            logradouro.ClienteId = clienteId;
            await _logradouroRepository.AddLogradouro(logradouro);
        }
        public async Task AddLogradouroWithClientId(Logradouro logradouro, int clienteId)
        {
            logradouro.ClienteId = clienteId;
            await _logradouroRepository.AddLogradouro(logradouro);
        }
        #endregion

        //#region Altera dados do Logradouro
        //public async Task UpdateLogradouro(Logradouro logradouro, AppDbContext context)
        //{
        //    try
        //    {                
        //        if (logradouro != null)
        //        {                    
        //            context.Entry(logradouro).Property(x => x.DataInclusao).IsModified = false;

        //            logradouro.DataAlteracao = DateTime.Now;

        //            context.Entry(logradouro).State = EntityState.Modified;

        //            await context.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Logradouro não encontrado");
        //        }
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        throw new Exception("Erro ao atualizar o Logradouro no banco de dados.", ex);
        //    }
        //}
        //#endregion

        //#region Exclui Logradouro
        //public async Task DeleteLogradouro(int id)
        //{
        //    await _logradouroRepository.DeleteLogradouro(id);
        //}
        //#endregion
    }
}

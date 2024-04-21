using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Api.Models
{
    public class Cliente
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Logotipo { get; set; }        
        public DateTime DataInclusao { get; set; } = DateTime.Now;        
        public DateTime? DataAlteracao { get; set; }
    }
}


namespace CadastroClientes.Api.Models
{
    public class Logradouro
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Endereco { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }        
        public DateTime DataInclusao { get; set; } = DateTime.Now;        
        public DateTime? DataAlteracao { get; set; }
    }
}

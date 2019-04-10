using System;
using System.Collections.Generic;

namespace Dados.DTO
{
    public class PessoaDTO
    {
        public long? Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Cpf { get; set; }
        public IEnumerable<EnderecoDTO> Enderecos { get; set; }
        public IEnumerable<TelefoneDTO> Telefones { get; set; }
    }
}

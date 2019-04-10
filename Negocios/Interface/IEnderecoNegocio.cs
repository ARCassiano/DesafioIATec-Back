using Dados.DTO;
using System.Collections.Generic;

namespace Negocios.Interface
{
    public interface IEnderecoNegocio
    {
        EnderecoDTO Adicionar(long idPessoa, EnderecoDTO endereco);
        EnderecoDTO Atualizar(long idPessoa, long id, EnderecoDTO endereco);
        bool Remover(long idPessoa, long id);
        IEnumerable<EnderecoDTO> Consultar(long idPessoa);
        EnderecoDTO Consultar(long idPessoa, long id);
    }
}

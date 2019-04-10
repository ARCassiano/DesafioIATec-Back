using Dados.DTO;
using System.Collections.Generic;

namespace Negocios.Interface
{
    public interface ITelefoneNegocio
    {
        TelefoneDTO Adicionar(long idPessoa, TelefoneDTO telefone);
        TelefoneDTO Atualizar(long idPessoa, long id, TelefoneDTO telefone);
        bool Remover(long idPessoa, long id);
        IEnumerable<TelefoneDTO> Consultar(long idPessoa);
        TelefoneDTO Consultar(long idPessoa, long id);
    }
}

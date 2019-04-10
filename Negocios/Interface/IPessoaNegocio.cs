using Dados.DTO;
using System.Collections.Generic;

namespace Negocios.Interface
{
    public interface IPessoaNegocio
    {
        PessoaDTO Adicionar(PessoaDTO pessoa);
        PessoaDTO Atualizar(long id, PessoaDTO pessoa);
        bool Remover(long id);
        IEnumerable<PessoaDTO> Consultar();
        PessoaDTO Consultar(long id);
    }
}

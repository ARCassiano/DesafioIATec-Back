using Dados.Entidade;
using System.Linq;

namespace Dados.Repositorio
{
    public interface IRepositorioGenerico
    {
        TEntidade Create<TEntidade>(TEntidade entidade) where TEntidade : EntidadeBase;

        IQueryable<TEntidade> Read<TEntidade>(bool asTracking = false) where TEntidade : EntidadeBase;

        TEntidade ReadById<TEntidade>(long id, bool asTracking = false) where TEntidade : EntidadeBase;

        TEntidade Update<TEntidade>(TEntidade entidade) where TEntidade : EntidadeBase;

        void Delete<TEntidade>(TEntidade entidade) where TEntidade : EntidadeBase;

    }
}

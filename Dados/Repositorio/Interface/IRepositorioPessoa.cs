using Dados.Entidades;
using System.Linq;

namespace Dados.Repositorio.Interface
{
    public interface IRepositorioPessoa: IRepositorioGenerico
    {
        IQueryable<Pessoa> Read(bool asTracking = false);

        Pessoa ReadById(long id, bool asTracking = false);
    }
}

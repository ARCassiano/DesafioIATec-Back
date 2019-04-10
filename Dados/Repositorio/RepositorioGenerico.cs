using Dados.Entidade;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dados.Repositorio
{
    public class RepositorioGenerico: IRepositorioGenerico
    {
        protected DbContext Context;

        public RepositorioGenerico(DbContext context)
        {
            this.Context = context;
        }

        public TEntidade Create<TEntidade>(TEntidade entidade) where TEntidade: EntidadeBase
        {
            this.Context.Add(entidade);
            this.Context.SaveChanges();

            return entidade;
        }

        public virtual IQueryable<TEntidade> Read<TEntidade>(bool asTracking = false) 
            where TEntidade : EntidadeBase
        {
            if (asTracking)
                return this.Context.Set<TEntidade>().AsTracking();

            return this.Context.Set<TEntidade>().AsNoTracking();
        }

        public virtual TEntidade ReadById<TEntidade>(long id, bool asTracking = false) where TEntidade : EntidadeBase 
            => this.Read<TEntidade>(asTracking).FirstOrDefault(x => x.Id == id);

        public TEntidade Update<TEntidade>(TEntidade entidade) where TEntidade : EntidadeBase
        {
            this.Context.Update(entidade);
            this.Context.SaveChanges();

            return entidade;
        }

        public void Delete<TEntidade>(TEntidade entidade) where TEntidade : EntidadeBase
        {
            this.Context.Remove(entidade);
            this.Context.SaveChanges();
        }
    }
}

using Dados.Entidades;
using Dados.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dados.Repositorio
{
    public class RepositorioPessoa : RepositorioGenerico, IRepositorioPessoa
    {
        public RepositorioPessoa(DbContext context) : base(context)
        {
        }

        public IQueryable<Pessoa> Read(bool asTracking = false)
        {
            IQueryable<Pessoa>  pessoas = base.Read<Pessoa>(asTracking)
                .Include(x => x.Enderecos)
                .Include(x => x.Telefones);

            return pessoas;
        }

        public Pessoa ReadById(long id, bool asTracking = false) => this.Read(asTracking).FirstOrDefault(x => x.Id == id);
    }
}

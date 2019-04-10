using Microsoft.EntityFrameworkCore;

namespace Dados.Mapeamento
{
    public class DesafioContext: DbContext
    {
        public DesafioContext(DbContextOptions<DesafioContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaMap());
            modelBuilder.ApplyConfiguration(new TelefoneMap());
            modelBuilder.ApplyConfiguration(new EnderecoMap());
        }   
    }
}

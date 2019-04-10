using Dados.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dados.Mapeamento
{
    public class PessoaMap : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoa");

            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Enderecos).WithOne(x => x.Pessoa).HasForeignKey(x => x.IdPessoa);
            builder.HasMany(x => x.Telefones).WithOne(x => x.Pessoa).HasForeignKey(x => x.IdPessoa);
        }
    }
}

using Dados.Mapeamento;
using Microsoft.EntityFrameworkCore;
using System;

namespace Testes
{
    public class TesteConfiguracao
    {
        private TesteConfiguracao()
        {
            DbContextOptionsBuilder<DesafioContext> optionsBuilder = new DbContextOptionsBuilder<DesafioContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Desafio;Trusted_Connection=True;", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            TesteConfiguracao.Options = optionsBuilder.Options;
        }

        private static TesteConfiguracao Instance;
        private static DbContextOptions<DesafioContext> Options { get; set; }

        public static DbContextOptions<DesafioContext> GetDbContextOptions()
        {
            if (TesteConfiguracao.Instance == null)
                TesteConfiguracao.Instance = new TesteConfiguracao();

            return TesteConfiguracao.Options;
        }
    }
}

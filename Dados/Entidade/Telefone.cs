using Dados.Entidade;
using Dados.Enum;
using System;

namespace Dados.Entidades
{
    public class Telefone: EntidadeBase
    {
        protected Telefone()
        {
            this.Ddi = "+55";
        }

        public Telefone(string numero, string operadora, string ddi)
            : this()
        {
            this.SetNumero(numero);
            this.Operadora = operadora;
            if (!string.IsNullOrEmpty(ddi) && !string.IsNullOrWhiteSpace(ddi))
                this.Ddi = ddi;
        }

        public string Numero { get; private set; }
        public string Operadora { get; set; }
        public string Ddi { get; set; }
        public long IdPessoa { get; set; }
        public Pessoa Pessoa { get; set; }

        public void SetNumero(string numero)
        {
            this.CampoObigatorio("Telefone", numero);
            this.Numero = this.LimparTelefone(numero);
        }

        private void CampoObigatorio(string chave, string valor)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
                throw new Exception($"O campo '{chave}' é obrigatório");
        }

        private string LimparTelefone(string telefone) => telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
    }
}

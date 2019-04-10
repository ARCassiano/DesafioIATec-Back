using Dados.Entidade;
using System;

namespace Dados.Entidades
{
    public class Endereco: EntidadeBase
    {
        protected Endereco() { }

        public Endereco(string tipo, string logradouro, string numero, string cep, string bairro, string complemento = null)
        {
            this.SetTipo(tipo);
            this.SetLogradouro(logradouro);
            this.SetNumero(numero);
            this.SetCep(cep);
            this.SetBairro(bairro);

            this.Complemento = complemento;
        }

        public Endereco(Pessoa pessoa, string tipo, string logradouro, string numero, string cep, string bairro, string complemento = null) 
            : this(tipo, logradouro, numero, cep, bairro, complemento) 
        {
            this.Pessoa = pessoa;
        }

        public string Tipo { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Cep { get; private set; }
        public string Bairro { get; private set; }
        public string Complemento { get; set; }
        public long IdPessoa { get; set; }
        public Pessoa Pessoa { get; set; }

        public void SetTipo(string tipo)
        {
            this.CampoObigatorio("Tipo", tipo);
            this.Tipo = tipo;
        }

        public void SetLogradouro(string logradouro)
        {
            this.CampoObigatorio("Logradouro", logradouro);
            this.Logradouro = logradouro;
        }

        public void SetNumero(string numero)
        {
            this.CampoObigatorio("Numero", numero);
            this.Numero = numero;
        }

        public void SetCep(string cep)
        {
            this.CampoObigatorio("CEP", cep);
            if (!this.ECepValido(cep))
                throw new Exception("O valor do campo 'CEP' é inválido.");

            this.Cep = this.LimparCep(cep);
        }

        public void SetBairro(string bairro)
        {
            this.CampoObigatorio("Bairro", bairro);
            this.Bairro = bairro;
        }

        public bool ECepValido(string cep)
        {
            long cepNumerico;
            cep = this.LimparCep(cep);

            if (!long.TryParse(cep, out cepNumerico))
                return false;

            return true;
        }

        private string LimparCep(string cep) => cep.Replace(".", "").Replace("-", "");

        private void CampoObigatorio(string chave, string valor)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor))
                throw new Exception($"O campo '{chave}' é obrigatório");
        }
    }
}

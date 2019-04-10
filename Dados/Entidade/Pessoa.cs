using Dados.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dados.Entidades
{
    public class Pessoa: EntidadeBase
    {
        protected Pessoa()
        {
            this.Enderecos = new List<Endereco>();
            this.Telefones = new List<Telefone>();
        } 

        public Pessoa(string nome, DateTime dataNascimento, string cpf, IEnumerable<Endereco> enderecos = null, IEnumerable<Telefone> telefones = null)
            : this()
        {
            this.SetNome(nome);
            this.SetDataNascimento(dataNascimento);
            this.SetCpf(cpf);
            this.Enderecos = enderecos.ToList();
            this.Telefones = telefones.ToList();
        }

        public string Nome { get; private set; }
        public DateTime DataNascimento { get; set; }
        public string Cpf { get; set; }
        public List<Endereco> Enderecos { get; private set; }
        public List<Telefone> Telefones { get; private set; }

        public void SetNome(string nome)
        {
            this.Nome = nome;
        }

        public void SetDataNascimento(DateTime dataNascimento)
        {
            this.DataNascimento = dataNascimento;
        }

        public void SetCpf(string cpf)
        {
            this.Cpf = cpf.Replace("-", "").Replace(".", "");
        }

    }
}

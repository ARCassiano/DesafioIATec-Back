using Dados.DTO;
using Dados.Entidades;
using Dados.Mapeamento;
using Dados.Repositorio;
using Dados.Repositorio.Interface;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Negocios;
using Negocios.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Testes
{
    public class TelefoneNegocioTeste
    {
        private readonly ITelefoneNegocio TelefoneNegocio;
        private readonly IRepositorioPessoa Repositorio;

        public TelefoneNegocioTeste()
        {
            RepositorioPessoa repositorio = new RepositorioPessoa(new DesafioContext(TesteConfiguracao.GetDbContextOptions()));
            this.TelefoneNegocio = new TelefoneNegocio(repositorio);
            this.Repositorio = repositorio;
        }

        [Fact(DisplayName = "Adicionar um telefone a um pessoa com sucesso")]
        public void CriarPessoaComSucesso()
        {
            TelefoneDTO telefoneDTO = new TelefoneDTO()
            {
                Operadora = "Vivo",
                Numero = "(99) 12345-1234"
            };

            int qtdTelefones = 0;
            Pessoa pessoa = this.Repositorio.Read(true).OrderByDescending(x => x.Telefones.Count()).FirstOrDefault();

            if (pessoa != null)
            {
                qtdTelefones = pessoa.Telefones.Count();
                telefoneDTO = this.TelefoneNegocio.Adicionar(pessoa.Id, telefoneDTO);
            }

            telefoneDTO.Should().NotBeNull();
            pessoa.Should().NotBeNull();
            qtdTelefones.Should().Be(qtdTelefones++);
            telefoneDTO.Id.Should().BePositive();
            telefoneDTO.Ddi.Should().Be("+55");
            telefoneDTO.Numero.Should().Be("99123451234");
            telefoneDTO.Operadora.Should().Be("Vivo");

        }

        [Fact(DisplayName = "Consultar um telefone de pessoa com sucesso")]
        public void ConsultarPessoaComSucesso()
        {
            TelefoneDTO telefoneDTO = null;

            Pessoa pessoa = this.Repositorio.Read().FirstOrDefault(x => x.Telefones.Any());

            if (pessoa != null)
                telefoneDTO = this.TelefoneNegocio.Consultar(pessoa.Id, pessoa.Telefones.First().Id);

            telefoneDTO.Should().NotBeNull();
        }

        [Fact(DisplayName = "Consultar telefones da pessoa com sucesso")]
        public void ConsultarPessoasComSucesso()
        {
            IEnumerable<TelefoneDTO> telefoneDTO = null;

            Pessoa pessoa = this.Repositorio.Read().FirstOrDefault(x => x.Telefones.Any());

            if (pessoa != null)
                telefoneDTO = this.TelefoneNegocio.Consultar(pessoa.Id);

            telefoneDTO.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Remover o telefone de uma pessoa com sucesso")]
        public void RemoverPessoaComSucesso()
        {
            bool removido = false;

            Pessoa pessoa = this.Repositorio.Read().FirstOrDefault(x => x.Telefones.Any());
            if (pessoa != null)
                removido = this.TelefoneNegocio.Remover(pessoa.Id, pessoa.Telefones.First().Id);

            removido.Should().BeTrue();
        }
    }
}

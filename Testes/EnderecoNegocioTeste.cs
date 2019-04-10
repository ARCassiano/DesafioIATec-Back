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
    public class EnderecoNegocioTeste
    {
        private readonly IEnderecoNegocio EnderecoNegocio;
        private readonly IRepositorioPessoa Repositorio;

        public EnderecoNegocioTeste()
        {
            RepositorioPessoa repositorio = new RepositorioPessoa(new DesafioContext(TesteConfiguracao.GetDbContextOptions()));
            this.EnderecoNegocio = new EnderecoNegocio(repositorio);
            this.Repositorio = repositorio;
        }

        [Fact(DisplayName = "Adicionar um endereço a um pessoa com sucesso")]
        public void CriarPessoaComSucesso()
        {
            EnderecoDTO enderecoDTO = new EnderecoDTO()
            {
                Bairro = "Bairro da paz",
                Cep = "11.807-555",
                Complemento = "Sem complemento",
                Logradouro = "Dos Santos",
                Numero = "123",
                Tipo = "Rua"
            };

            int qtdEnderecos = 0;
            Pessoa pessoa = this.Repositorio.Read(true).OrderByDescending(x => x.Enderecos.Count()).FirstOrDefault();

            if (pessoa != null)
            {
                qtdEnderecos = pessoa.Enderecos.Count();
                enderecoDTO = this.EnderecoNegocio.Adicionar(pessoa.Id, enderecoDTO);
            }
            
            enderecoDTO.Should().NotBeNull();
            pessoa.Should().NotBeNull();
            qtdEnderecos.Should().Be(qtdEnderecos++);
            enderecoDTO.Id.Should().BePositive();
            enderecoDTO.Bairro.Should().Be("Bairro da paz");
            enderecoDTO.Cep.Should().Be("11807555");
            enderecoDTO.Complemento.Should().Be("Sem complemento");
            enderecoDTO.Logradouro.Should().Be("Dos Santos");
            enderecoDTO.Numero.Should().Be("123");
            enderecoDTO.Tipo.Should().Be("Rua");
            
        }

        [Fact(DisplayName = "Consultar um endereço de pessoa com sucesso")]
        public void ConsultarPessoaComSucesso()
        {
            EnderecoDTO enderecoDTO = null;

            Pessoa pessoa = this.Repositorio.Read().FirstOrDefault(x => x.Enderecos.Any());

            if (pessoa != null)
                enderecoDTO = this.EnderecoNegocio.Consultar(pessoa.Id, pessoa.Enderecos.First().Id);

            enderecoDTO.Should().NotBeNull();
        }

        [Fact(DisplayName = "Consultar endereços da pessoa com sucesso")]
        public void ConsultarPessoasComSucesso()
        {
            IEnumerable<EnderecoDTO> enderecoDTO = null;

            Pessoa pessoa = this.Repositorio.Read().FirstOrDefault(x => x.Enderecos.Any());

            if (pessoa != null)
                enderecoDTO = this.EnderecoNegocio.Consultar(pessoa.Id);

            enderecoDTO.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Remover o endereço de uma pessoa com sucesso")]
        public void RemoverPessoaComSucesso()
        {
            bool removido = false;

            Pessoa pessoa = this.Repositorio.Read().FirstOrDefault(x => x.Enderecos.Any());
            if (pessoa != null)
                removido = this.EnderecoNegocio.Remover(pessoa.Id, pessoa.Enderecos.First().Id);

            removido.Should().BeTrue();
        }
    }
}

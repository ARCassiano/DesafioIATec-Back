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
    public class PessoaNegocioTeste
    {
        public readonly IPessoaNegocio PessoaNegocio;
        public readonly IRepositorioPessoa Repositorio;

        public PessoaNegocioTeste()
        {
            RepositorioPessoa repositorio = new RepositorioPessoa(new DesafioContext(TesteConfiguracao.GetDbContextOptions()));
            this.PessoaNegocio = new PessoaNegocio(repositorio);
            this.Repositorio = repositorio;
        }

        [Fact(DisplayName = "Cadastrar uma pessoa com telefone e endereco com sucesso")]
        public void CriarPessoaCompletaComSucesso()
        {
            PessoaDTO pessoaDTO = new PessoaDTO()
            {
                Nome = "Anderson Cassiano",
                Cpf = "449.550.168-20",
                DataNascimento = new DateTime(1996, 4, 12),
                Enderecos = new List<EnderecoDTO>()
                {
                    new EnderecoDTO()
                    {
                        Complemento = "Com complemento",
                        Numero = "123A",
                        Bairro = "Bairro Top",
                        Cep = "12.345-987",
                        Logradouro = "The Kings",
                        Tipo = "Avenida"
                    }
                },
                Telefones = new List<TelefoneDTO>()
                {
                    new TelefoneDTO()
                    {
                        Numero = "(99) 12345-6789",
                        Operadora = "Tim"
                    }
                }
            };

            pessoaDTO = this.PessoaNegocio.Adicionar(pessoaDTO);

            pessoaDTO.Should().NotBeNull();
            pessoaDTO.Id.Should().BePositive();
            pessoaDTO.Nome.Should().Be("Anderson Cassiano");
            pessoaDTO.Cpf.Should().Be("44955016820");
            pessoaDTO.DataNascimento.Should().Be(new DateTime(1996, 4, 12));
            pessoaDTO.Enderecos.Count().Should().BePositive();
            pessoaDTO.Telefones.Count().Should().BePositive();
        }

        [Fact(DisplayName = "Cadastrar uma pessoa com sucesso")]
        public void CriarPessoaComSucesso()
        {
            PessoaDTO pessoaDTO = new PessoaDTO()
            {
                Nome = "Anderson Cassiano",
                Cpf = "449.550.168-20",
                DataNascimento = new DateTime(1996, 4, 12)
            };

            pessoaDTO = this.PessoaNegocio.Adicionar(pessoaDTO);

            pessoaDTO.Should().NotBeNull();
            pessoaDTO.Id.Should().BePositive();
            pessoaDTO.Nome.Should().Be("Anderson Cassiano");
            pessoaDTO.Cpf.Should().Be("44955016820");
            pessoaDTO.DataNascimento.Should().Be(new DateTime(1996, 4, 12));
        }

        [Fact(DisplayName = "Consultar uma pessoa com sucesso")]
        public void ConsultarPessoaComSucesso()
        {
            PessoaDTO pessoaDTO = null;

            PessoaDTO pessoaAux = this.PessoaNegocio.Consultar().FirstOrDefault();

            if(pessoaAux != null && pessoaAux.Id.HasValue)
                pessoaDTO = this.PessoaNegocio.Consultar(pessoaAux.Id.Value);

            pessoaDTO.Should().NotBeNull();
            pessoaDTO.Nome.Should().Be("Anderson Cassiano");
            pessoaDTO.Cpf.Should().Be("44955016820");
            pessoaDTO.DataNascimento.Should().Be(new DateTime(1996, 4, 12));
        }

        [Fact(DisplayName = "Consultar pessoas com sucesso")]
        public void ConsultarPessoasComSucesso()
        {
            IEnumerable<PessoaDTO> pessoaDTO = this.PessoaNegocio.Consultar();
            pessoaDTO.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Remover uma pessoa com sucesso")]
        public void RemoverPessoaComSucesso()
        {
            bool removido = false;
            var pessoa = this.Repositorio.Read().FirstOrDefault(x => !x.Enderecos.Any());
            if(pessoa != null)
                removido = this.PessoaNegocio.Remover(pessoa.Id);

            removido.Should().BeTrue();
        }
    }
}

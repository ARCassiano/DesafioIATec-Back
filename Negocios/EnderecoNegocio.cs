using System;
using System.Collections.Generic;
using System.Linq;
using Dados.DTO;
using Dados.Entidades;
using Dados.Repositorio;
using Dados.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;
using Negocios.Interface;

namespace Negocios
{
    public class EnderecoNegocio : IEnderecoNegocio
    {
        private readonly IRepositorioPessoa Repositorio;

        public EnderecoNegocio(IRepositorioPessoa repositorio)
        {
            this.Repositorio = repositorio;
        }

        public EnderecoDTO Adicionar(long idPessoa, EnderecoDTO enderecoDto)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);

            Endereco endereco = new Endereco(
                enderecoDto.Tipo,
                enderecoDto.Logradouro,
                enderecoDto.Numero,
                enderecoDto.Cep,
                enderecoDto.Bairro,
                enderecoDto.Complemento);

            pessoa.Enderecos.Add(endereco);
            pessoa = this.Repositorio.Update<Pessoa>(pessoa);

            return this.ReturnDto(endereco);
        }

        public EnderecoDTO Atualizar(long idPessoa, long id, EnderecoDTO enderecoDto)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);
            Endereco endereco = pessoa.Enderecos.FirstOrDefault(x => x.Id == id);

            if (endereco == null)
                throw new Exception("Nenhum registro encontrado");

            endereco.SetTipo(enderecoDto.Tipo);
            endereco.SetLogradouro(enderecoDto.Logradouro);
            endereco.SetNumero(enderecoDto.Numero);
            endereco.SetCep(enderecoDto.Cep);
            endereco.SetBairro(enderecoDto.Bairro);
            endereco.Complemento = enderecoDto.Complemento;
            endereco = this.Repositorio.Update<Endereco>(endereco);

            return this.ReturnDto(endereco);
        }

        public IEnumerable<EnderecoDTO> Consultar(long idPessoa) => this.Repositorio.ReadById(idPessoa)?.Enderecos.Select(x => this.ReturnDto(x));

        public EnderecoDTO Consultar(long idPessoa, long id)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);
            Endereco endereco = pessoa.Enderecos.FirstOrDefault(x => x.Id == id);

            if (endereco == null)
                throw new Exception("Nenhum registro encontrado");

            return this.ReturnDto(endereco);
        }

        public bool Remover(long idPessoa, long id)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);
            Endereco endereco = pessoa.Enderecos.FirstOrDefault(x => x.Id == id);

            if (endereco == null)
                throw new Exception("Nenhum registro encontrado");

            this.Repositorio.Delete<Endereco>(endereco);

            return true;
        }

        private EnderecoDTO ReturnDto(Endereco endereco)
        {
            return new EnderecoDTO()
            {
                Id = endereco.Id,
                Tipo = endereco.Tipo,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Cep = endereco.Cep,
                Bairro = endereco.Bairro,
                Complemento = endereco.Complemento
            };
        }

        private Pessoa ValidarPessoa(long idPessoa)
        {
            Pessoa pessoa = this.Repositorio.ReadById(idPessoa, true);
            if (pessoa == null)
                throw new Exception("Nenhum(a) pessoa encontrado(a)");
            return pessoa;
        }
    }
}

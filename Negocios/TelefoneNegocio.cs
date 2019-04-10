using System;
using System.Collections.Generic;
using System.Linq;
using Dados.DTO;
using Dados.Entidades;
using Dados.Repositorio.Interface;
using Negocios.Interface;

namespace Negocios
{
    public class TelefoneNegocio : ITelefoneNegocio
    {
        private readonly IRepositorioPessoa Repositorio;

        public TelefoneNegocio(IRepositorioPessoa repositorio)
        {
            this.Repositorio = repositorio;
        }

        public TelefoneDTO Adicionar(long idPessoa, TelefoneDTO telefoneDto)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);

            Telefone telefone = new Telefone(
                telefoneDto.Numero,
                telefoneDto.Operadora,
                telefoneDto.Ddi);

            pessoa.Telefones.Add(telefone);
            pessoa = this.Repositorio.Update<Pessoa>(pessoa);

            return this.ReturnDto(telefone);
        }

        public TelefoneDTO Atualizar(long idPessoa, long id, TelefoneDTO telefoneDto)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);
            Telefone telefone = pessoa.Telefones.FirstOrDefault(x => x.Id == id);

            if (telefone == null)
                throw new Exception("Nenhum registro encontrado");

            telefone.Operadora = telefoneDto.Operadora;
            telefone.Ddi = telefoneDto.Ddi;
            telefone.SetNumero(telefoneDto.Numero);
            
            telefone = this.Repositorio.Update<Telefone>(telefone);

            return this.ReturnDto(telefone);
        }

        public IEnumerable<TelefoneDTO> Consultar(long idPessoa) => this.Repositorio.ReadById(idPessoa)?.Telefones.Select(x => this.ReturnDto(x));

        public TelefoneDTO Consultar(long idPessoa, long id)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);
            Telefone telefone = pessoa.Telefones.FirstOrDefault(x => x.Id == id);

            if (telefone == null)
                throw new Exception("Nenhum registro encontrado");

            return this.ReturnDto(telefone);
        }

        public bool Remover(long idPessoa, long id)
        {
            Pessoa pessoa = this.ValidarPessoa(idPessoa);
            Telefone telefone = pessoa.Telefones.FirstOrDefault(x => x.Id == id);

            if (telefone == null)
                throw new Exception("Nenhum registro encontrado");

            this.Repositorio.Delete<Telefone>(telefone);

            return true;
        }

        private Pessoa ValidarPessoa(long idPessoa)
        {
            Pessoa pessoa = this.Repositorio.ReadById(idPessoa, true);
            if (pessoa == null)
                throw new Exception("Nenhum(a) pessoa encontrado(a)");
            return pessoa;
        }

        private TelefoneDTO ReturnDto(Telefone telefone)
        {
            return new TelefoneDTO()
            {
                Id = telefone.Id,
                Ddi = telefone.Ddi,
                Operadora = telefone.Operadora,
                Numero = telefone.Numero
            };
        }
    }
}

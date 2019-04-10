using Dados.DTO;
using Dados.Entidades;
using Dados.Repositorio;
using Dados.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;
using Negocios.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocios
{
    public class PessoaNegocio : IPessoaNegocio
    {
        private readonly IRepositorioPessoa Repositorio;

        public PessoaNegocio(IRepositorioPessoa repositorio)
        {
            this.Repositorio = repositorio;
        }

        public PessoaDTO Adicionar(PessoaDTO pessoaDto)
        {
            pessoaDto.DataNascimento = pessoaDto.DataNascimento ?? DateTime.MinValue;
            List<Endereco> enderecos = this.CriarListEnderecos(pessoaDto.Enderecos);
            List<Telefone> telefones = this.CriarListTelefones(pessoaDto.Telefones);

            Pessoa pessoa = new Pessoa(pessoaDto.Nome, pessoaDto.DataNascimento.Value, pessoaDto.Cpf, enderecos, telefones);

            pessoa = this.Repositorio.Create<Pessoa>(pessoa);
            return ReturnDto(pessoa);
        }

        public PessoaDTO Atualizar(long id, PessoaDTO pessoaDto)
        {

            Pessoa pessoa = this.Repositorio.ReadById(id, true);

            if (pessoa == null)
                throw new Exception("Nenhum registro encontrado");

            IEnumerable<EnderecoDTO> enderecoDTOs = this.FiltrarEnderecosASeremInclusos(pessoaDto);
            IEnumerable<TelefoneDTO> telefoneDTOs = this.FiltrarTelefonesASeremInclusos(pessoaDto);
            List<Endereco> enderecos = this.CriarListEnderecos(enderecoDTOs);
            List<Telefone> telefones = this.CriarListTelefones(telefoneDTOs);

            List<Endereco> enderecosASeremRemovidos = this.RemoverEEditarEnderecos(pessoaDto, pessoa);
            List<Telefone> telefonessASeremRemovidos = this.RemoverEEditarTelefones(pessoaDto, pessoa);

            pessoa.SetNome(pessoaDto.Nome);
            pessoa.SetDataNascimento(pessoaDto.DataNascimento.Value);
            pessoa.SetCpf(pessoaDto.Cpf);

            pessoa.Telefones.AddRange(telefones);
            telefonessASeremRemovidos.ForEach(x => pessoa.Telefones.Remove(x));

            enderecosASeremRemovidos.ForEach(x => pessoa.Enderecos.Remove(x));
            pessoa.Enderecos.AddRange(enderecos);

            pessoa = this.Repositorio.Update<Pessoa>(pessoa);

            return this.ReturnDto(pessoa);
        }

        public IEnumerable<PessoaDTO> Consultar() => this.Repositorio.Read().Select(x => this.ReturnDto(x));

        public PessoaDTO Consultar(long id)
        {
            Pessoa pessoa = this.Repositorio.ReadById(id, true);

            if (pessoa == null)
                throw new Exception("Nenhum registro encontrado");

            return this.ReturnDto(pessoa);
        }

        public bool Remover(long id)
        {
            Pessoa pessoa = this.Repositorio.ReadById(id, true);
            if (pessoa == null)
                throw new Exception("Nenhum registro encontrado");

            this.Repositorio.Delete<Pessoa>(pessoa);

            return true;
        }

        //private Municipio ValidarMunicipio(EnderecoDTO enderecoDto)
        //{
        //    if (!enderecoDto.IdMunicipio.HasValue)
        //        throw new Exception("O campo 'IdMunicipio' é obrigatório");

        //    Municipio municipio = this.Repositorio.Read<Municipio>(true)
        //        .Include(x => x.Uf)
        //        .FirstOrDefault(x => x.Id == enderecoDto.IdMunicipio.Value);

        //    if (municipio == null)
        //        throw new Exception("Nenhum município encontrado");

        //    return municipio;
        //}

        private List<Telefone> CriarListTelefones(IEnumerable<TelefoneDTO> telefoneDTOs)
        {
            List<Telefone> telefones = new List<Telefone>();
            if (telefoneDTOs != null)
                foreach (var telefone in telefoneDTOs)
                    telefones.Add(new Telefone(telefone.Numero, telefone.Operadora, telefone.Ddi));
            return telefones;
        }

        private List<Endereco> CriarListEnderecos(IEnumerable<EnderecoDTO> enderecoDTOs)
        {
            List<Endereco> enderecos = new List<Endereco>();
            if (enderecoDTOs != null)
                foreach (var endereco in enderecoDTOs)
                    enderecos.Add(new Endereco(endereco.Tipo, endereco.Logradouro, endereco.Numero, endereco.Cep, endereco.Bairro, endereco.Complemento));

            return enderecos;
        }

        private PessoaDTO ReturnDto(Pessoa pessoa)
        {
            return new PessoaDTO()
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                DataNascimento = pessoa.DataNascimento,
                Cpf = pessoa.Cpf,
                Telefones = pessoa.Telefones.Select(x => new TelefoneDTO()
                {
                    Id = x.Id,
                    Numero = x.Numero,
                    Ddi = x.Ddi,
                    Operadora = x.Operadora
                }),
                Enderecos = pessoa.Enderecos.Select(endereco => new EnderecoDTO()
                {
                    Id = endereco.Id,
                    Tipo = endereco.Tipo,
                    Logradouro = endereco.Logradouro,
                    Numero = endereco.Numero,
                    Cep = endereco.Cep,
                    Bairro = endereco.Bairro,
                    Complemento = endereco.Complemento
                })
            };
        }

        private IEnumerable<TelefoneDTO> FiltrarTelefonesASeremInclusos(PessoaDTO pessoaDto)
        {
            IEnumerable<TelefoneDTO> telefoneDTOs = new List<TelefoneDTO>();
            if (pessoaDto.Telefones != null)
                telefoneDTOs = pessoaDto.Telefones.Where(x => !x.Id.HasValue);

            return telefoneDTOs;
        }

        private IEnumerable<EnderecoDTO> FiltrarEnderecosASeremInclusos(PessoaDTO pessoaDto)
        {
            IEnumerable<EnderecoDTO> enderecoDTOs = new List<EnderecoDTO>();
            if (pessoaDto.Enderecos != null)
                enderecoDTOs = pessoaDto.Enderecos.Where(x => !x.Id.HasValue);

            return enderecoDTOs;
        }

        private List<Endereco> RemoverEEditarEnderecos(PessoaDTO pessoaDto, Pessoa pessoa)
        {
            List<Endereco> enderecosASeremRemovidos = new List<Endereco>();

            if (pessoaDto.Enderecos != null)
            {
                List<long> idsEndereco = pessoa.Enderecos.Select(x => x.Id).ToList();
                List<long> idsEnderecoAtualizado = pessoaDto.Enderecos.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
                IEnumerable<long> idsEnderecoASeremRemovidos = idsEndereco.Except(idsEnderecoAtualizado);
                enderecosASeremRemovidos = pessoa.Enderecos.Where(x => idsEnderecoASeremRemovidos.Contains(x.Id)).ToList();

                foreach (var enderecoDTO in pessoaDto.Enderecos)
                {
                    Endereco endereco = pessoa.Enderecos.FirstOrDefault(x => x.Id == enderecoDTO.Id);

                    if (endereco != null)
                    {
                        endereco.SetBairro(enderecoDTO.Bairro);
                        endereco.SetCep(enderecoDTO.Cep);
                        endereco.SetLogradouro(enderecoDTO.Logradouro);
                        endereco.SetNumero(enderecoDTO.Numero);
                        endereco.SetTipo(enderecoDTO.Tipo);
                        endereco.Complemento = enderecoDTO.Complemento;
                    }
                }

            }
            return enderecosASeremRemovidos;
        }

        private List<Telefone> RemoverEEditarTelefones(PessoaDTO pessoaDto, Pessoa pessoa)
        {
            List<Telefone> telefonesASeremRemovidos = new List<Telefone>();
            if (pessoaDto.Telefones != null)
            {
                List<long> ids = pessoa.Telefones.Select(x => x.Id).ToList();
                List<long> idsAtualizado = pessoaDto.Telefones.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
                IEnumerable<long> idsASeremRemovidos = ids.Except(idsAtualizado);
                telefonesASeremRemovidos = pessoa.Telefones.Where(x => idsASeremRemovidos.Contains(x.Id)).ToList();

                foreach (var telefoneDTO in pessoaDto.Telefones)
                {
                    Telefone telefone = pessoa.Telefones.FirstOrDefault(x => x.Id == telefoneDTO.Id);

                    if (telefone != null)
                    {
                        telefone.SetNumero(telefoneDTO.Numero);
                        telefone.Ddi = telefoneDTO.Ddi;
                        telefone.Operadora = telefoneDTO.Operadora;
                    }
                }
            }

            return telefonesASeremRemovidos;
        }

    }
}

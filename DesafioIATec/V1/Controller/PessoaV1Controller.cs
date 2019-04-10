using Dados.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Negocios.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesafioIATec.V1.Controller
{
    [Route("api/v1/pessoas/")]
    [ApiController]
    public class PessoaV1Controller: ControllerBase
    {
        private readonly IPessoaNegocio PessoaNegocio;
        private readonly IEnderecoNegocio EnderecoNegocio;
        private readonly ITelefoneNegocio TelefoneNegocio;

        public PessoaV1Controller(IPessoaNegocio pessoaNegocio, IEnderecoNegocio enderecoNegocio, ITelefoneNegocio telefoneNegocio)
        {
            this.PessoaNegocio = pessoaNegocio;
            this.EnderecoNegocio = enderecoNegocio;
            this.TelefoneNegocio = telefoneNegocio;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PessoaDTO>> GetAll()
        {

            IEnumerable<PessoaDTO> retorno = this.PessoaNegocio.Consultar();

            return new OkObjectResult(retorno);
        }

        [HttpGet("{id}")]
        public ActionResult<PessoaDTO> Get(long id)
        {

            PessoaDTO retorno = this.PessoaNegocio.Consultar(id);

            if (retorno == null)
                return new NotFoundResult();

            return new OkObjectResult(retorno);
        }

        [HttpGet("{id}/enderecos")]
        public ActionResult<IEnumerable<EnderecoDTO>> GetAllEndereco(long id)
        {

            IEnumerable<EnderecoDTO> retorno = this.EnderecoNegocio.Consultar(id);

            if (retorno == null)
                return new NotFoundResult();

            return new OkObjectResult(retorno);
        }

        [HttpGet("{id}/enderecos/{idEndereco}")]
        public ActionResult<EnderecoDTO> GetEndereco(long id, long idEndereco)
        {

            EnderecoDTO retorno = this.EnderecoNegocio.Consultar(id, idEndereco);

            if (retorno == null)
                return new NotFoundResult();

            return new OkObjectResult(retorno);
        }

        [HttpGet("{id}/telefones")]
        public ActionResult<IEnumerable<TelefoneDTO>> GetAllTelefone(long id)
        {

            IEnumerable<TelefoneDTO> retorno = this.TelefoneNegocio.Consultar(id);

            if (retorno == null)
                return new NotFoundResult();

            return new OkObjectResult(retorno);
        }

        [HttpGet("{id}/telefones/{idTelefone}")]
        public ActionResult<TelefoneDTO> GetTelefone(long id, long idTelefone)
        {

            TelefoneDTO retorno = this.TelefoneNegocio.Consultar(id, idTelefone);

            if (retorno == null)
                return new NotFoundResult();

            return new OkObjectResult(retorno);
        }

        [HttpPost]
        public ActionResult<PessoaDTO> Post(PessoaDTO pessoa)
        {
            PessoaDTO retorno = this.PessoaNegocio.Adicionar(pessoa);

            return CreatedAtAction(nameof(Get), new { id = retorno.Id }, retorno);
        }

        [HttpPost("{id}/enderecos")]
        public ActionResult<EnderecoDTO> PostEndereco(long id, EnderecoDTO endereco)
        {

            EnderecoDTO retorno = this.EnderecoNegocio.Adicionar(id, endereco);

            if (retorno == null)
                return new BadRequestResult();

            return new CreatedAtActionResult(nameof(Get), nameof(PessoaV1Controller), new { id = retorno.Id }, retorno);
        }

        [HttpPost("{id}/telefones")]
        public ActionResult<TelefoneDTO> PostTelefone(long id, TelefoneDTO telefone)
        {

            TelefoneDTO retorno = this.TelefoneNegocio.Adicionar(id, telefone);

            if (retorno == null)
                return new BadRequestResult();

            return new CreatedAtActionResult(nameof(Get), nameof(PessoaV1Controller), new { id = retorno.Id }, retorno);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, PessoaDTO pessoa)
        {
            PessoaDTO objeto = this.PessoaNegocio.Atualizar(id, pessoa);

            if (objeto == null)
                return new BadRequestResult();

            return new NoContentResult();
        }

        [HttpPut("{id}/enderecos/{idEndereco}")]
        public IActionResult PutEndereco(long id, long idEndereco, EnderecoDTO endereco)
        {
            EnderecoDTO retorno = this.EnderecoNegocio.Atualizar(id, idEndereco, endereco);

            if (retorno == null)
                return new BadRequestResult();

            return new NoContentResult();
        }

        [HttpPut("{id}/telefones/{idTelefone}")]
        public IActionResult PutTelefone(long id, long idTelefone, TelefoneDTO telefone)
        {
            TelefoneDTO retorno = this.TelefoneNegocio.Atualizar(id, idTelefone, telefone);

            if (retorno == null)
                return new BadRequestResult();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            bool removido = this.PessoaNegocio.Remover(id);

            if (!removido)
                return new BadRequestResult();

            return new NoContentResult();
        }

        [HttpDelete("{id}/enderecos/{idEndereco}")]
        public ActionResult DeleteEndereco(long id, long idEndereco)
        {
            bool removido = this.EnderecoNegocio.Remover(id, idEndereco);

            if (!removido)
                return new BadRequestResult();

            return new NoContentResult();
        }

        [HttpDelete("{id}/telefones/{idTelefone}")]
        public ActionResult DeleteTelefone(long id, long idTelefone)
        {
            bool removido = this.TelefoneNegocio.Remover(id, idTelefone);

            if (!removido)
                return new BadRequestResult();

            return new NoContentResult();
        }
    }
}

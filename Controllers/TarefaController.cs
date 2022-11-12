using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using trilha_net_api_desafio.Interfaces;
using trilha_net_api_desafio.Validator;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private IValidator<Tarefa> _validator;
        private readonly ITarefasRepository _repository;

        public TarefaController(IValidator<Tarefa> validator, ITarefasRepository repository)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return NotFound();
            }

            var tarefa = _repository.ObterPorId(id);

            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            return Ok(_repository.ObterTodos());
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            if (string.IsNullOrEmpty(titulo))
            {
                return NotFound();
            }
            
            var tarefa = _repository.ObterPorTitulo(titulo);
            
            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _repository.ObterPorData(data);

            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _repository.ObterPorStatus(status);

            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            List<string> ValidationMessages = new List<string>();
            var validacao = await _validator.ValidateAsync(tarefa);
            var response = new ResponseModel();
            if (!validacao.IsValid)
            {
                foreach (var error in validacao.Errors)
                {
                    ValidationMessages.Add(error.ErrorMessage);
                }
               response.ValidationMessages = ValidationMessages;
            }
            if(validacao.Errors.Count > 0)
            {
                return BadRequest(ValidationMessages);
            }

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _repository.Criar(tarefa);
            
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            List<string> ValidationMessages = new List<string>();
            var validacao = await _validator.ValidateAsync(tarefa);
            var response = new ResponseModel();
            if (!validacao.IsValid)
            {
                foreach (var error in validacao.Errors)
                {
                    ValidationMessages.Add(error.ErrorMessage);
                }
               response.ValidationMessages = ValidationMessages;
            }
            if(validacao.Errors.Count > 0)
            {
                return BadRequest(ValidationMessages);
            }

            var tarefaBanco = _repository.ObterPorId(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _repository.Atualizar(tarefaBanco);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefa = _repository.ObterPorId(id);

            if (tarefa == null)
                return NotFound();

            _repository.Deletar(tarefa);
            _repository.Save();

            return NoContent();
        }
    }
}

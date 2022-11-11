using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
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
        private readonly OrganizadorContext _context;

        public TarefaController(IValidator<Tarefa> validator, OrganizadorContext context)
        {
            _context = context;
            _validator = validator;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return NotFound();
            }
            var resultado = _context.Tarefas.FirstOrDefault(x => x.Id == id);
            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            return Ok(_context.Tarefas.ToList());
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            if (string.IsNullOrEmpty(titulo))
            {
                return NotFound();
            }
            var resultado = _context.Tarefas.FirstOrDefault(x => x.Titulo.Equals(titulo));
            return Ok(resultado);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefaBanco = _context.Tarefas.FirstOrDefault(x => x.Status.Equals(status));
            return Ok(tarefaBanco);
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

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

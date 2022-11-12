using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using trilha_net_api_desafio.Interfaces;
using TrilhaApiDesafio.Context;

namespace TrilhaApiDesafio.Models
{
    public class Tarefa : ITarefasRepository
    {
        private readonly ILogger<Tarefa>  _logger;
        private readonly OrganizadorContext _context;
        private bool disposed;
        private bool disposing;

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public EnumStatusTarefa Status { get; set; }

        public Tarefa(OrganizadorContext context, ILogger<Tarefa> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Tarefa()
        {
            
        }

        public void Atualizar(Tarefa tarefa)
        {
            _logger.LogInformation($"Atualizando tarefa, ", 
            DateTime.UtcNow.ToLongTimeString());
            _context.Tarefas.Update(tarefa);
            Save();
        }

        public void Criar(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            Save();
        }

        public void Deletar(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            Save();
        }

        public IEnumerable<Tarefa> ObterPorData(DateTime data)
        {
            return (IEnumerable<Tarefa>)_context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();
        }

        public Tarefa ObterPorId(int id)
        {
            _logger.LogInformation($"Pesquisando tarefas por Id:{id} ", 
            DateTime.UtcNow.ToLongTimeString());
            return _context.Tarefas.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Tarefa> ObterPorStatus(EnumStatusTarefa status)
        {
            return (IEnumerable<Tarefa>)_context.Tarefas.Where(x => x.Status.Equals(status));
        }

        public Tarefa ObterPorTitulo(string titulo)
        {
            return _context.Tarefas.FirstOrDefault(x => x.Titulo.Equals(titulo));
        }

        public IEnumerable<Tarefa> ObterTodos()
        {
            return _context.Tarefas.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool v)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}

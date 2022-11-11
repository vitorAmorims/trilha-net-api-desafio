using TrilhaApiDesafio.Models;

namespace trilha_net_api_desafio.Interfaces
{
    public interface ITarefasRepository: IDisposable
    {
        IEnumerable<Tarefa> ObterTodos();
        Tarefa ObterPorId(int id);
        Tarefa ObterPorTitulo(string titulo);
        IEnumerable<Tarefa> ObterPorData(DateTime data);
        IEnumerable<Tarefa> ObterPorStatus(EnumStatusTarefa status);
        void Criar(Tarefa tarefa);
        void Atualizar(Tarefa tarefa);
        void Deletar(Tarefa tarefa);
        void Save();


    }
}

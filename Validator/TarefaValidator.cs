using FluentValidation;
using TrilhaApiDesafio.Models;

namespace trilha_net_api_desafio.Validator
{
    public class TarefaValidator : AbstractValidator<Tarefa>
    {
        public TarefaValidator()
        {
            RuleFor(x => x.Titulo).Length(0, 50);
            RuleFor(x => x.Titulo).NotNull();
            RuleFor(x => x.Descricao).Length(0, 50);
            RuleFor(x => x.Descricao).NotNull();
            RuleFor(x => x.Data).NotNull();
            RuleFor(x => x.Status).NotNull();
        }
    }
}

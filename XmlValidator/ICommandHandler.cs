using System.Threading;
using System.Threading.Tasks;

namespace XmlValidator
{
    public interface ICommandHandlerAsync<in TCommand, TResultDto>
    {
        Task<TResultDto> Execute(TCommand command);
    }

    public interface ICommandHandlerAsync<in TCommand>
    {
        CancellationTokenSource CancellationTokenSource { get; set; }
        Task Execute(TCommand command);
    }

    public interface ICommandHandler<in TCommand>
    {
        void Execute(TCommand command);
    }

    public interface ICommandHandler<in TCommand, out TResultDto>
    {
        TResultDto Execute(TCommand command);
    }
}
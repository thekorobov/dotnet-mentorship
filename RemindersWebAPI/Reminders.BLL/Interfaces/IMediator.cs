namespace Reminders.BLL.Interfaces;

public interface IMediator
{
    Task SendCommandAsync<TCommand>(TCommand command);
    Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command);
    Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query);
}
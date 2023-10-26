namespace Reminders.Interfaces
{
    public interface ICommand<TResult>
    {
        Task<TResult> ExecuteAsync();
    }
}
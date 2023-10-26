namespace TicketFlow.Application.Common.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string name)
        : base($"Entity <{name}> not found.") { }
    
    public EntityNotFoundException(string name, object? key)
        : base($"Entity <{name}> ({key}) not found.") { }
}
namespace Ordering.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string Name, object Key)
        : base($"Entity \"{Name}\" ({Key}) was not found.") { }
}

namespace Utilities.CustomExceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type type, int id) : base(GetMessage(type, id))
    {
    }
    
    public EntityNotFoundException(Type type, int id, Exception inner) : base(GetMessage(type, id), inner)
    {
    }

    private static string GetMessage(Type type, int id) => $"Entity {type.Name} with id {id} not found";
}
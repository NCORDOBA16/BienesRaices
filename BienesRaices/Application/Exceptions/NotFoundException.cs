namespace Application.Exceptions
{
    public class NotFoundException(string name, object key) : Exception($"Entity \"{name}\" ({key})  no fue encontrado")
    {
    }
}

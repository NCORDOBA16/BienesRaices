namespace Application.Exceptions
{
    public class RecordAlreadyExistException(string message = "Este registro ya existe") : Exception(message)
    {
    }
}

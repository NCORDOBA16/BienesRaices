namespace Application.Exceptions
{
    public class NullFileException(string message = "La solicitud requiere que seleccione un archivo") : Exception(message)
    {
    }
}

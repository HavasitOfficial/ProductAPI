namespace Product.Application.Helpers
{
    public class NotFoundException(string message) : Exception(message);
}

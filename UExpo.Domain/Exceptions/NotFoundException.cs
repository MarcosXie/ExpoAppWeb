namespace UExpo.Domain.Exceptions;

public class NotFoundException(string item) : BaseException($"{item} not found!", 404);
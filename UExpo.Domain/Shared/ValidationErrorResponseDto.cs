namespace UExpo.Domain.Shared;

public class ValidationErrorResponseDto
{
    public bool IsError { get; set; }
    public string? Message { get; set; }
}

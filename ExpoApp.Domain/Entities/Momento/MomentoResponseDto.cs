namespace ExpoApp.Domain.Entities.Momento;

public class MomentoResponseDto
{
	public required Guid Id { get; set; }
	public required string Value { get; set; }
	public required string Comment { get; set; }
	public required int Order { get; set; }
	public required DateTime CreatedDate { get; set; }
}
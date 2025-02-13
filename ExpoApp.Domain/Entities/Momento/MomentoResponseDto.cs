namespace ExpoApp.Domain.Entities.Momento;

public class MomentoResponseDto
{
	public Guid Id { get; set; }
	public string Value { get; set; }
	public string Comment { get; set; }
	public DateTime CreatedDate { get; set; }
}
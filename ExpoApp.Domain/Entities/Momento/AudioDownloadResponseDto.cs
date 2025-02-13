namespace ExpoApp.Domain.Entities.Momento;

public class AudioDownloadResponseDto
{
	public MemoryStream ZipFile { get; set; }
	public List<MomentoResponseDto> AudioResponse { get; set; }
}
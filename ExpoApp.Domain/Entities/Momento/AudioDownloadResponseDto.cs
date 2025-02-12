namespace ExpoApp.Domain.Entities.Momento;

public class AudioDownloadResponseDto
{
	public MemoryStream ZipFile { get; set; }
	public List<AudioResponseDto> AudioResponse { get; set; }
}
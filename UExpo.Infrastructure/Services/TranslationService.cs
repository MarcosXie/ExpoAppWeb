using Amazon;
using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.Extensions.Configuration;
using UExpo.Domain.Translation;
using UExpo.Infrastructure.Utils;

namespace UExpo.Infrastructure.Services;

public class TranslationService : ITranslationService
{
    private readonly AmazonTranslateClient _tranlateClient;

    public TranslationService(IConfiguration config)
    {
        _tranlateClient = new AmazonTranslateClient(
            AwsUtils.GetAwsCredentials(config),
            RegionEndpoint.GetBySystemName(config["AWS:Region"]));
    }

    public async Task<string> TranslateText(string text, string srcLang, string trgLang)
    {
        if (srcLang.Equals(trgLang)) return text;

        TranslateTextRequest translateRequest = new TranslateTextRequest
        {
            SourceLanguageCode = string.IsNullOrEmpty(srcLang) ? "en" : srcLang,
            TargetLanguageCode = string.IsNullOrEmpty(trgLang) ? "en" : trgLang,
            Text = text
        };

        try
        {
            TranslateTextResponse translatedResponse = await _tranlateClient.TranslateTextAsync(translateRequest);
            return translatedResponse.TranslatedText;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error translating text: ", ex.ToString());
            throw;
        }
    }
}

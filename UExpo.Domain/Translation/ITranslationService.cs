namespace UExpo.Domain.Translation;

public interface ITranslationService
{
    Task<string> TranslateText(string text, string srcLang, string trgLang);
}

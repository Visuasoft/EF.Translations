namespace EF.Translations;

public interface ITranslatedToString
{
    string ToString(string language = Languages.Default);
}
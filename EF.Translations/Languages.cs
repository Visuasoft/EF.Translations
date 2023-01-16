namespace EF.Translations;

public static class Languages
{
    public const string Dutch = "nl-be";
    public const string English = "en-gb";
    public const string French = "fr-be";
    public const string German = "de-de";
    public const string Default = "nl-be";

    public static bool MatchAll(this IEnumerable<string> languages)
    {
        return languages.All(l =>
            l.Equals(Dutch, System.StringComparison.InvariantCultureIgnoreCase) ||
            l.Equals(French, System.StringComparison.InvariantCultureIgnoreCase) ||
            l.Equals(German, System.StringComparison.InvariantCultureIgnoreCase) ||
            l.Equals(English, System.StringComparison.InvariantCultureIgnoreCase)
        );
    }
}
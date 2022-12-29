namespace Model
{
    public class Language
    {
        public LanguageCode Code { get; set; }

        public string Name { get; set; } 

        public Dictionary<TranslationKey, string> Translations { get; set;}
    }
}
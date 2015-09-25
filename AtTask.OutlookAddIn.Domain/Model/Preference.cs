namespace AtTask.OutlookAddIn.Domain.Model
{
    public abstract class Preference : StreamBase
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
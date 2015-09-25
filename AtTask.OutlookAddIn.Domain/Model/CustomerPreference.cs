namespace AtTask.OutlookAddIn.Domain.Model
{
    public class CustomerPreference : Preference
    {
        public override string GetObjectType() { return "customerpref"; }
    }
}
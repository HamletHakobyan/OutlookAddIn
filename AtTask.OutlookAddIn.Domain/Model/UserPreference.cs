namespace AtTask.OutlookAddIn.Domain.Model
{
    public class UserPreference : Preference
    {
        public override string GetObjectType() { return "userpref"; }
    }
}
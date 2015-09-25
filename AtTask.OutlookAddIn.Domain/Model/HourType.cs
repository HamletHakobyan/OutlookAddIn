namespace AtTask.OutlookAddIn.Domain.Model
{
    public class HourType : NamedEntityBase
    {
        #region Fields

        private bool? isActive;
        private Scope scope;

        #endregion Fields

        #region Properties

        public bool? IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Scope Scope
        {
            get { return scope; }
            set { scope = value; }
        }

        #endregion Properties

        public override string GetObjectType() { return "hourType"; }
    }
}
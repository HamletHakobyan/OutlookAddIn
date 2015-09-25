namespace AtTask.OutlookAddIn.Domain.Model
{
    /// <summary>
    /// Base class for those objects which have Name property
    /// </summary>
    public abstract class NamedEntityBase : EntityBase
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}: \"{1}\"]", this.GetType().Name, Name);
        }
    }
}
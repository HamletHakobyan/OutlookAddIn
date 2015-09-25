using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    /// <summary>
    /// Base class for all Stream API Objects.
    /// Any API object should override ObjectType property
    /// </summary>
    [Serializable()]
    public abstract class StreamBase : ICloneable
    {
        public abstract string GetObjectType();

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
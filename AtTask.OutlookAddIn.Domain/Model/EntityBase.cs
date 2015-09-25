using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    /// <summary>
    /// Base class for all those Stream API objects which have ID and ObjCode property
    /// </summary>
    public abstract class EntityBase : StreamBase
    {
        public string ID { get; set; }

        public string ObjCode { get; set; }

        //-- replaced with Permissions since API v4.0
        //public List<string> AllowedActions { get; set; }

        public Permissions Permissions { get; set; }

        /// <summary>
        /// Stores names of fields that will be null
        /// </summary>
        [JsonIgnore]
        public List<string> NullableFields { get; private set; }

        /// <summary>
        /// Stores Custom Data fields with appropriate values
        /// </summary>
        //[JsonIgnore]
        //public Dictionary<string, object> CustomDataFields { get; private set; }

        /// <summary>
        /// Adds a field to NullableFields. A property with that name must exist and must be null assignable, otherwise parameter will not be added to the list.
        /// </summary>
        /// <param name="fieldName"></param>
        public void AddNullableField(string fieldName)
        {
            //-- Ignore null or empty fields
            if (string.IsNullOrEmpty(fieldName))
            {
                return;
            }

            if (NullableFields == null)
            {
                NullableFields = new List<string>();
            }

            //-- No duplicate entries
            if (!NullableFields.Contains<string>(fieldName, StringComparer.InvariantCultureIgnoreCase))
            {
                //-- A property with specified name must exist and that type can accept null value
                PropertyInfo propInfo = FindPropertyInfo(fieldName);
                if (propInfo != null && (!propInfo.PropertyType.IsValueType || Nullable.GetUnderlyingType(propInfo.PropertyType) != null))
                {
                    propInfo.SetValue(this, null, null);
                    NullableFields.Add(fieldName);
                }
            }
        }

        protected PropertyInfo FindPropertyInfo(string fieldName)
        {
            PropertyInfo ret = null;
            foreach (PropertyInfo info in this.GetType().GetProperties())
            {
                if (info.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase))
                {
                    ret = info;
                    break;
                }
            }
            return ret;
        }

        //public void AddCustomDataField(string paramName, object value)
        //{
        //    //-- Ignore null or empty Custom fields
        //    if (string.IsNullOrEmpty(paramName))
        //    {
        //        return;
        //    }
        //    if (CustomDataFields == null)
        //    {
        //        CustomDataFields = new Dictionary<string, object>();
        //    }
        //    //-- Check if ParameterValues field exist, make it null
        //    PropertyInfo info = this.GetType().GetProperty("ParameterValues");
        //    if (info != null)
        //    {
        //        info.SetValue(this, null, null);
        //    }

        //    CustomDataFields.Add(paramName, value);
        //}
        protected bool Equals(EntityBase other)
        {
            if (ID == null || other.ID == null)
            {
                return false;
            }

            return string.Equals(ID, other.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EntityBase) obj);
        }

        public override int GetHashCode()
        {
            return (ID != null ? ID.GetHashCode() : 0);
        }

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !Equals(left, right);
        }
    }
}
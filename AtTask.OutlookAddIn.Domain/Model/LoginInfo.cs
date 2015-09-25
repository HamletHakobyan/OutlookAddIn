using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    [Serializable()]
    public class LoginInfo : ICloneable
    {
        public string Cookie { get; set; }

        public Session Session { get; set; }

        private User user;

        public User User
        {
            get { return this.user; }
            set
            {
                //if (value == null || value.ID == null)
                //{
                //    throw new ArgumentException("Null user or null user.ID");
                //}
                //if (SessionAttributes == null || SessionAttributes.UserID == null)
                //{
                //    throw new ArgumentException("SessionAttributes or SessionAttributes.UserID is null: no user can be set");
                //}
                //if (SessionAttributes.UserID != value.ID)
                //{
                //    throw new ArgumentException("SessionAttributes.UserID and User.ID inconsistence");
                //}
                this.user = value;
                //this.loggedIn = true;
            }
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
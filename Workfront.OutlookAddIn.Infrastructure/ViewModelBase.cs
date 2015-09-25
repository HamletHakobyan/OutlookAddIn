using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName=null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}

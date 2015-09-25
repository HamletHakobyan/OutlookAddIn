using System;
using System.Windows.Input;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public abstract class ClosableViewModelBase :ViewModelBase
    {
        private ICommand _closeCommand;

        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => OnRequestClose())); }
        }

        private void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler RequestClose;
    }
}

namespace Workfront.OutlookAddIn.Infrastructure
{
    public abstract class LoadableViewModel : ClosableViewModelBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
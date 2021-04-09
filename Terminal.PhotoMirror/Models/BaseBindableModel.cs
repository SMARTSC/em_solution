namespace EM.Terminal.PhotoMirror.Models
{
    public class BaseBindableModel : System.ComponentModel.INotifyPropertyChanged, System.IDisposable
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T property, T newValue, System.Action listener = null, [System.Runtime.CompilerServices.CallerMemberName]string propertyName = null)
        {
            if (Equals(property, newValue)) return;

            property = newValue;

            RaisePropertyChanged(propertyName);

            listener?.Invoke();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeManagementResources();
                }

                DisposeUnmanagementResources();

                disposedValue = true;
            }
        }

        protected virtual void DisposeManagementResources() { }

        protected virtual void DisposeUnmanagementResources() { }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}
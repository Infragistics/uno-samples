using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FinancialChart
{
    public abstract class ObservableModel : INotifyPropertyChanged
    {
        protected ObservableModel()
        {
            IsPropertyNotifyActive = true;
        }

        #region INotifyPropertyChanged  
        public bool IsPropertyNotifyActive { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool HasPropertyChangedHandler()
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            return handler != null;
        }
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null && IsPropertyNotifyActive)
                handler(sender, e);
        }
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            OnPropertyChanged(this, e);
        }
        protected void OnPropertyChanged(object sender, string propertyName)
        {
            OnPropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected delegate void PropertyChangedDelegate(object sender, string propertyName);

        //WPF
        //public void OnAsyncPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //           new ThreadStart(() =>
        //           {
        //              PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //         }));
        //    }
        //}

        #endregion
    }
}

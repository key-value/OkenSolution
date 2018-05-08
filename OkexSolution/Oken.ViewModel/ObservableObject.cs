using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Oken.ViewModel;

namespace Oken.ViewModel
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return;
            storage = value;
            this.NotifyPropertyChanged(propertyName);
        }
    }
}
public static class PropertyChangedBaseEx
{
    public static void NotifyPropertyChanged<T, TProperty>(this T propertyChangedBase, Expression<Func<T, TProperty>> expression) where T : ObservableObject
    {
        var memberExpression = expression.Body as MemberExpression;
        if (memberExpression != null)
        {
            string propertyName = memberExpression.Member.Name;
            propertyChangedBase.NotifyPropertyChanged(propertyName);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
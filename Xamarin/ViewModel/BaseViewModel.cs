﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CKAN.Xamarin.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel ()
        {
        }

        protected bool SetProperty<T> (ref T field, T value, [CallerMemberName]string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }

            field = value;
            OnPropertyChanged(name);
            return true;
        }

        protected void OnPropertyChanged ([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

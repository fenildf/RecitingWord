using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace MVVM
{
    public class ViewModeBaseDependencyObject : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ProperChange(string ProperName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ProperName));
        }
        public bool SetProperty<T>(ref T filed, T value, string ProperName)
        {
            if (!EqualityComparer<T>.Default.Equals(filed, value))
            {
                filed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ProperName)); 
                return true;
            }
            return false;
        }
    }
    public class ViewModeBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ProperChange(string ProperName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ProperName));
        }
        public bool SetProperty<T>(ref T filed, T value, string ProperName)
        {
            if (!EqualityComparer<T>.Default.Equals(filed, value))
            {
                filed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ProperName));
                return true;
            }
            return false;
        }
    }
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsCanExecute == null ? true : IsCanExecute();
        }

        /// <summary>
        /// 刷新所有 Command的CanExecute
        /// </summary>
        public static void OnAllCanExecuteChanged()
        {
            if (System.Windows.Application.Current.Dispatcher.CheckAccess())
            {
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    CommandManager.InvalidateRequerySuggested();
                }));
            }
        }
        public void Execute(object parameter)
        {
            ExrcuteParame?.Invoke(parameter);
            Exrcute?.Invoke();
        }
        Action Exrcute;
        Action<object> ExrcuteParame;
        Func<bool> IsCanExecute;

        public Command(Action Exrcute, Func<bool> IsCanExecute)
        {
            this.Exrcute = Exrcute;
            this.IsCanExecute = IsCanExecute;
        }
        public Command(Action<object> ExrcuteParame, Func<bool> IsCanExecute)
        {
            this.ExrcuteParame = ExrcuteParame;
            this.IsCanExecute = IsCanExecute;
        }
        public Command(Action Exrcute) : this(Exrcute, null) { }
        public Command(Action<object> Exrcute) : this(Exrcute, null) { }
    }
}

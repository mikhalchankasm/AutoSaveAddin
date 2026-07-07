using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AutoSaveAddin.Lib;

namespace AutoSaveAddin
{
    public partial class MessageBoxT : Window
    {
        public MessageBoxT()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
        }

        private readonly DispatcherTimer _timer;

        private void TimerStart()
        {
            _timer.Interval = TimeSpan.FromSeconds(1d);
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerStop()
        {
            _timer.Stop();
            _timer.Tick -= TimerTick;
        }

        private void Close(bool result)
        {
            DialogResult = result;
            TimerStop();
        }

        #region YesCommand : ICommand - desc

        private ICommand _YesCommand;

        /// <summary>desc</summary>
        public ICommand YesCommand
        {
            get
            {
                return _YesCommand ?? (_YesCommand = new RelayCommand(OnYesCommandExecuted, CanYesCommandExecute));
            }
        }

        private void OnYesCommandExecuted(object p)
        {
            Close(true);
        }

        private bool CanYesCommandExecute(object p)
        {
            return true;
        }

        #endregion

        #region NoCommand : ICommand - desc

        private ICommand _NoCommand;

        /// <summary>desc</summary>
        public ICommand NoCommand
        {
            get
            {
                return _NoCommand ?? (_NoCommand = new RelayCommand(OnNoCommandExecuted, CanNoCommandExecute));
            }
        }

        private void OnNoCommandExecuted(object p)
        {
            Close(false);
        }

        private bool CanNoCommandExecute(object p)
        {
            return true;
        }

        #endregion

        private void MessageBoxT_OnLoaded(object sender, RoutedEventArgs e)
        {
            TimerStart();
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            MessageBoxViewModel vm = DataContext as MessageBoxViewModel;
            vm.Time--;
            
            if (vm.Time <= 0)
            {
                Close(true);
            }
        }
    }
}

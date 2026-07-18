using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AutoSaveAddin.Lib;

namespace AutoSaveAddin
{
    public partial class AutoSaveConfirmationWindow : Window
    {
        public AutoSaveConfirmationWindow()
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

        private ICommand _yesCommand;

        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand(OnYesCommandExecuted));
            }
        }

        private void OnYesCommandExecuted(object p)
        {
            Close(true);
        }

        private ICommand _noCommand;

        public ICommand NoCommand
        {
            get
            {
                return _noCommand ?? (_noCommand = new RelayCommand(OnNoCommandExecuted));
            }
        }

        private void OnNoCommandExecuted(object p)
        {
            Close(false);
        }

        private void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            TimerStart();
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            AutoSaveConfirmationViewModel vm = DataContext as AutoSaveConfirmationViewModel;
            if (vm == null)
                return;

            vm.Time--;
            
            if (vm.Time <= 0)
            {
                Close(vm.TimeoutResult);
            }
        }
    }
}

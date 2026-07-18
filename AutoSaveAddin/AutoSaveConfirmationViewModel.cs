using AutoSaveAddin.Lib;
using AutoSaveAddin.Localization;

namespace AutoSaveAddin
{
    public class AutoSaveConfirmationViewModel : NotifyBase
    {
        private string _title = UiText.ConfirmationTitle;
        private string _message = UiText.ConfirmationMessage;
        private string _details = string.Empty;
        private string _yesText = UiText.YesSaveButton;
        private string _noText = UiText.CancelButton;
        private bool _timeoutResult = true;
        private int _time = 10;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public string Details
        {
            get { return _details; }
            set { SetProperty(ref _details, value); }
        }

        public string YesText
        {
            get { return _yesText; }
            set { SetProperty(ref _yesText, value); }
        }

        public string NoText
        {
            get { return _noText; }
            set { SetProperty(ref _noText, value); }
        }

        public bool TimeoutResult
        {
            get { return _timeoutResult; }
            set { SetProperty(ref _timeoutResult, value); }
        }

        public int Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }
    }
}

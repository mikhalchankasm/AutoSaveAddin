using AutoSaveAddin.Lib;
using AutoSaveAddin.Localization;

namespace AutoSaveAddin
{
    public class MessageBoxViewModel : NotifyBase
    {
        private string _Title = UiText.ConfirmationTitle;
        private string _Message = UiText.ConfirmationMessage;
        private string _Details = string.Empty;
        private string _YesText = UiText.YesSaveButton;
        private string _NoText = UiText.CancelButton;
        private bool _TimeoutResult = true;
        private int _Time = 10;

        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
        }

        public string Details
        {
            get { return _Details; }
            set { SetProperty(ref _Details, value); }
        }

        public string YesText
        {
            get { return _YesText; }
            set { SetProperty(ref _YesText, value); }
        }

        public string NoText
        {
            get { return _NoText; }
            set { SetProperty(ref _NoText, value); }
        }

        public bool TimeoutResult
        {
            get { return _TimeoutResult; }
            set { SetProperty(ref _TimeoutResult, value); }
        }

        public int Time
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value); }
        }
    }
}

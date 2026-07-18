using System;
using AutoSaveAddin.Lib;

namespace AutoSaveAddin.Model
{
    public class Settings : NotifyBase
    {
        private bool _enabled;
        private bool _isNeedRequest;
        private bool _requireConfirmationToSave;
        private bool _unclaimAfterSave;
        private TimeSpan _delay;
        private TimeSpan _closeDelay;

        public bool Enabled
        {
            get { return _enabled; }
            set { SetProperty(ref _enabled, value); }
        }

        public bool IsNeedRequest
        {
            get { return _isNeedRequest; }
            set { SetProperty(ref _isNeedRequest, value); }
        }

        public bool RequireConfirmationToSave
        {
            get { return _requireConfirmationToSave; }
            set { SetProperty(ref _requireConfirmationToSave, value); }
        }

        public bool UnclaimAfterSave
        {
            get { return _unclaimAfterSave; }
            set { SetProperty(ref _unclaimAfterSave, value); }
        }

        public TimeSpan Delay
        {
            get { return _delay; }
            set { SetProperty(ref _delay, value); }
        }

        public TimeSpan CloseDelay
        {
            get { return _closeDelay; }
            set { SetProperty(ref _closeDelay, value); }
        }

        public static Settings GetDefault()
        {
            return new Settings
            {
                Enabled = true,
                IsNeedRequest = false,
                RequireConfirmationToSave = false,
                UnclaimAfterSave = false,
                Delay = TimeSpan.FromHours(2),
                CloseDelay = TimeSpan.FromMinutes(1)
            };
        }
    }
}

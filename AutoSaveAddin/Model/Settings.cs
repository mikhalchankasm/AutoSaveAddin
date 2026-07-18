using System;
using AutoSaveAddin.Lib;

namespace AutoSaveAddin.Model
{
    public class Settings : NotifyBase
    {
        #region Enabled : bool - Активно ли автосохранение

        private bool _Enabled;

        /// <summary>Активно ли автосохранение</summary>
        public bool Enabled
        {
            get { return _Enabled; }
            set { SetProperty(ref _Enabled, value); }
        }

        #endregion
        #region IsNeedRequest : bool - Спрашивать ли перед сохранением

        private bool _IsNeedRequest;

        /// <summary>Спрашивать ли перед сохранением</summary>
        public bool IsNeedRequest
        {
            get { return _IsNeedRequest; }
            set { SetProperty(ref _IsNeedRequest, value); }
        }

        #endregion
        #region RequireConfirmationToSave : bool - Сохранять только после подтверждения

        private bool _RequireConfirmationToSave;

        /// <summary>Сохранять только после подтверждения</summary>
        public bool RequireConfirmationToSave
        {
            get { return _RequireConfirmationToSave; }
            set { SetProperty(ref _RequireConfirmationToSave, value); }
        }

        #endregion
        #region UnclaimAfterSave : bool - Выполнять unclaim all после сохранения

        private bool _UnclaimAfterSave;

        /// <summary>Выполнять unclaim all после сохранения</summary>
        public bool UnclaimAfterSave
        {
            get { return _UnclaimAfterSave; }
            set { SetProperty(ref _UnclaimAfterSave, value); }
        }

        #endregion
        #region Delay : TimeSpan - Интервал между автосохранениями

        private TimeSpan _Delay;

        /// <summary>Интервал между автосохранениями</summary>
        public TimeSpan Delay
        {
            get { return _Delay; }
            set { SetProperty(ref _Delay, value); }
        }

        #endregion
        #region CloseDelay : TimeSpan - Время ожидания ответа при запросе

        private TimeSpan _CloseDelay;

        /// <summary>Время ожидания ответа при запросе</summary>
        public TimeSpan CloseDelay
        {
            get { return _CloseDelay; }
            set { SetProperty(ref _CloseDelay, value); }
        }

        #endregion

        public static Settings GetDefault()
        {
            return new Settings()
            {
                Enabled = true,
                IsNeedRequest = false,
                RequireConfirmationToSave = false,
                UnclaimAfterSave = false,
                Delay = new TimeSpan(2, 0, 0),
                CloseDelay = new TimeSpan(0 , 1, 0)
            };
        }
    }
}

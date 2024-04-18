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
            get => _Enabled;
            set => SetProperty(ref _Enabled, value);
        }

        #endregion
        #region IsNeedRequest : bool - Спрашивать ли перед сохранением

        private bool _IsNeedRequest;

        /// <summary>Спрашивать ли перед сохранением</summary>
        public bool IsNeedRequest
        {
            get => _IsNeedRequest;
            set => SetProperty(ref _IsNeedRequest, value);
        }

        #endregion
        #region Delay : TimeSpan - Интервал между автосохранениями

        private TimeSpan _Delay;

        /// <summary>Интервал между автосохранениями</summary>
        public TimeSpan Delay
        {
            get => _Delay;
            set => SetProperty(ref _Delay, value);
        }

        #endregion
        #region CloseDelay : TimeSpan - Время ожидания ответа при запросе

        private TimeSpan _CloseDelay;

        /// <summary>Время ожидания ответа при запросе</summary>
        public TimeSpan CloseDelay
        {
            get => _CloseDelay;
            set => SetProperty(ref _CloseDelay, value);
        }

        #endregion

        public static Settings GetDefault()
        {
            return new Settings()
            {
                Enabled = true,
                IsNeedRequest = false,
                Delay = new TimeSpan(2, 0, 0),
                CloseDelay = new TimeSpan(0 , 1, 0)
            };
        }
    }
}
using TatBel.Common.Notify;

namespace WpfApplication1
{
    public class MainViewModel : NotifyBase
    {
        #region Title : string - desc

        private string _Title  = "Подтверждение";

        /// <summary>desc</summary>
        public string Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }

        #endregion

        #region Message : string - desc

        private string _Message = "Сохранить работу. Вы уверены?";

        /// <summary>desc</summary>
        public string Message
        {
            get => _Message;
            set => SetProperty(ref _Message, value);
        }

        #endregion

        #region Time : int - desc

        private int _Time = 10;

        /// <summary>desc</summary>
        public int Time
        {
            get => _Time;
            set => SetProperty(ref _Time, value);
        }

        #endregion
    }
}
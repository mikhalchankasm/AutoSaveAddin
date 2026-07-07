using AutoSaveAddin.Lib;

namespace AutoSaveAddin
{
    public class MessageBoxViewModel : NotifyBase
    {
        #region Title : string - desc

        private string _Title  = "Подтверждение";

        /// <summary>desc</summary>
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        #endregion

        #region Message : string - desc

        private string _Message = "Сохранить работу. Вы уверены?";

        /// <summary>desc</summary>
        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
        }

        #endregion

        #region Time : int - desc

        private int _Time = 10;

        /// <summary>desc</summary>
        public int Time
        {
            get { return _Time; }
            set { SetProperty(ref _Time, value); }
        }

        #endregion
    }
}

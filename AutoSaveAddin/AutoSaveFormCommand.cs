using System.Windows;
using Aveva.ApplicationFramework.Presentation;

namespace AutoSaveAddin
{
    public class AutoSaveFormCommand : Command
    {
        public AutoSaveFormCommand()
        {
            Key = "AutoSaveFormCmd";
        }

        public override void Execute()
        {
            if (Environment.MainForm is null)
            {
                Environment.CreateForm();
                Environment.MainForm?.Show();
            }
            else
                Environment.CloseForm();
        }
    }
}
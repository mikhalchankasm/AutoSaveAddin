using System;
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
            try
            {
                if (Environment.MainForm == null)
                    Environment.CreateForm();

                if (Environment.MainForm.IsVisible)
                    Environment.MainForm.Hide();
                else
                    Environment.MainForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "AutoSaveAddin error:\r\n" + ex.Message,
                    "AutoSaveAddin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}

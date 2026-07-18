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
                if (AddinUiContext.MainForm == null)
                    AddinUiContext.CreateForm();

                if (AddinUiContext.MainForm.IsVisible)
                    AddinUiContext.MainForm.Hide();
                else
                    AddinUiContext.MainForm.Show();
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

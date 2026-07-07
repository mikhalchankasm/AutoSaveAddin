using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;

namespace AutoSaveAddin
{
    public class Environment
    {
        public static string SettingPath
        {
            get { return SettingsStorage.SettingsPath; }
        }

        public static Window MainForm { get; set; }

        public static Window CreateForm()
        {
            MainForm = new MainForm();

            IWindowManager windowManager = DependencyResolver.GetImplementationOf<IWindowManager>();
            if (windowManager != null && windowManager.MainForm != null)
                SetOwner(windowManager.MainForm, MainForm);

            ElementHost.EnableModelessKeyboardInterop(MainForm);
            return MainForm;
        }

        public static void CloseForm()
        {
            if (MainForm == null)
                return;

            MainForm.Close();
            MainForm = null;
        }

        private static void SetOwner(Form ownerForm, Window window)
        {
            new WindowInteropHelper(window).Owner = ownerForm.Handle;
        }
    }
}

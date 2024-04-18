using System.IO;
using System.Reflection;
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
        public static string SettingPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/AutoSaveAddin/settings.json");

        public static Window MainForm { get; set; }

        public static Window CreateForm()
        {
            MainForm = new MainForm();
            var avevaWindow = DependencyResolver.GetImplementationOf<IWindowManager>().MainForm;
            SetOwner(avevaWindow, MainForm);
            ElementHost.EnableModelessKeyboardInterop(MainForm);

            return MainForm;
        }

        public static void CloseForm()
        {
            MainForm.Close();
            MainForm = null;
        }
        
        private static void SetOwner(Form ownerForm, Window window) => new WindowInteropHelper(window).Owner = ownerForm.Handle;
    }
}
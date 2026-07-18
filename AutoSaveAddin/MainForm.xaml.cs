using System.ComponentModel;
using System.Windows;

namespace AutoSaveAddin
{
    public partial class MainForm : Window
    {
        public MainForm()
        {
            InitializeComponent();
            DataContext = new MainFormViewModel();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowInTaskbar = false;
        }
    }
}

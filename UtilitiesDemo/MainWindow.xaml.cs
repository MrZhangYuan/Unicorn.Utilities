using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unicorn.Utilities.Commands;
using Unicorn.ViewManager;
using UtilitiesDemo.Dialogs;

namespace UtilitiesDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow Instance
        {
            get;
            private set;
        }
        static MainWindow()
        {
            DefaultUICommandManager.Instance.CommandCanExecuteAction = CommandCanExecuteAction;
            DefaultUICommandManager.Instance.CommandExecuteAction = CommandExecuteAction;
        }

        private CmdListWindow _cmdListWindow = new CmdListWindow();

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            ViewManager.Instance.InitializeRichView(this);
            ViewManager.Instance.ViewPreferences.UsePopupViewAnimations = true;

            this.IsVisibleChanged += (sender, e) =>
            {
                if (this.IsVisible)
                {
                    this._cmdListWindow.Show();
                }
            };
            this.SizeChanged += (sender, e) =>
            {
                this.RefreshCmdWindowLocation();
            };
            this.StateChanged += (sender, e) =>
            {
                this.RefreshCmdWindowLocation();
            };
            this.LocationChanged += (sender, e) =>
            {
                this.RefreshCmdWindowLocation();
            };
        }

        public void RefreshCmdWindowLocation()
        {
            this._cmdListWindow.Owner = this;
            this._cmdListWindow.Height = this.ActualHeight;
            this._cmdListWindow.Left = this.Left + this.ActualWidth;
            this._cmdListWindow.Top = this.Top;
        }

        private static bool CommandCanExecuteAction(string cmdkey, UICommandParameter<string> parameter)
        {
            return true;
        }

        private static void CommandExecuteAction(string cmdkey, UICommandParameter<string> parameter)
        {
            switch (cmdkey)
            {
                case "GreedyWrapPanel":
                    ViewManager.Instance.Show(GreedyWrapPanelDialog.Instance);
                    break;

                case "LazyGrid":
                    ViewManager.Instance.Show(LazyGridDialog.Instance);
                    break;

                case "PaginationWrapPanel":
                    ViewManager.Instance.Show(PaginationWrapPanelDialog.Instance);
                    break;

                case "SpacingWrapPanel":
                    ViewManager.Instance.Show(SpacingWrapPanelDialog.Instance);
                    break;

                case "UniformLineGrid":
                    ViewManager.Instance.Show(UniformLineGridDialog.Instance);
                    break;

                case "ProgressRing":
                    ViewManager.Instance.Show(ProgressRingDialog.Instance);
                    break;

                case "Exit":
                    MainWindow.Instance.Close();
                    break;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.UpdateClipRegion();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var msresult = MessageDialogBox.Show($"是否确认关闭？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msresult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}

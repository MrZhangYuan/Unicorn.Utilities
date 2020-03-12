using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilitiesDemo.Dialogs
{
    /// <summary>
    /// ViewerPanelDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ViewerPanelDialog
    {

        private static ViewerPanelDialog _instance = null;
        public static ViewerPanelDialog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ViewerPanelDialog();
                }
                return _instance;
            }
        }

        public ViewerPanelDialog()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var count = this.panel.NonCollapsedChildren.Count();

            if (this.panel.VisibleIndex < count - 1)
            {
                this.panel.VisibleIndex++;
            }
            else
            {
                this.panel.VisibleIndex = 0;
            }
        }
    }
}

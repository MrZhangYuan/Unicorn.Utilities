using System;
using System.Collections.Generic;
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
    /// PaginationWrapPanelDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PaginationWrapPanelDialog
    {

        private static PaginationWrapPanelDialog _instance = null;
        public static PaginationWrapPanelDialog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PaginationWrapPanelDialog();
                }
                return _instance;
            }
        }

        public PaginationWrapPanelDialog()
        {
            InitializeComponent();

            for (int i = 0; i < 111; i++)
            {
                this.panel.Children.Add(new Button
                {
                    Content = i,
                    BorderThickness=new Thickness(0)
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.panel.PageIndex--;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.panel.PageIndex++;
        }
    }
}

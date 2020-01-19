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
    /// GreedyWrapPanelDialog.xaml 的交互逻辑
    /// </summary>
    public partial class GreedyWrapPanelDialog
    {

        private static GreedyWrapPanelDialog _instance = null;
        public static GreedyWrapPanelDialog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GreedyWrapPanelDialog();
                }
                return _instance;
            }
        }

        public GreedyWrapPanelDialog()
        {
            InitializeComponent();
        }
    }
}

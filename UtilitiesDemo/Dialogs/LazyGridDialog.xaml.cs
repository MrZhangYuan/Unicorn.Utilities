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
    /// LazyGridDialog.xaml 的交互逻辑
    /// </summary>
    public partial class LazyGridDialog
    {

        private static LazyGridDialog _instance = null;
        public static LazyGridDialog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LazyGridDialog();
                }
                return _instance;
            }
        }

        public LazyGridDialog()
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Button bt = new Button();
                    bt.SetValue(Grid.RowProperty, i);
                    bt.SetValue(Grid.ColumnProperty, j);

                    this.grid.Children.Add(bt);
                }
            }
        }
    }
}

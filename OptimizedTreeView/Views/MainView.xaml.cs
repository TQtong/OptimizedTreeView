using OptimizedTreeView.Common;
using OptimizedTreeView.Managers;
using OptimizedTreeView.Models;
using OptimizedTreeView.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace OptimizedTreeView.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(new TreeViewManager());
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            (sender as Grid).Background = Brushes.Red;
        }

        private void Grid_DragLeave(object sender, DragEventArgs e)
        {
            Grid grid = sender as Grid;

            grid.Background = Brushes.Transparent;
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {

        }
    }
}

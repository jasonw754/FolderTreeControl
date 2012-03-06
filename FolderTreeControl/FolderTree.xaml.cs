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
using GeekJ.FolderTreeControl.Model;

namespace GeekJ.FolderTreeControl
{
    public partial class FolderTree : UserControl
    {
        public FolderTree()
        {
            InitializeComponent();
        }

        private void FolderTree_Loaded(object sender, RoutedEventArgs e)
        {
            // default the datacontext if it hasn't been set or isn't the expected type
            if (this.DataContext == null || !(this.DataContext is FolderTreeViewModel))
            {
                this.DataContext = new Model.FolderTreeViewModel();
            }
        }

        private void FolderTree_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(this.DataContext is FolderTreeViewModel))
            {
                this.DataContext = new FolderTreeViewModel();
            }
        }

        private void FolderTree_Expanded(object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)e.OriginalSource;
            var node = (FolderTreeItem)treeViewItem.Header;
            node.LoadChildren();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            HandleCheckBox(sender, e);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleCheckBox(sender, e);
        }

        private void HandleCheckBox(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)e.OriginalSource;
            var treeViewItem = (TreeViewItem)checkbox.Tag;
            var node = (FolderTreeItem)treeViewItem.Header;
            var model = this.DataContext as FolderTreeViewModel;

            model.Selection.HandleCheckBox(node, checkbox.IsChecked);
        }
    }
}

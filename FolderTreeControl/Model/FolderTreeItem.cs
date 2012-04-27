using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace GeekJ.FolderTreeControl.Model
{
    public abstract class FolderTreeItem : ViewModelBase
    {

        private bool _isExpanded;
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private bool? _isChecked = false;
        public bool? IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public abstract string Path { get; }
        public abstract DirectoryInfo DirectoryInfo { get; }

        private static FolderTreeItem placeholder = new Placeholder();
        private ObservableCollection<FolderTreeItem> _folders;
        public ObservableCollection<FolderTreeItem> Folders
        {
            get
            {
                if (_folders == null)
                {
                    _folders = new ObservableCollection<FolderTreeItem>();
                    _folders.Add(placeholder);
                }
                return _folders;
            }
        }

        public FolderTreeItem Parent { get; set; }
        
        public bool FoldersLoaded { get; set; }

        public FolderTreeItem() { }

        public FolderTreeItem(FolderTreeItem parent)
        {
            if (parent != null)
            {
                Parent = parent;
                SelectionItem = parent.SelectionItem;
                IsChecked = parent.IsChecked;
            }
        }


        public abstract void LoadChildren();

        internal FolderTreeSelection.Item SelectionItem { get; set; }
        

        public class Placeholder : FolderTreeItem
        {
            public override void LoadChildren()
            {
                return;
            }

            public override DirectoryInfo DirectoryInfo
            {
                get { return null; }
            }

            public override string Path
            {
                get { return string.Empty; }
            }
        }
    }
}

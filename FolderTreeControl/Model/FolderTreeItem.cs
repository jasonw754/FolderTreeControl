using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

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

        public abstract string Label { get; }
        public abstract ObservableCollection<FolderTreeItem> Folders { get;}

        public abstract void LoadChildren();

        public FolderTreeItem Parent { get; set; }

        internal FolderTreeSelection.Item SelectionItem { get; set; }

        public abstract bool FoldersLoaded { get; set; }
    }
}

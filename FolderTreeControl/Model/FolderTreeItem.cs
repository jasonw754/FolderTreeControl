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

        private bool _isCheckedInherited = true;
        public bool IsCheckedInherited 
        { 
            get 
            { 
                return _isCheckedInherited;
            }
            set
            {
                _isCheckedInherited = value;
                OnPropertyChanged("IsCheckedInherited");
            }
        }

        public abstract string Label { get; }
        public abstract ObservableCollection<FolderTreeItem> Folders { get;}

        public abstract void LoadChildren();
    }
}

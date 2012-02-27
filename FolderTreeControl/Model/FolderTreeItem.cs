using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public abstract void LoadChildren();
    }
}

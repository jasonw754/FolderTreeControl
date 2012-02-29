using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace GeekJ.FolderTreeControl.Model
{
    public class Drive : FolderTreeItem
    {
        private DriveInfo _driveInfo;
        public DriveInfo DriveInfo
        {
            get
            {
                return _driveInfo;
            }
            set
            {
                _driveInfo = value;
            }
        }

        public Drive(DriveInfo driveInfo)
        {
            DriveInfo = driveInfo;
        }

        public override string Label
        {
            get
            {
                if (DriveInfo == null)
                    return null;
                return DriveInfo.Name;
            }
        }

        private ObservableCollection<FolderTreeItem> _folders;
        public override ObservableCollection<FolderTreeItem> Folders
        {
            get
            {
                if (_folders == null)
                {
                    _folders = new ObservableCollection<FolderTreeItem>();
                    _folders.Add(new Folder());
                }
                return _folders;
            }
        }

        public override void LoadChildren()
        {
            Folders.Clear();
            foreach (var child in Folder.LoadSubFolders(DriveInfo.RootDirectory))
            {
                Folders.Add(child);
            }
        }
    }
}

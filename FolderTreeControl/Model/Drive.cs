using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace GeekJ.FolderTreeControl.Model
{
    /// <summary>
    /// A folder tree item representing a logical drive on the local system.
    /// </summary>
    public class Drive : FolderTreeItem
    {
        private DriveInfo _driveInfo;
        
        /// <summary>
        /// Information about the drive.
        /// </summary>
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

        /// <summary>
        /// Loads subfolders for the drive tree item by loading subfolders of the drive's root directory.
        /// </summary>
        public override void LoadChildren()
        {
            Folders.Clear();
            foreach (var child in Folder.LoadSubFolders(DriveInfo.RootDirectory, this))
            {
                Folders.Add(child);
            }
            FoldersLoaded = true;
        }

        public override DirectoryInfo DirectoryInfo
        {
            get { return DriveInfo.RootDirectory; }
        }

        public override string Path
        {
            get 
            {
                return DirectoryInfo.FullName;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Drive;
            return (other != null && other.DriveInfo.Equals(DriveInfo));
        }

        public override int GetHashCode()
        {
            return DriveInfo.GetHashCode();
        }
    }
}

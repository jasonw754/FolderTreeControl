using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace GeekJ.FolderTreeControl.Model
{
    /// <summary>
    /// A folder tree item representing a folder in the local filesystem.
    /// </summary>
    public class Folder : FolderTreeItem
    {
        private System.IO.DirectoryInfo _directoryInfo;
        
        /// <summary>
        /// Information about the folder.
        /// </summary>
        public override System.IO.DirectoryInfo DirectoryInfo
        {
            get
            {
                return _directoryInfo;
            }
        }

        public Folder(System.IO.DirectoryInfo directoryInfo, FolderTreeItem parent)
            : base(parent)
        {
           _directoryInfo = directoryInfo;
        }

        public override void LoadChildren()
        {
            Folders.Clear();
            foreach (var child in Folder.LoadSubFolders(DirectoryInfo, this))
            {
                Folders.Add(child);
            }
            FoldersLoaded = true;
        }

        internal static IEnumerable<Folder> LoadSubFolders(System.IO.DirectoryInfo directoryInfo, FolderTreeItem parent)
        {
            try
            {
                return directoryInfo.EnumerateDirectories().Select(x => new Folder(x, parent));
            }
            catch (UnauthorizedAccessException e)
            {
                return Enumerable.Empty<Folder>();
            }
            catch (IOException e)
            {
                return Enumerable.Empty<Folder>();
            }
        }

        public override string Path
        {
            get { return DirectoryInfo.FullName; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Folder;
            return (other != null 
                && other.DirectoryInfo != null
                && other.DirectoryInfo.Equals(DirectoryInfo));
        }

        public override int GetHashCode()
        {
            return DirectoryInfo.GetHashCode();
        }
    }
}

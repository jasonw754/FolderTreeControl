using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace GeekJ.FolderTreeControl.Model
{
    public class Folder : FolderTreeItem
    {
        private System.IO.DirectoryInfo directoryInfo;

        public Folder(System.IO.DirectoryInfo directoryInfo, FolderTreeItem parent)
            : base(parent)
        {
            this.directoryInfo = directoryInfo;
        }

        public override bool FoldersLoaded { get; set; }

        public override string Label
        {
            get
            {
                if (directoryInfo == null)
                    return null;
                return directoryInfo.Name;
            }
        }

        public override void LoadChildren()
        {
            Folders.Clear();
            foreach (var child in Folder.LoadSubFolders(directoryInfo, this))
            {
                Folders.Add(child);
            }
            FoldersLoaded = true;
        }

        internal static IEnumerable<Folder> LoadSubFolders(System.IO.DirectoryInfo directoryInfo, FolderTreeItem parent)
        {
            return directoryInfo.EnumerateDirectories().Select(x => new Folder(x, parent));            
        }

        public override bool Equals(object obj)
        {
            var other = obj as Folder;
            return (other != null 
                && other.directoryInfo != null
                && other.directoryInfo.Equals(directoryInfo));
        }

        public override int GetHashCode()
        {
            return directoryInfo.GetHashCode();
        }
    }
}

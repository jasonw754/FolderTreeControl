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

        public Folder()
        {

        }

        public Folder(System.IO.DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
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
            foreach (var child in Folder.LoadSubFolders(directoryInfo))
            {
                Folders.Add(child);
                child.Parent = this;
                child.SelectionItem = SelectionItem;
                child.IsChecked = IsChecked;
            }
            FoldersLoaded = true;
        }

        internal static IEnumerable<Folder> LoadSubFolders(System.IO.DirectoryInfo directoryInfo)
        {
            return directoryInfo.EnumerateDirectories().Select(x => new Folder(x));            
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

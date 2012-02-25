using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace GeekJ.FolderTreeControl.Model
{
    public class Folder : Node
    {
        private System.IO.DirectoryInfo directoryInfo;

        public Folder()
        {

        }

        public Folder(System.IO.DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        private ObservableCollection<Folder> _folders;
        public ObservableCollection<Folder> Folders
        {
            get
            {
                if (_folders == null)
                {
                    _folders = new ObservableCollection<Folder>();
                    _folders.Add(new Folder());
                }
                return _folders;
            }
        }

        public string Label
        {
            get
            {
                return directoryInfo.Name;
            }
        }

        public override void LoadChildren()
        {
            Folders.Clear();
            foreach (var child in Folder.LoadSubFolders(directoryInfo))
            {
                Folders.Add(child);
            }
        }

        internal static IEnumerable<Folder> LoadSubFolders(System.IO.DirectoryInfo directoryInfo)
        {
            return directoryInfo.EnumerateDirectories().Select(x => new Folder(x));            
        }
    }
}

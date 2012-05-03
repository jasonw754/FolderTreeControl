using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GeekJ.FolderTreeControl.Model
{
    public class FolderTreeSelection : ViewModelBase
    {
        private SortedList<string, Item> items = new SortedList<string, Item>();
        private Dictionary<Item, long> itemCounts = new Dictionary<Item, long>(); 

        public event EventHandler Changed;

        public FolderTreeSelection() { }

        public long FolderCount {
            get
            {
                return itemCounts.Sum(x => x.Value);
            }
        }

        public IEnumerable<DirectoryInfo> SelectedFolders
        {
            get
            {
                if (items.Count() > 0)
                {
                    foreach (var dir in items.Where(x => x.Value.Include).Select(x => x.Value))
                    {
                        yield return dir.TreeItem.DirectoryInfo;
                        itemCounts[dir] = 1;
                        OnPropertyChanged("FolderCount");
                        foreach (var subDir in EnumerateSelectedSubFolders(dir.TreeItem.DirectoryInfo))
                        {
                            yield return subDir;
                            itemCounts[dir]++;
                            OnPropertyChanged("FolderCount");
                        }
                    }
                }
            }
        }

        private IEnumerable<DirectoryInfo> EnumerateSelectedSubFolders(DirectoryInfo directoryInfo)
        {
            IEnumerable<DirectoryInfo> subDirs;
            try
            {
                subDirs = directoryInfo.EnumerateDirectories();
            }
            catch (UnauthorizedAccessException e)
            {
                subDirs = Enumerable.Empty<DirectoryInfo>();
            }
            catch (IOException e)
            {
                subDirs = Enumerable.Empty<DirectoryInfo>();
            }

            foreach (var subDir in subDirs)
            {
                if (items.ContainsKey(subDir.FullName) && !items[subDir.FullName].Include)
                {
                    continue;
                }
                yield return subDir;
                foreach (var recursion in EnumerateSelectedSubFolders(subDir))
                {
                    yield return recursion;
                }
            }
        }

        internal void Add(FolderTreeItem folderTreeItem, bool include = true)
        {
            if (folderTreeItem.SelectionItem == null && include)
            {
                // node is checked for the first time, add a selection item
                folderTreeItem.SelectionItem = new Item() { TreeItem = folderTreeItem, Include = include };
                items.Add(folderTreeItem.Path, folderTreeItem.SelectionItem);

                OnChanged();
            }
            else if (folderTreeItem.SelectionItem != null && folderTreeItem.SelectionItem.Include != include)
            {
                // node with a current state is checked/unchecked
                if (!include && (folderTreeItem.Parent == null || folderTreeItem.Parent.SelectionItem == null))
                {
                    // special behavior if root node is unchecked
                    items.Remove(folderTreeItem.Path);
                    itemCounts.Remove(folderTreeItem.SelectionItem);
                    OnPropertyChanged("FolderCount");
                    folderTreeItem.SelectionItem = null;
                }
                else
                {
                    // determine if this node itself was specifically checked/unchecked
                    var match = items.Where(x => x.Key.Equals(folderTreeItem.Path)).SingleOrDefault().Value;
                    if (match != null)
                    {
                        // if so, adjust it's state
                        match.Include = include;
                        if (!match.Include)
                        {
                            itemCounts.Remove(match);
                            OnPropertyChanged("FolderCount");
                        }
                    }
                    else
                    {
                        // if not, add a new selection item
                        folderTreeItem.SelectionItem = new Item() { TreeItem = folderTreeItem, Include = include };
                        items.Add(folderTreeItem.Path, folderTreeItem.SelectionItem);
                    }
                }
                OnChanged();
            }

            // process children nodes if any are loaded
            if (folderTreeItem.FoldersLoaded)
            {
                foreach (var childItem in folderTreeItem.Folders)
                {
                    // all children node states should be wiped out and given a reference to this nodes state
                    if (items.Remove(childItem.Path))
                    {
                        OnChanged();
                    }
                    childItem.SelectionItem = folderTreeItem.SelectionItem;
                    childItem.IsChecked = include;

                    // recurse to go as many levels deep as we can
                    // recursive calls will skip the code above because the selection item is already set here
                    Add(childItem, include);
                }
            }

            // give the parent nodes either checked or indeterminate states
            ProcessParents(folderTreeItem);
        }

        private void ProcessParents(FolderTreeItem folderTreeItem)
        {
            if (folderTreeItem.Parent == null)
                return;
            bool indeterminate = false;
            bool? current = folderTreeItem.IsChecked;

            // walk up the tree
            do
            {
                folderTreeItem = folderTreeItem.Parent;

                // if any of the other children have a different checkbox state, set to indeterminate
                if (indeterminate || folderTreeItem.Folders.Any(x => x.IsChecked != current))
                {
                    folderTreeItem.IsChecked = null;
                    indeterminate = true;
                }
                else
                {
                    folderTreeItem.IsChecked = current;
                }
            } while (folderTreeItem.Parent != null);
        }

        internal void HandleCheckBox(FolderTreeItem node, bool? isChecked)
        {
            // defend against the recursive calls
            if (node.SelectionItem == null || node.SelectionItem.Include != isChecked.Value)
            {
                Add(node, isChecked.Value);
            }
        }

        private void OnChanged()
        {
            OnPropertyChanged("SelectedFolders");
            if (Changed != null)
            {
                Changed(this, new EventArgs());
            }
        }

        public class Item
        {
            public FolderTreeItem TreeItem { get; set; }
            public bool Include { get; set; }

            public Item() { }
        }
    }
}
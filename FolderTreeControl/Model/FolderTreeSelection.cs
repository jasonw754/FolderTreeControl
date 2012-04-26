using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeekJ.FolderTreeControl.Model
{
    public class FolderTreeSelection : ViewModelBase
    {
        private List<Item> items = new List<Item>();

        public FolderTreeSelection() { }

        public void Add(FolderTreeItem folderTreeItem, bool include = true)
        {
            if (folderTreeItem.SelectionItem == null && include)
            {
                // node is checked for the first time, add a selection item
                folderTreeItem.SelectionItem = new Item() { TreeItem = folderTreeItem, Include = include };
                items.Add(folderTreeItem.SelectionItem);
            }
            else if (folderTreeItem.SelectionItem != null && folderTreeItem.SelectionItem.Include != include)
            {
                // node with a current state is checked/unchecked
                if (!include && (folderTreeItem.Parent == null || folderTreeItem.Parent.SelectionItem == null))
                {
                    // special behavior if root node is unchecked
                    items.RemoveAll(x => x.TreeItem.Equals(folderTreeItem));
                    folderTreeItem.SelectionItem = null;
                }
                else
                {
                    // determine if this node itself was specifically checked/unchecked
                    var match = items.Where(x => x.TreeItem.Equals(folderTreeItem)).SingleOrDefault();
                    if (match != null)
                    {
                        // if so, adjust it's state
                        match.Include = include;
                    }
                    else
                    {
                        // if not, add a new selection item
                        folderTreeItem.SelectionItem = new Item() { TreeItem = folderTreeItem, Include = include };
                        items.Add(folderTreeItem.SelectionItem);
                    }
                }
            }

            // process children nodes if any are loaded
            if (folderTreeItem.FoldersLoaded)
            {
                foreach (var childItem in folderTreeItem.Folders)
                {
                    // all children node states should be wiped out and given a reference to this nodes state
                    items.RemoveAll(x => x.TreeItem.Equals(childItem));
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

        public class Item
        {
            public FolderTreeItem TreeItem { get; set; }
            public bool Include { get; set; }

            public Item() { }
        }
    }
}
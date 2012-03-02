using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeekJ.FolderTreeControl.Model
{
    public class FolderTreeSelection
    {
        private List<Item> items = new List<Item>();

        public FolderTreeSelection() { }

        public void Add(FolderTreeItem folderTreeItem, bool include = true)
        {
            if (folderTreeItem.SelectionItem == null && include)
            {                
                folderTreeItem.SelectionItem = new Item() { TreeItem = folderTreeItem, Include = include };
                items.Add(folderTreeItem.SelectionItem);
            }
            else if (folderTreeItem.SelectionItem != null && folderTreeItem.SelectionItem.Include != include)
            {
                if (!include && (folderTreeItem.Parent == null || folderTreeItem.Parent.SelectionItem == null))
                {
                    items.RemoveAll(x => x.TreeItem.Equals(folderTreeItem));
                    folderTreeItem.SelectionItem = null;
                }
                else
                {
                    var match = items.Where(x => x.TreeItem.Equals(folderTreeItem)).SingleOrDefault();
                    if (match != null)
                    {
                        match.Include = include;
                    }
                    else
                    {
                        folderTreeItem.SelectionItem = new Item() { TreeItem = folderTreeItem, Include = include };
                        items.Add(folderTreeItem.SelectionItem);
                    }
                }
            }

            if (folderTreeItem.FoldersLoaded)
            {
                foreach (var childItem in folderTreeItem.Folders)
                {
                    items.RemoveAll(x => x.TreeItem.Equals(childItem));
                    childItem.SelectionItem = folderTreeItem.SelectionItem;
                    childItem.IsChecked = include;
                    Add(childItem, include);
                }
            }

            Console.WriteLine(items.Count());
        }

        public class Item
        {
            public FolderTreeItem TreeItem { get; set; }
            public bool Include { get; set; }

            public Item() { }
        }
    }
}
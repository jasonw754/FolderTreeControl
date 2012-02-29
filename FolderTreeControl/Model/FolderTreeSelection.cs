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
            items.Add(new Item() { TreeItem = folderTreeItem, Include = include});
        }

        public class Item
        {
            public FolderTreeItem TreeItem { get; set; }
            public bool Include { get; set; }

            public Item() { }
        }
    }
}
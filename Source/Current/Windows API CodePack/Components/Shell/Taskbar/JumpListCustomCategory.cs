//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Microsoft.WindowsAPICodePack.Taskbar
{
    /// <summary>
    /// Represents a custom category on the taskbar's jump list
    /// </summary>
    public class JumpListCustomCategory
    {
        private string? _name;

        internal JumpListItemCollection<IJumpListItem> JumpListItems
        {
            get;
            private set;
        }

        /// <summary>
        /// Category name
        /// </summary>
        public string? Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }


        /// <summary>
        /// Add JumpList items for this category
        /// </summary>
        /// <param name="items">The items to add to the JumpList.</param>
        public void AddJumpListItems(params IJumpListItem[]? items)
        {
            if (items != null)
            {
                foreach (IJumpListItem item in items)
                {
                    JumpListItems.Add(item);
                }
            }
        }

        /// <summary>
        /// Event that is triggered when the jump list collection is modified
        /// </summary>
        internal event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        /// <summary>
        /// Creates a new custom category instance
        /// </summary>
        /// <param name="categoryName">Category name</param>
        public JumpListCustomCategory(string? categoryName)
        {
            Name = categoryName;

            JumpListItems = new JumpListItemCollection<IJumpListItem>();
            JumpListItems.CollectionChanged += OnJumpListCollectionChanged!;
        }

        internal void OnJumpListCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged(this, args);
        }


        internal void RemoveJumpListItem(string path)
        {
            List<IJumpListItem> itemsToRemove = new(
                from i in JumpListItems
                where string.Equals(path, i.Path, StringComparison.OrdinalIgnoreCase)
                select i);

            // Remove matching items
            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                JumpListItems.Remove(itemsToRemove[i]);
            }
        }
    }
}

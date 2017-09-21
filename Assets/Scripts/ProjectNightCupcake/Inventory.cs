using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectnightcupcake
{
    public class Inventory : MonoBehaviour
    {
        private List<InventoryItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }
        [SerializeField]
        private List<InventoryItem> _items;

        // Use this for initialization
        void Start()
        {
            Items = new List<InventoryItem>();
        }

        public void AddItem(InventoryItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(InventoryItem item)
        {
            Items.Remove(item);
        }

        public IList<InventoryItem> GetAllItems()
        {
            return Items;
        }
    }
}
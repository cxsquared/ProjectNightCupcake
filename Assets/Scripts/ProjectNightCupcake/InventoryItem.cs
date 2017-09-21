using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectnightcupcake
{
    public class InventoryItem : IInteractable
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        private DragNDropItem DragNDropScript { get; set; }

        // Use this for initialization
        void Start()
        {
            DragNDropScript = GetComponent<DragNDropItem>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        override public void Interact(GameObject player)
        {
            if (DragNDropScript == null)
            {
                player.GetComponent<Inventory>().AddItem(this);
                Destroy(gameObject);
            }
        }
    }
}

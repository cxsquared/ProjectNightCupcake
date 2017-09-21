using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectnightcupcake
{
    public class DragNDropItem : IInteractable
    {
        private Rigidbody ThisRigidbody { get; set; }
        private GameObject Player { get; set; }

        // Use this for initialization
        void Start()
        {
            ThisRigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Player != null && Input.GetAxis("Interact") == 0)
            {
                Player = null;
                transform.parent = null;
                ThisRigidbody.isKinematic = false;
            }
        }

        override public void Interact(GameObject player)
        {
            Player = player.GetComponentInChildren<Camera>().gameObject;
            transform.parent = Player.transform;
            ThisRigidbody.isKinematic = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectnightcupcake
{
    [RequireComponent(typeof(Collider))]
    public abstract class IInteractable : MonoBehaviour{

        public virtual void Interact(GameObject player) { }

        public virtual void OnHoverEnter(GameObject player) { }

        public virtual void OnHoverExit(GameObject player) { }
    }
}

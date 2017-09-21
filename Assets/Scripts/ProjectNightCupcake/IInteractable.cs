using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectnightcupcake
{
    public abstract class IInteractable : MonoBehaviour{

        public virtual void Interact(GameObject player) { }
    }
}

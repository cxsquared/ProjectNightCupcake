using UnityEngine;

namespace projectnightcupcake
{
    [RequireComponent(typeof(Collider))]
    public abstract class IInteractable : MonoBehaviour {

        [SerializeField]
        private string _name;
        public string Name { get { return _name; } protected set { _name = value; } }

        public virtual void Interact(GameObject player) { }

        public virtual void OnHoverEnter(GameObject player) { }

        public virtual void OnHoverExit(GameObject player) { }

        public virtual bool IsInteracting(GameObject player) { return false; }
    }
}

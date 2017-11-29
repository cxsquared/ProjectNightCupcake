using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectnightcupcake
{
    public class Player : MonoBehaviour {

        [SerializeField]
        float _reachLength = 10f;
        float ReachLength
        {
            get { return _reachLength; }
            set { _reachLength = value; }
        }

        private Inventory PlayerInventory { get; set; }

        private Camera PlayerCamera { get; set; }

        [SerializeField]
        private IInteractable _currentObject;
        private IInteractable CurrentObject
        {   get
            {
                return _currentObject;
            }
            set
            {
                _currentObject = value;
                if (_currentObject != null)
                {
                    Debug.Log("Sending on hover enter message to " + _currentObject.name);
                    _currentObject.OnHoverEnter(gameObject);
                }
            }
        }

        // Use this for initialization
        void Start () {
            PlayerCamera = GetComponentInChildren<Camera>(); 
        }
        
        // Update is called once per frame
        void Update () {

            var rayStartLocation = PlayerCamera.transform.position;
            var rayDirection = PlayerCamera.transform.forward;
            RaycastHit ray;
 
            if (Physics.Raycast(rayStartLocation, rayDirection, out ray, ReachLength))
            {
                var interactComponent = ray.transform.GetComponent<IInteractable>();
                if (interactComponent != null)
                {
                    if (CurrentObject == null || CurrentObject != interactComponent)
                    {
                        CallOnHoverExitOnCurrentObject();
                        CurrentObject = interactComponent;
                    }

                    if (Input.GetButtonDown("Interact"))
                    {
                        Debug.Log("Sending hit message to " + interactComponent.name);
                        CurrentObject.Interact(gameObject);
                    }
                }
            }
            else if (CurrentObject != null && !CurrentObject.IsInteracting(gameObject))
            {
                CallOnHoverExitOnCurrentObject();
                CurrentObject = null;
            }
        }

        private void CallOnHoverExitOnCurrentObject()
        {
            if (CurrentObject != null)
            {
                Debug.Log("Sending on hover exit message to " + CurrentObject.name);
                CurrentObject.OnHoverExit(gameObject);
            }
            CurrentObject = null;
        }
    }
}


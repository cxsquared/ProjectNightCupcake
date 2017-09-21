using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace projectnightcupcake
{
    public class Player : MonoBehaviour {

        float ReachLength
        {
            get { return _reachLength; }
            set { _reachLength = value; }
        }
        [SerializeField]
        float _reachLength = 10f;

        private Camera PlayerCamera { get; set; }

        // Use this for initialization
        void Start () {
            PlayerCamera = GetComponentInChildren<Camera>(); 
        }
        
        // Update is called once per frame
        void Update () {
            
            if (Input.GetAxis("Interact") != 0)
            {
                var rayStartLocation = PlayerCamera.transform.position;
                var rayDirection = PlayerCamera.transform.forward;
                RaycastHit ray;
                if (Physics.Raycast(rayStartLocation, rayDirection, out ray, ReachLength))
                {
                    var interactComponent = ray.transform.GetComponent<IInteractable>();
                    if (interactComponent != null)
                    {
                        Debug.Log("Sending hit message to " + interactComponent.name);
                        interactComponent.Interact(gameObject);
                    }
                }
                Debug.DrawRay(rayStartLocation, rayDirection * ReachLength, Color.cyan, 10f);
            }
        }
    }
}


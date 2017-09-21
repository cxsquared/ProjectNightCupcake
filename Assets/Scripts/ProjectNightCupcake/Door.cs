using System.Collections;
using UnityEngine;

namespace projectnightcupcake
{
    public class Door : IInteractable
    {
        float OpenSpeed
        {
            get { return _openSpeed; }
            set { _openSpeed = value; }
        }
        [SerializeField]
        float _openSpeed = 5;

        [SerializeField]
        bool opened = false;
        [SerializeField]
        bool opening = false;

        Animator Animator { get; set; }

        // Use this for initialization
        void Start ()
        {
            Animator = GetComponent<Animator>();
        }

        override public void Interact(GameObject player)
        {
            if (!opening)
            {
                opening = true;
                if (opened)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }

        void Open()
        {
            Animator.CrossFade("OpenDoor", .5f);
        }
        
        void Close()
        {
            Animator.CrossFade("CloseDoor", .5f);
        }

        void ToggleOpening()
        {
            opened = !opened;
            opening = false;
        }
    }
}

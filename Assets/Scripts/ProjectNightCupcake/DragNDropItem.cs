using UnityEngine;

namespace projectnightcupcake
{

    [RequireComponent(typeof(Rigidbody))]
    public class DragNDropItem : IInteractable
    {
        private Rigidbody ThisRigidbody { get; set; }

        [SerializeField]
        private GameObject _player;
        public GameObject Player { get { return _player; } private set { _player = value; } }

        [SerializeField]
        private Timer _timer;
        private Timer Timer { get { return _timer; } set { _timer = value; } }

        [SerializeField]
        private bool _canBePickedUp = true;
        public bool CanBePickedUp { get { return _canBePickedUp; } private set { _canBePickedUp = value; } }

        [SerializeField]
        private bool _canBeDropped = false;
        public bool CanBeDropped { get { return _canBeDropped; } private set { _canBeDropped = value; } }

        [SerializeField]
        private float _hoverDistance = 1.5f;
        public float HoverDistance { get { return _hoverDistance; } private set { _hoverDistance = value; } }

        [SerializeField]
        private float _hoverSpeed = 5f;
        public float HoverSpeed { get { return _hoverSpeed; } private set { _hoverSpeed = value; } }

        [SerializeField]
        private float _pickupDelay = .25f;
        public float PickupDelay { get { return _pickupDelay; } private set { _pickupDelay = value; } }

        [SerializeField]
        private Quaternion _startingOrientation;
        private Quaternion StartingOrientation { get; set; }

        // Use this for initialization
        void Start()
        {
            Timer = gameObject.AddComponent<Timer>();
            ThisRigidbody = GetComponent<Rigidbody>();
            StartingOrientation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (Player != null && Input.GetButtonDown("Interact") && CanBeDropped)
            {
                Player = null;
                //ThisRigidbody.isKinematic = false;
                ThisRigidbody.useGravity = true;
                ThisRigidbody.constraints = RigidbodyConstraints.None;
                Timer.StartTimer(PickupDelay, 1, PickUpReset);
            }
            else if (Player != null)
            {
                var newPosition = Player.transform.position + (Player.transform.forward.normalized * HoverDistance);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * HoverSpeed);
                var newRotation = Player.transform.rotation.eulerAngles;
                newRotation.x = StartingOrientation.eulerAngles.x;
                newRotation.z = StartingOrientation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(newRotation); 
            }
        }

        void PickUpReset(Timer t)
        {
            CanBePickedUp = true;
        }
        
        void DropReset(Timer t)
        {
            CanBeDropped = true;
        }

        override public void Interact(GameObject player)
        {
            if (Player == null && CanBePickedUp == true)
            {
                Player = player.GetComponentInChildren<Camera>().gameObject;
                ThisRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                //ThisRigidbody.isKinematic = true;
                ThisRigidbody.useGravity = false;
                CanBePickedUp = false;
                CanBeDropped = false;
                Timer.StartTimer(PickupDelay, 1, DropReset);
                transform.rotation = StartingOrientation;
            }
        }
    }
}
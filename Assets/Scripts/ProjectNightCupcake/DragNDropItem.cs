using System.Diagnostics;
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
        private Timer _dropTimer;
        private Timer DropTimer { get { return _dropTimer; } set { _dropTimer = value; } }

        [SerializeField]
        private Stopwatch _throwTimer;
        private Stopwatch ThrowTimer { get { return _throwTimer; } set { _throwTimer = value; } }

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
        private float _throwForceMin = 1f;
        public float ThrowForceMin { get { return _throwForceMin; } private set { _throwForceMin = value; } }

        [SerializeField]
        private float _throwForceMax = 2f;
        public float ThrowForceMax { get { return _throwForceMax; } private set { _throwForceMax = value; } }

        [SerializeField]
        private float _timeToMaxThrow = 2f;
        public float TimerToMaxThrow { get { return _timeToMaxThrow; } private set { _timeToMaxThrow = value; } }

        [SerializeField]
        private bool _throwing = false;
        public bool Throwing { get { return _throwing; } private set { _throwing = value; } }

        [SerializeField]
        private Quaternion _startingOrientation;
        private Quaternion StartingOrientation { get; set; }

        // Use this for initialization
        void Start()
        {
            DropTimer = gameObject.AddComponent<Timer>();
            ThrowTimer = new Stopwatch();
            ThisRigidbody = GetComponent<Rigidbody>();
            StartingOrientation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (Player == null)
                return;

            if (Input.GetButtonDown("Throw") && !Throwing)
            {
                Throwing = true;
                ThrowTimer.Reset();
                ThrowTimer.Start();
            }

            if (Input.GetButtonDown("Interact") && CanBeDropped)
            {
                DropItem();
            }
            else if (Input.GetButtonUp("Throw") && CanBeDropped)
            {
                var playerVelocity = Player.GetComponentInParent<Rigidbody>().velocity;
                var baseThrowForce = Mathf.Lerp(ThrowForceMin, ThrowForceMax, (float)ThrowTimer.Elapsed.TotalSeconds / TimerToMaxThrow);
                var throwingVelocity = Player.transform.forward * Mathf.Clamp(baseThrowForce, ThrowForceMin, ThrowForceMax);
                UnityEngine.Debug.Log("Throwing itme with force " + throwingVelocity + "   Player velocity = " );
                DropItem();
                ThrowTimer.Stop();
                Throwing = false;

                ThisRigidbody.AddForce(throwingVelocity);
            }
            else 
            {
                UpdateHeldItemPosition();
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

        void DropItem()
        {
                Player = null;
                //ThisRigidbody.isKinematic = false;
                ThisRigidbody.useGravity = true;
                ThisRigidbody.ResetInertiaTensor();
                ThisRigidbody.constraints = RigidbodyConstraints.None;
                DropTimer.StartTimer(PickupDelay, 1, PickUpReset);
        }

        void UpdateHeldItemPosition()
        {
                var newPosition = Player.transform.position + (Player.transform.forward.normalized * HoverDistance);
                ThisRigidbody.MovePosition(Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * HoverSpeed));
                var newRotation = Player.transform.rotation.eulerAngles;
                newRotation.x = StartingOrientation.eulerAngles.x;
                newRotation.z = StartingOrientation.eulerAngles.y;
                ThisRigidbody.MoveRotation(Quaternion.Euler(newRotation));
        }

        override public void Interact(GameObject player)
        {
            if (Player == null && CanBePickedUp == true)
            {
                Player = player.GetComponentInChildren<Camera>().gameObject;
                ThisRigidbody.useGravity = false;
                CanBePickedUp = false;
                CanBeDropped = false;
                DropTimer.StartTimer(PickupDelay, 1, DropReset);
                transform.rotation = StartingOrientation;
            }
        }

        public override void OnHoverEnter(GameObject player)
        {
            base.OnHoverEnter(player);

            Crosshair.SetAlpha(1f);
        }

        public override void OnHoverExit(GameObject player)
        {
            base.OnHoverExit(player);

            Crosshair.ResetAlpha();
        }
    }
}
using System.Diagnostics;
using UnityEngine;

namespace projectnightcupcake
{
    [RequireComponent(typeof(Rigidbody))]
    public class DragNDropItem : IInteractable
    {
        private Rigidbody ThisRigidbody { get; set; }
        private Rigidbody PlayerRigidBody { get; set; }
        private Renderer ThisRenderer { get; set; }

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
        private Quaternion StartingOrientation { get { return _startingOrientation; } set { _startingOrientation = value; } }

        [SerializeField]
        private float _stuckDropTollerance;
        public float StuckDropTollerance { get { return _stuckDropTollerance; } set { _stuckDropTollerance = value; } }

        [SerializeField]
        private float _distanceToCollisionCheck;
        public float DistanceToCollisionCheck { get { return _distanceToCollisionCheck; } set { _distanceToCollisionCheck = value; } }

        [SerializeField]
        private Vector3 _locationToMoveTo;
        private Vector3 LocationToMoveTo { get { return _locationToMoveTo; } set { _locationToMoveTo = value; } }

        // Use this for initialization
        void Start()
        {
            DropTimer = gameObject.AddComponent<Timer>();
            ThrowTimer = new Stopwatch();
            ThisRigidbody = GetComponent<Rigidbody>();
            ThisRenderer = GetComponent<Renderer>();
            StartingOrientation = transform.rotation;

            var size = ThisRenderer.bounds.size;
            DistanceToCollisionCheck = Mathf.Max(Mathf.Max(size.x, size.y), size.z);
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
        }

        private void FixedUpdate()
        {
            if (Player == null)
                return;

            if (Input.GetButtonDown("Interact") && CanBeDropped)
            {
                DropItem();
            }
            else if (Input.GetButtonUp("Throw") && CanBeDropped)
            {
                var playerVelocity = Player.GetComponentInParent<Rigidbody>().velocity;
                var baseThrowForce = Mathf.Lerp(ThrowForceMin, ThrowForceMax, (float)ThrowTimer.Elapsed.TotalSeconds / TimerToMaxThrow);
                var throwingVelocity = Player.transform.forward * Mathf.Clamp(baseThrowForce, ThrowForceMin, ThrowForceMax);
                DropItem();
                ThrowTimer.Stop();
                Throwing = false;

                ThisRigidbody.AddForce(throwingVelocity);
            }
            else 
            {
                UpdateHeldItemPosition();

                var distance = Mathf.Abs(Vector3.Distance(transform.position, LocationToMoveTo));

                if (distance >= StuckDropTollerance)
                {
                    DropItem();
                }
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
            PlayerRigidBody = null;
            ThisRigidbody.useGravity = true;
            ThisRigidbody.ResetInertiaTensor();
            ThisRigidbody.constraints = RigidbodyConstraints.None;
            DropTimer.StartTimer(PickupDelay, 1, PickUpReset);
        }

        void UpdateHeldItemPosition()
        {
            LocationToMoveTo = Player.transform.position + (Player.transform.forward.normalized * HoverDistance);

            if (CanMove())
            {
                ThisRigidbody.MovePosition(Vector3.MoveTowards(transform.position, LocationToMoveTo, Time.deltaTime * HoverSpeed));
            }

            var newRotation = Player.transform.rotation.eulerAngles;
            newRotation.x = StartingOrientation.eulerAngles.x;
            newRotation.z = StartingOrientation.eulerAngles.y;
            ThisRigidbody.MoveRotation(Quaternion.Euler(newRotation));
        }

        bool CanMove()
        {
            var direction = (LocationToMoveTo - transform.position).normalized;
            var objectsInTheWay = Physics.RaycastAll(new Ray(ThisRenderer.bounds.ClosestPoint(LocationToMoveTo), direction), DistanceToCollisionCheck);
            UnityEngine.Debug.DrawRay(ThisRenderer.bounds.ClosestPoint(LocationToMoveTo), direction, Color.cyan, .5f);

            foreach(var obj in objectsInTheWay)
            {
                if (obj.rigidbody != ThisRigidbody && obj.rigidbody != PlayerRigidBody)
                {
                    return false;
                }
            }

            return true;
        }

        override public void Interact(GameObject player)
        {
            if (Player == null && CanBePickedUp == true)
            {
                Player = player.GetComponentInChildren<Camera>().gameObject;
                PlayerRigidBody = player.GetComponent<Rigidbody>();
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
            UiController.Instance.SetItemDescription(name);
        }

        public override void OnHoverExit(GameObject player)
        {
            base.OnHoverExit(player);

            Crosshair.ResetAlpha();
            UiController.Instance.SetItemDescription("");
        }

        public override bool IsInteracting(GameObject player)
        {
            return Player != null;
        }
    }
}
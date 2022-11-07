using System.Linq;
using Managers;
using Mono.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public enum FacingDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public class PlayerController : MonoBehaviour
    {
        [Range(2, 5)] [SerializeField] float runSpeed = 4;
        [SerializeField] KeyCode interactionKey = KeyCode.Space;
        [SerializeField] private Animator[] animatorControllers;
    
        // Cached component references
        private Rigidbody2D _myRigidBody;

        // String const
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        // Initialize variables
        private float _xDirection;
        private float _yDirection;
        private bool _playerMoving;
        private FacingDirection _facingDirection = FacingDirection.Down;

        private static readonly int XDirection = Animator.StringToHash("xDirection");
        private static readonly int YDirection = Animator.StringToHash("yDirection");
        private static readonly int IdleDown = Animator.StringToHash("IdleDown");
        private static readonly int IdleUp = Animator.StringToHash("IdleUp");
        private static readonly int IdleLeft = Animator.StringToHash("IdleLeft");
        private static readonly int IdleRight = Animator.StringToHash("IdleRight");
        private static readonly int MovingTrigger = Animator.StringToHash("MovingTrigger");

        public FacingDirection FacingDirection
        {
            get => _facingDirection;
        }
        public bool InteractingWithShopper { get; set; }
        public bool ShopAccesed { get; set; }
        public Collection<ShopItem> EquipedItems { get; } = new();
        public Collection<ShopItem> OwnedItems { get; } = new();

        public Rigidbody2D MyRigidBody
        {
            get => _myRigidBody;
            set => _myRigidBody = value;
        }

        private void Awake()
        {
            _myRigidBody = GetComponent<Rigidbody2D>();
            GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (!ShopAccesed)
            {
                MovePlayer();
            }

            if (!InteractingWithShopper) return;
            if (Input.GetKeyDown(interactionKey))
            {
                ManagerLocator.Instance.UImanager.OpenCloseShop(true);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ManagerLocator.Instance.UImanager.OpenCloseShop(false);
            }
        }

        public void UpdatePlayerAnimation(Vector2 playerVelocity)
        {
            if (playerVelocity != Vector2.zero)
            {
                if (_xDirection > 0)
                {
                    _facingDirection = FacingDirection.Right;
                }
                else if (_xDirection < 0)
                {
                    _facingDirection = FacingDirection.Left;
                }
                else if (_yDirection > 0)
                {
                    _facingDirection = FacingDirection.Up;
                }
                else if (_yDirection < 0)
                {
                    _facingDirection = FacingDirection.Down;
                }

                if (!_playerMoving)
                {
                    animatorControllers.SetTriggers(MovingTrigger);
                }
                _playerMoving = true;
            }
            else
            {
                if (!_playerMoving) return;
                switch (_facingDirection)
                {
                    case FacingDirection.Down:
                        animatorControllers.SetTriggers(IdleDown);
                        break;
                    case FacingDirection.Up:
                        animatorControllers.SetTriggers(IdleUp);
                        break;
                    case FacingDirection.Left:
                        animatorControllers.SetTriggers(IdleLeft);
                        break;
                    case FacingDirection.Right:
                        animatorControllers.SetTriggers(IdleRight);
                        break;
                }

                _playerMoving = false;
            }
        }

        public void UpdateCloths()
        {
            foreach (var animatorController in animatorControllers)
            {
                if (animatorController.gameObject.CompareTag("Player") || animatorController.gameObject.name == "Hair") continue;
                if (EquipedItems.Select(x => x.name == animatorController.gameObject.name).Any(v => v))
                {
                    animatorController.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    animatorController.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }

                    
            }
        }

        private void MovePlayer()
        {
            _xDirection = Input.GetAxis(Horizontal);
            _yDirection = Input.GetAxis(Vertical);

            animatorControllers.SetFloats(boolHash: XDirection, _xDirection);
            animatorControllers.SetFloats(boolHash: YDirection, _yDirection);
            
            Vector2 playerVelocityVector = new Vector2(_xDirection, _yDirection);
            _myRigidBody.velocity = playerVelocityVector.normalized * runSpeed;
            UpdatePlayerAnimation(playerVelocityVector);
        }
    }
}
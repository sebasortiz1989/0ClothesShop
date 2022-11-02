using UnityEngine;
using UnityEngine.Serialization;

public enum FacingDirection
{
    Up,
    Down,
    Left,
    Right
}

public class PlayerMovement : MonoBehaviour
{
    // Cached component references
    private Rigidbody2D _myRigidBody;
    private Animator _myAnimator;
    private SpriteRenderer _mySprite;

    // String const
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    // Initialize variables
    private float _xDirection;
    private float _yDirection;
    private bool _playerMoving;
    private bool _shopAccesed;
    private Vector2 _playerVelocity;
    private FacingDirection _facingDirection = FacingDirection.Down;

    [Range(2, 5)] 
    [SerializeField] float runSpeed = 4;
    [SerializeField] KeyCode interactionKey = KeyCode.Space;

    private static readonly int MovingRight = Animator.StringToHash("MovingRight");
    private static readonly int MovingLeft = Animator.StringToHash("MovingLeft");
    private static readonly int MovingUp = Animator.StringToHash("MovingUp");
    private static readonly int MovingDown = Animator.StringToHash("MovingDown");
    private static readonly int IdleDown = Animator.StringToHash("IdleDown");
    private static readonly int IdleUp = Animator.StringToHash("IdleUp");
    private static readonly int IdleLeft = Animator.StringToHash("IdleLeft");
    private static readonly int IdleRight = Animator.StringToHash("IdleRight");
    
    public FacingDirection FacingDirection
    {
        get => _facingDirection;
        set
        {
            if (_facingDirection == value && _playerMoving)
            {
                return;
            }

            _facingDirection = value;
            switch (_facingDirection)
            {
                case FacingDirection.Down:
                    _myAnimator.SetTrigger(MovingDown);
                    break;
                case FacingDirection.Up:
                    _myAnimator.SetTrigger(MovingUp);
                    break;
                case FacingDirection.Left:
                    _myAnimator.SetTrigger(MovingLeft);
                    break;
                case FacingDirection.Right:
                    _myAnimator.SetTrigger(MovingRight);
                    break;
            }
        }
    }
    public bool interactingWithShopper = false;

    private void Awake()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _mySprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (!_shopAccesed)
        {
            MovePlayer();
        }

        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Access shop!!!");
            _shopAccesed = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Shop exited!!!");
            _shopAccesed = false;
        }
    }

    private void MovePlayer()
    {
        _xDirection = Input.GetAxis(Horizontal);
        _yDirection = Input.GetAxis(Vertical);

        _playerVelocity = new Vector2(_xDirection, _yDirection);
        _myRigidBody.velocity = _playerVelocity.normalized * runSpeed;

        if (_playerVelocity != Vector2.zero)
        {
            if (_xDirection > 0)
            {
                FacingDirection = FacingDirection.Right;
            }
            else if (_xDirection < 0)
            {
                FacingDirection = FacingDirection.Left;
            }
            else if (_yDirection > 0)
            {
                FacingDirection = FacingDirection.Up;
            }
            else if (_yDirection < 0)
            {
                FacingDirection = FacingDirection.Down;
            }

            _playerMoving = true;
        }
        else
        {
            if (!_playerMoving) return;
            switch (_facingDirection)
            {
                case FacingDirection.Down:
                    _myAnimator.SetTrigger(IdleDown);
                    break;
                case FacingDirection.Up:
                    _myAnimator.SetTrigger(IdleUp);
                    break;
                case FacingDirection.Left:
                    _myAnimator.SetTrigger(IdleLeft);
                    break;
                case FacingDirection.Right:
                    _myAnimator.SetTrigger(IdleRight);
                    break;
            }

            _playerMoving = false;
        }
    }
}

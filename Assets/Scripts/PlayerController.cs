using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float speed = 6f;
    private PlayerInputs _input;
    private Animator _Animator;
    private Rigidbody _Rigidbody;
    private AudioSource _AudioSource;
    private CharacterController _characterController;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;


    void Start()
    {
        _Animator = GetComponent<Animator>();
        _Rigidbody = GetComponent<Rigidbody>();
        _AudioSource = GetComponent<AudioSource>();
        _input = GetComponent<PlayerInputs>();
        _characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Returns the player's transform component
    /// </summary>
    public Vector3 GetPlayerTop()
    {
        return transform.position + Vector3.up;
    }

    void FixedUpdate()
    {
        float horizontal = _input.move.x;
        float vertical = _input.move.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        Animating(m_Movement.x, m_Movement.y);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }
 
    void OnAnimatorMove()
    {
        _Rigidbody.MovePosition(_Rigidbody.position + m_Movement * speed);
        _Rigidbody.MoveRotation(m_Rotation);
    }
    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        _Animator.SetBool("IsWalking", walking);
    }
}

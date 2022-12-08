using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerController : AbstractSingleton<PlayerController>
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float turnSpeed = 20f;
        [SerializeField] private float speed = 6f;


        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private Quaternion _rotation = Quaternion.identity;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
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
            Animating(InputManager.Instance.move.x, InputManager.Instance.move.y);
            Move(InputManager.Instance.move.x, InputManager.Instance.move.y);
            Turning();
        }

        private void Turning()
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, _movement, turnSpeed * Time.deltaTime, 0f);
            _rotation = Quaternion.LookRotation(desiredForward);
            _rigidbody.MoveRotation(_rotation);
        }

        void Move(float horizontal, float vertical)
        {
            _movement.Set(horizontal, 0, vertical);
            _movement = _movement.normalized * speed * Time.deltaTime;
            _rigidbody.MovePosition(_rigidbody.position + _movement);
        }

        void Animating(float horizontal, float vertical)
        {
            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            animator.SetBool("IsWalking", isWalking);
        }
    }
}

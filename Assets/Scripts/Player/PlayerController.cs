using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class PlayerController : ThinkingPlaceable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float turnSpeed = 20f;
        [SerializeField] private float speed = 6f;
        [SerializeField] private float range = 15f;
        public GameObject TransformTarget;

        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private Quaternion _rotation = Quaternion.identity;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            pType = Placeable.PlaceableType.Unit;

            //find references to components
            animator = GetComponentInChildren<Animator>();
            //navMeshAgent = GetComponent<NavMeshAgent>(); //will be disabled until Activate is called
            audioSource = GetComponent<AudioSource>();
            damage = 1;
        }

        /// <summary>
        /// Returns the player's transform component
        /// </summary>
        public Vector3 GetPlayerTop()
        {
            return transform.position + Vector3.up;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);

        }
        void FixedUpdate()
        {
            UpdateTarget();
            Animating(InputManager.Instance.move.x, InputManager.Instance.move.y);
            Move(InputManager.Instance.move.x, InputManager.Instance.move.y);
            Turning();
            if (state == States.Idle)
            {
                StartAttack();
            }
        }

        private void UpdateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (var enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.GetComponent<ThinkingPlaceable>();
            }
        }


        private void Turning()
        {
            if (state != States.Seeking)
            {
                Vector3 targetDirection = target.transform.position - transform.position;

                // The step size is equal to speed times frame time.
                float singleStep = speed * Time.deltaTime;

                // Rotate the forward vector towards the target direction by one step
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);

                // Draw a ray pointing at our target in
                Debug.DrawRay(transform.position, newDirection, Color.red);

                // Calculate a rotation a step closer to the target and applies rotation to this object
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                Vector3 desiredForward = Vector3.RotateTowards(transform.forward, _movement, turnSpeed * Time.deltaTime, 0f);
                _rotation = Quaternion.LookRotation(desiredForward);
                _rigidbody.MoveRotation(_rotation);
            }

        }
        public override void StartAttack()
        {

            float timeBetweenBullets = 0.15f;
            float angleBetweenBullets = 10f;
            int numberOfBullets = 1;
            base.StartAttack();
            if (Time.time >= lastBlowTime + attackRatio)
            {
                DealBlow();
                for (int i = 0; i < numberOfBullets; i++)
                {
                    float angle = i * angleBetweenBullets - ((angleBetweenBullets / 2) * (numberOfBullets - 1));
                    Quaternion rot = transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);
                    GameObject instantiatedBullet = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, rot) as GameObject;
                    instantiatedBullet.GetComponent<Projectile>().target = target;
                    instantiatedBullet.GetComponent<Projectile>().damage = damage;

                    //instantiatedBullet.GetComponent<Bullet>().piercing = piercing;
                    //instantiatedBullet.GetComponent<Bullet>().bounce = bounce;
                    //instantiatedBullet.GetComponent<Bullet>().bulletColor = bulletColor;
                }

            }
           
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
            if (isWalking)
            {
                state = States.Seeking;
            }
            else
            {
                state = States.Idle;

            }
            animator.SetBool("IsWalking", isWalking);
        }
    }
}

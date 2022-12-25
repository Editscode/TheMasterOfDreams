using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : ThinkingPlaceable
    {
        private Animator animator;
        [SerializeField] private float turnSpeed = 20f;
        [SerializeField] private float speed = 6f;
        [SerializeField] private float range = 15f;
        [SerializeField] private float playerDamage = 1f;
        [SerializeField] private float playerAttackRatio = 0.8f;

        private Rigidbody _rigidbody;
        private Vector3 _movement;
        private Quaternion _rotation = Quaternion.identity;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            pType = Placeable.PlaceableType.Unit;
            animator = GetComponentInChildren<Animator>();
            faction = Faction.Player;
            audioSource = GetComponent<AudioSource>();
            damage = playerDamage;
            attackRatio = playerAttackRatio;
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
                if (distanceToEnemy < shortestDistance && enemy.GetComponent<ThinkingPlaceable>().state != States.Dead)
                {
                    if (enemy.GetComponent<ThinkingPlaceable>().faction == Faction.Opponent)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }
            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.GetComponent<ThinkingPlaceable>();
            }
            else
            {
                target = null;
            }
        }

        //Starts the attack animation, and is repeated according to the Unit's attackRatio
        public override void DealBlow()
        {
            base.DealBlow();

            animator.SetTrigger("Attack");
            transform.forward = (target.transform.position - transform.position).normalized; //turn towards the target
        }
        private void Turning()
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, _movement, turnSpeed * Time.deltaTime, 0f);
            _rotation = Quaternion.LookRotation(desiredForward);
            _rigidbody.MoveRotation(_rotation);
        }
        public override void StartAttack()
        {
            if (target == null) return;
            if (target.state == States.Dead) { return; }

            transform.forward = (target.transform.position - transform.position).normalized; //turn towards the target


            base.StartAttack();
            float angleBetweenBullets = 10f;
            int numberOfBullets = 3;
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
                    instantiatedBullet.GetComponent<Projectile>().faction = faction;

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
            animator.SetBool("IsMoving", isWalking);
        }
    }
}

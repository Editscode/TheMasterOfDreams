using Assets.Scripts.Placeables;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public ThinkingPlaceable target;
        [HideInInspector] public float damage;
        private float speed = 3f;
        private float progress = 0f;
        private Vector3 offset = new Vector3(0f, 1.2f, 0f);
        private Vector3 initialPosition;

        private void Start()
        {
            GetComponent<Rigidbody>().velocity = transform.forward * 20f;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag("Environment") || collision.transform.CompareTag("Enemy"))
            {
                if (collision.transform.CompareTag("Enemy"))
                {
                    float newHP = collision.transform.GetComponent<Unit>().SufferDamage(damage);
                }
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(gameObject, 0.2f);
            }
        }

        //private void Update()
        //{
        //    if (progress >= 1f)
        //    {
        //        if (target != null)
        //        {
        //            if (target.state !=
        //                ThinkingPlaceable.States.Dead) //target might be dead already as this projectile is flying
        //            {
        //                // Debug.Log("My target " + target.name + " is dead", gameObject);
        //                float newHP = target.SufferDamage(damage);
        //                //Debug.Log("My target " + target.name + " is newHP: " + damage.ToString(), gameObject);
        //                //target.healthBar.SetHealth(newHP);
        //            }
        //        }

        //        Destroy(gameObject);
        //    }
        //    else
        //    {
        //        Move();
        //    }
        //}

        private void Awake()
        {
            initialPosition = transform.position;
        }

        public float Move()
        {
            if (target != null)
            {
                progress += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(initialPosition, target.transform.position + offset, progress);

            }
            else
            {
                Destroy(gameObject);
            }

            return progress;
        }
    }
}
﻿using Unity.VisualScripting;
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


        private void Update()
        {

            Move();
            if (progress >= 1f)
            {
                if (target.state != ThinkingPlaceable.States.Dead) //target might be dead already as this projectile is flying
                {
                   // Debug.Log("My target " + target.name + " is dead", gameObject);
                    float newHP = target.SufferDamage(damage);
                    Debug.Log("My target " + target.name + " is newHP: "  + damage.ToString(), gameObject);
                    //target.healthBar.SetHealth(newHP);
                }
                Destroy(gameObject);
            }
        }

        private void Awake()
        {
            initialPosition = transform.position;
        }

        public float Move()
        {
            progress += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(initialPosition, target.transform.position + offset, progress);

            return progress;
        }
    }
}
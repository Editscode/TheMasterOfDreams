﻿using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Placeables
{
    [CreateAssetMenu(fileName = "NewPlaceable", menuName = "The master of dreams/Placeable Data")]

    public class PlaceableData : ScriptableObject
    {

        [Header("Common")]
        public Placeable.PlaceableType pType;
        public GameObject associatedPrefab;
        public GameObject alternatePrefab;

        [Header("Units and Buildings")]
        public ThinkingPlaceable.AttackType attackType = ThinkingPlaceable.AttackType.Melee;
        public Placeable.PlaceableTarget targetType = Placeable.PlaceableTarget.Both;
        public float attackRatio = 1f; //time between attacks
        public float damagePerAttack = 2f; //damage each attack deals
        public float attackRange = 1f;
        public float hitPoints = 10f; //when units or buildings suffer damage, they lose hitpoints
        public AudioClip attackClip, dieClip;

        [Header("Units")]
        public float speed = 5f; //movement speed

        [Header("Obstacles and Spells")]
        public float lifeTime = 5f; //the maximum lifetime of the Placeable. Especially important for obstacle types, so they are removed after a while

        [Header("Spells")]
        public float damagePerSecond = 1f; //damage per second for non-instantaneous spells

        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}
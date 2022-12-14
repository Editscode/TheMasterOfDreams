using Assets.Scripts.Placeables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WaveData", menuName = "The master of dreams/Wave Data")]
    public class WaveData : ScriptableObject
    {
        [Header("List of Placeables")]
        public PlaceableData[] placeablesData; //link to all the Placeables that this card spawns
        public Vector3[] relativeOffsets; //the relative offsets (from cursor) where the placeables will be dropped
    }
}

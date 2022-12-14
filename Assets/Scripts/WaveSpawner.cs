using Assets.Scripts.Placeables;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class WaveSpawner : MonoBehaviour
    {

        public WaveData[] WaveDatas;

        private CPUUnit unitCpu;

        public float timeBetweenWaves = 5f;
        private float countdown = 2f;

        private int waveIndex = 0;

        public UnityAction<PlaceableData[], Vector3[], Placeable.Faction> OnCardUsed;

        private void Start()
        {
            unitCpu = GetComponent<CPUUnit>();
        }

        private void Update()
        {
            if (unitCpu.allThinkingPlaceables.Count > 0) return;

            if (waveIndex == WaveDatas.Length) this.enabled = false;


            if (countdown <= 0f)
            {
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves;
                return;
            }

            countdown -= Time.deltaTime;

            countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        }
        IEnumerator SpawnWave()
        {
            PlaceableData[] placeablesData = WaveDatas[waveIndex].placeablesData;
            Vector3[] relativeOffsets = WaveDatas[waveIndex].relativeOffsets;

            if (OnCardUsed != null)
                OnCardUsed(WaveDatas[waveIndex].placeablesData, WaveDatas[waveIndex].relativeOffsets, Placeable.Faction.Player); //GameManager picks this up to spawn the actual Placeable
            yield return new WaitForSeconds(0);
            waveIndex++;
        }
    }
}


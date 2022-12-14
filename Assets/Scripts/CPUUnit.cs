using Assets.Scripts;
using Assets.Scripts.Placeables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUUnit : MonoBehaviour
{
    // Start is called before the first frame update
    private WaveSpawner waveSpawner;
    public List<ThinkingPlaceable> allThinkingPlaceables;
    public bool gameOver = false;
    public ThinkingPlaceable targetToPass; //ref
    private void Awake()
    {
        allThinkingPlaceables = new List<ThinkingPlaceable>();
        waveSpawner = GetComponent<WaveSpawner>();

        waveSpawner.OnCardUsed += UseWave;
    }

    private void Update()
    {
        
        if (gameOver)
            return;

        
        ThinkingPlaceable p; //ref

        for (int pN = 0; pN < allThinkingPlaceables.Count; pN++)
        {
            p = allThinkingPlaceables[pN];

            switch (p.state)
            {
                case ThinkingPlaceable.States.Idle:
                    //this if is for innocuous testing Units
                    if (p.targetType == Placeable.PlaceableTarget.None)
                        break;

                    //find closest target and assign it to the ThinkingPlaceable
                    if (targetToPass == null) Debug.LogError("No more targets!"); //this should only happen on Game Over
                    p.SetTarget(targetToPass);
                    p.Seek();
                    break;


                case ThinkingPlaceable.States.Seeking:
                    if (p.IsTargetInRange())
                    {
                        p.StartAttack();
                    }
                    else
                    {
                        p.Seek();
                    }
                    break;


                case ThinkingPlaceable.States.Attacking:
                    if (p.IsTargetInRange())
                    {
                        if (Time.time >= p.lastBlowTime + p.attackRatio)
                        {
                            p.DealBlow();
                            //Animation will produce the damage, calling animation events OnDealDamage and OnProjectileFired. See ThinkingPlaceable
                        }
                    }
                    else
                    {
                        p.Seek();
                    }
                    break;

                case ThinkingPlaceable.States.Dead:
                    Debug.LogError("A dead ThinkingPlaceable shouldn't be in this loop");
                    break;
            }
        }
    }
    private void UseWave(PlaceableData[] placeablesData, Vector3[] position, Placeable.Faction pFaction)
    {
        for (int pNum = 0; pNum < placeablesData.Length; pNum++)
        {
            PlaceableData pDataRef = placeablesData[pNum];
            Quaternion rot = Quaternion.Euler(0f, 180f, 0f);
            GameObject prefabToSpawn = pDataRef.associatedPrefab;
            GameObject newPlaceableGO = Instantiate<GameObject>(prefabToSpawn, position[pNum], rot);

            SetupPlaceable(newPlaceableGO, pDataRef);
        }
        //audioManager.PlayAppearSFX(position);
    }
    private void SetupPlaceable(GameObject go, PlaceableData pDataRef)
    {
        var uScript = go.GetComponent<Unit>();
        uScript.Activate(Placeable.Faction.Opponent, pDataRef); //enables NavMeshAgent
        uScript.OnDealDamage += OnPlaceableDealtDamage;
        allThinkingPlaceables.Add(uScript);
        go.GetComponent<Placeable>().OnDie += OnPlaceableDead;
    }
    private void OnPlaceableDealtDamage(ThinkingPlaceable p)
    {
        if (p.target.state == ThinkingPlaceable.States.Dead) return;

        var newHealth = p.target.SufferDamage(p.damage);
        p.target.healthBar.SetHealth(newHealth);
    }
    private void OnPlaceableDead(Placeable p)
    {
        p.OnDie -= OnPlaceableDead; //remove the listener

        var u = (Unit)p;

        allThinkingPlaceables.Remove(u);
        u.OnDealDamage -= OnPlaceableDealtDamage;

        StartCoroutine(Dispose(u));
    }
    private IEnumerator Dispose(ThinkingPlaceable p)
    {
        yield return new WaitForSeconds(3f);

        Destroy(p.gameObject);
    }

}

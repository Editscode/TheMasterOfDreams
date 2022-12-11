using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Placeables;
using UnityEngine;

public class SetUnitData : MonoBehaviour
{
    private Unit unit;
    public PlaceableData data;
    public ThinkingPlaceable player;
    void Start()
    {
        unit = GetComponent<Unit>();
        unit.Activate(Placeable.Faction.Opponent,data);
        unit.SetTarget(player);
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (unit.state)
        {
            case ThinkingPlaceable.States.Idle:
                //this if is for innocuous testing Units
                if (unit.targetType == Placeable.PlaceableTarget.None)
                    break;

                //find closest target and assign it to the ThinkingPlaceable
                //bool targetFound = FindClosestInList(unit.transform.position, GetAttackList(unit.faction, unit.targetType), out targetToPass);
                if (!true) Debug.LogError("No more targets!"); //this should only happen on Game Over
                unit.SetTarget(player);
                unit.Seek();
                break;


            case ThinkingPlaceable.States.Seeking:
                if (unit.IsTargetInRange())
                {
                    unit.StartAttack();
                }
                else
                {
                    unit.Seek();
                }
                break;


            case ThinkingPlaceable.States.Attacking:
                if (unit.IsTargetInRange())
                {
                    if (Time.time >= unit.lastBlowTime + unit.attackRatio)
                    {
                        unit.DealBlow();
                        //Animation will produce the damage, calling animation events OnDealDamage and OnProjectileFired. See ThinkingPlaceable
                    }
                }
                else
                {
                    unit.Seek();
                }
                break;

            case ThinkingPlaceable.States.Dead:
                Debug.LogError("A dead ThinkingPlaceable shouldn't be in this loop");
                break;
        }
    }
}

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour, IPunObservable
{

    [SerializeField] private Unit firstUnitType;
    [SerializeField] private Unit secondUnitType;
    [SerializeField] private Transform[] leftSpawnPoints;
    [SerializeField] private Transform[] rightSpawnPoints;
    

    private List<Unit> spawnedUnits = new List<Unit>();

    private int unitCount = 1;
    private bool isLeftSideFree = true;

    public int UnitCount
    {
        set 
        {
            if(value < 1)
            {
                unitCount = 1;
                return;
            }
            else if(value > 3)
            {
                unitCount = 3;
            }
            else
            {
                unitCount = value;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isLeftSideFree);
        }
        else
        {
            isLeftSideFree = (bool)stream.ReceiveNext();
        }
    }

    public Unit[] GetAllUnits()
    {
        return spawnedUnits.ToArray();
    }

    public void Spawn()
    {
        if (isLeftSideFree)
        {
            SpawnUnits(unitCount, firstUnitType, leftSpawnPoints);
            isLeftSideFree = false;
        }
        else
        {
            SpawnUnits(unitCount, secondUnitType, rightSpawnPoints);
        }
    }

    private void SpawnUnits(int count, Unit unit, Transform[] spawnPositions)
    {
        if(count > spawnPositions.Length)
        {
            count = spawnPositions.Length;
        }

        for(int i = 0; i < count; i++)
        {
            GameObject unitGO = PhotonNetwork.Instantiate(unit.name, spawnPositions[i].position, Quaternion.identity);
            Unit newUnit;
            if(unitGO.TryGetComponent<Unit>(out newUnit))
            {
                spawnedUnits.Add(newUnit);
            }
        }
    }

}

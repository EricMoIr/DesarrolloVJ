using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaceData;

public class DestroyObjDistinto : MonoBehaviour
{
    [SerializeField]
    private int numberOfPlayers;

    void Awake()
    {
        if (RaceDataHolder.isNew.NumberOfPlayers != numberOfPlayers)
            Destroy(gameObject);
    }
}


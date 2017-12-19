using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaceData;

public class DestroyObjMayores : MonoBehaviour
{
    [SerializeField]
    private int numberPlayer;

    void Awake()
    {
        if (RaceDataHolder.isNew.NumberOfPlayers < numberPlayer)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}


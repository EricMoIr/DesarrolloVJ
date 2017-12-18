using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RaceData;

public class DestroyObjDistinto : MonoBehaviour
{
    [SerializeField]
    private int numberOfPlayers;
    public static int s_numberOfPlayers;
    void Awake()
    {
        s_numberOfPlayers = RaceDataHolder.isNew.NumberOfPlayers;
        if (RaceDataHolder.isNew.NumberOfPlayers != numberOfPlayers)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        print("pase por aca");
    }
}


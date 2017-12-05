using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps.asset", menuName = "Power Ups")]
public class PowerUpsHolderObject : ScriptableObject
{
    public Transform[] powerUps;
    internal static readonly int MISSILE_POSITION = 0;
    internal static readonly int TURBO_POSITION = 1;
}

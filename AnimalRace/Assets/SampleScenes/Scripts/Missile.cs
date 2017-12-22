using System;
using Assets.Scripts.Controllers;
using UnityEngine;

internal class Missile : SpecialPower
{
    public override string GetName()
    {
        return "Misil";
    }

    public override string ToString()
    {
        return "MissilePrefab";
    }

    internal override void Activate(CarBehavior car)
    {
        GameObject missile = ObjectPooler.GetInstance().GetInactiveObjectOfType(ToString());
        missile.transform.position = car.missileLauncher.position;
        missile.transform.forward = car.missileLauncher.forward;
        missile.SetActive(true);
    }
}
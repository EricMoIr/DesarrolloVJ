using Assets.Scripts.Controllers;
using UnityEngine;

internal class Mine : SpecialPower
{
    public override string GetName()
    {
        return "Caca";
    }

    public override string ToString()
    {
        return "MinePrefab";
    }

    internal override void Activate(CarBehavior car)
    {
        GameObject mine = ObjectPooler.GetInstance().GetInactiveObjectOfType(ToString());
        mine.transform.position = car.MineLauncher.position;
        mine.SetActive(true);
    }
}
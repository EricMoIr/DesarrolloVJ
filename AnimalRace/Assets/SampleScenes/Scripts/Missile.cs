using Assets.Scripts.Controllers;
using UnityEngine;

internal class Missile : SpecialPower
{
    public override string ToString()
    {
        return "MissilePrefab";
    }

    internal override void Activate(CarBehavior car)
    {
        GameObject missile = ObjectPooler.GetInstance().GetInactiveObjectOfType(ToString());
        float missileLengthHorizontally = 0f;//missile.GetComponent<Renderer>().bounds.extents.z;
        missile.transform.position = car.GetLauncherPosition() + (car.transform.forward * missileLengthHorizontally);
        missile.transform.forward = car.transform.forward;
        missile.SetActive(true);
    }
}
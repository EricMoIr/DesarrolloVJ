
using UnityEngine;

internal class Missile : SpecialPower
{
    internal override void Activate(CarBehavior car)
    {
        Transform missileHax = car.GetPowerUp(PowerUpsHolderObject.MISSILE_POSITION);
        Transform missile = missileHax.GetChild(0);
        float missileLengthHorizontally = missile.GetComponent<Renderer>().bounds.extents.z;
        Vector3 missilePosition = car.GetLauncherPosition() + (car.transform.forward * missileLengthHorizontally);
        Object.Instantiate(
            missileHax,
            missilePosition,
            car.GetLauncherRotation());//missileHax.rotation);
    }
}
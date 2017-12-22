using Assets.Scripts.Controllers;
using System;
using UnityEngine;

internal class Shield : SpecialPower, AbnormalStatus
{
    private double remainingDuration;
    public double Duration
    {
        get
        {
            return remainingDuration;
        }
    }

    public void Deactivate(CarBehavior car)
    {
        car.HasShield = false;
    }

    public override string GetName()
    {
        return "Escudo";
    }

    public void ReduceTime(float deltaTime)
    {
        remainingDuration -= deltaTime;
    }

    public override string ToString()
    {
        return "ShieldPrefab";
    }

    internal override void Activate(CarBehavior car)
    {
        car.HasShield = true;
        remainingDuration = 5f;
        car.AddAbnormalStatus(this);
        GameObject shield = ObjectPooler.GetInstance().GetInactiveObjectOfType(ToString());
        shield.transform.position = car.transform.position;
        ShieldBehavior script = shield.GetComponent<ShieldBehavior>();
        script.time = DateTime.Now;
        script.player = car.gameObject;
        shield.SetActive(true);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MissileCollisionStatus : AbnormalStatus
{
    private const float STOP_MULTIPLIER = 0.001f;
    private const double STOP_DURATION = 5;
    private double remainingDuration;
    public double Duration
    {
        get
        {
            return remainingDuration;
        }
    }

    internal void Activate(CarBehavior car)
    {
        remainingDuration = STOP_DURATION;
        car.AddAbnormalStatus(this);
        car.ChangeMaxSpeed(STOP_MULTIPLIER);
    }

    public void Deactivate(CarBehavior car)
    {
        car.RemoveAbnormalStatus(this);
        car.ChangeMaxSpeed(1 / STOP_MULTIPLIER);
    }

    public void ReduceTime(float deltaTime)
    {
        remainingDuration -= deltaTime;
    }
}

using System;

internal class Turbo : SpecialPower, AbnormalStatus
{
    private const float TURBO_MULTIPLIER = 1.5f;
    private const double TURBO_DURATION = 3;
    private double remainingDuration;

    public double Duration
    {
        get
        {
            return remainingDuration;
        }
    }

    internal override void Activate(CarBehavior car)
    {
        remainingDuration = TURBO_DURATION;
        car.AddAbnormalStatus(this);
        car.ChangeMaxSpeed(TURBO_MULTIPLIER);
    }

    void AbnormalStatus.Deactivate(CarBehavior car)
    {
        car.RemoveAbnormalStatus(this);
        car.ChangeMaxSpeed(1 / TURBO_MULTIPLIER);
    }

    void AbnormalStatus.ReduceTime(float deltaTime)
    {
        remainingDuration -= deltaTime;
    }

    public override string ToString()
    {
        return "TurboPrefab";
    }
}
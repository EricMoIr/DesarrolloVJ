using System;
using System.Collections.Generic;

internal class SpecialPowerBuilder
{
    internal static SpecialPower CreateRandomPower(CarBehavior car)
    {
        Dictionary<SpecialPower, double> availablePowerUps = InitializePowerUpsWithProbabilities(car);
        return GetRandomPower(availablePowerUps);
    }

    private static Dictionary<SpecialPower, double> InitializePowerUpsWithProbabilities(CarBehavior car)
    {
        Dictionary<SpecialPower, double> powerUpsWithProbabilities = new Dictionary<SpecialPower, double>();
        for (var i = 0; i < ConstantsHelper.ALL_POWERUPS.Length; i++)
        {
            powerUpsWithProbabilities.Add(
                ConstantsHelper.ALL_POWERUPS[i],
                ConstantsHelper.ALL_POWERUPS[i].GetProbability(car));
        }
        return powerUpsWithProbabilities;
    }

    private static SpecialPower GetRandomPower(Dictionary<SpecialPower, double> availablePowerUps)
    {
        Random random = new Random();
        List<SpecialPower> powerUps = new List<SpecialPower>();
        foreach (KeyValuePair<SpecialPower, double> entry in availablePowerUps)
        {
            for (int i = 0; i < entry.Value; i++)
            {
                powerUps.Add(entry.Key);
            }
        }
        int randomPosition = random.Next(powerUps.Count);
        return powerUps[randomPosition];
    }
}
using System;

internal abstract class SpecialPower
{
    internal virtual double GetProbability(CarBehavior car)
    {
        return ConstantsHelper.DEFAULT_PROBABILITY;
        //Ejemplo de como lo usaria
        /*switch (car.Position)
        {
            case 1:
                return ConstantsHelper.DEFAULT_PROBABILITY;
            default:
                return ConstantsHelper.DEFAULT_PROBABILITY;
        }*/
    }

    internal abstract void Activate(CarBehavior car);
}
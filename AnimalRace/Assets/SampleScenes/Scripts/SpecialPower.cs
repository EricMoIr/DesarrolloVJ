using System;
using UnityEngine;
using UnityEngine.UI;

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

    public abstract override string ToString();

    public abstract string GetName();

    internal abstract void Activate(CarBehavior car);
}
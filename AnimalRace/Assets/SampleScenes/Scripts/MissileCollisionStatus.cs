using Assets.Scripts.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MissileCollisionStatus : AbnormalStatus
{
    private const float STOP_MULTIPLIER = 0.001f;
    private const double STOP_DURATION = 5;
    private double remainingDuration;
    private GameObject stars;
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
        ShowStars(car);
    }

    public void Deactivate(CarBehavior car)
    {
        car.RemoveAbnormalStatus(this);
        car.ChangeMaxSpeed(1 / STOP_MULTIPLIER);
        stars.SetActive(false);
    }

    public void ReduceTime(float deltaTime)
    {
        remainingDuration -= deltaTime;
    }

    private void ShowStars(CarBehavior car)
    {
        stars =
            ObjectPooler.GetInstance().GetInactiveObjectOfType("StunnedStatePrefab");
        Vector3 starsPosition = car.transform.position;
        starsPosition.y += car.GetComponent<BoxCollider>().size.y;
        stars.transform.position = starsPosition;
        stars.SetActive(true);
    }
}
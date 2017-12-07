using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class CarBehavior : MonoBehaviour
{
    [SerializeField]
    private PowerUpsHolderObject powerUps;
    private int lapCounter;
    private bool[] activeCheckpoints;
    private List<AbnormalStatus> abnormalStatuses = new List<AbnormalStatus>();
    private SpecialPower myPowerUp;
    private Transform missileLauncher;
    [SerializeField]
    private string FIRE_AXIS;
    [SerializeField]
    Text middleText;
    [SerializeField]
    Text timerText;
    [SerializeField]
    Text lapText;
    float lapTime;
    public PowerUpsHolderObject PowerUps { get { return powerUps; } }

    // Use this for initialization
    void Start()
    {
        lapCounter = 0;
        activeCheckpoints = new bool[] { false, false, false };
        middleText.text = "";
        timerText.text = "";
        lapText.text = "Lap: 1/2";
        lapTime = 0f;
    }

    internal void ChangeMaxSpeed(float TURBO_MULTIPLIER)
    {
        CarController stolenScript = gameObject.GetComponent<CarController>();
        stolenScript.Torque = stolenScript.Torque * TURBO_MULTIPLIER;
    }

    // Update is called once per frame
    void Update()
    {
        FireSpecialPower();
        ReduceAbnormalStatusTime();
        if(lapCounter < 2)
        {
            lapTime += Time.deltaTime;
            int milliseconds = (int)(lapTime * 1000) % 1000;
            int seconds = (int)(lapTime % 60);
            int minutes = (int)(lapTime / 60) % 60;

            string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds,milliseconds);
            timerText.text = timeText;
        }
        
    }

    void OnTriggerEnter(Collider otherObject)
    {
        PowerUpBoxTriggerEnter(otherObject);
        CheckpointTriggerEnter(otherObject);
    }

    private void CheckpointTriggerEnter(Collider otherObject)
    {
        if (IsCheckPoint(otherObject))
        {
            string checkPointNumber = otherObject.tag.Split('_')[1];
            switch (checkPointNumber)
            {
                case "1":
                    if (!activeCheckpoints[1] && !activeCheckpoints[2])
                    {
                        activeCheckpoints[0] = true;
                    }
                    break;
                case "2":
                    if (activeCheckpoints[0] && !activeCheckpoints[2])
                    {
                        activeCheckpoints[1] = true;
                    }
                    break;
                case "3":
                    if (activeCheckpoints[0] && activeCheckpoints[1])
                    {
                        activeCheckpoints[2] = true;
                        AddLap(otherObject);
                    }
                    break;
            }
        }
    }

    #region PowerUp

    private void FireSpecialPower()
    {
        if (Input.GetAxisRaw(FIRE_AXIS) != 0)
        {
            if (myPowerUp != null)
            {
                myPowerUp.Activate(this);
                myPowerUp = null;
            }
        }
    }
    private void PowerUpBoxTriggerEnter(Collider otherObject)
    {
        if (IsPowerUpBox(otherObject))
        {
            if (CanHaveNewPowerUp())
            {
                myPowerUp = SpecialPowerBuilder.CreateRandomPower(this);
            }
            RemoveObject(otherObject);
        }
    }
    internal void RemoveAbnormalStatus(AbnormalStatus status)
    {
        abnormalStatuses.Remove(status);
    }

    internal void AddAbnormalStatus(AbnormalStatus status)
    {
        abnormalStatuses.Add(status);
    }


    private void ReduceAbnormalStatusTime()
    {
        for (var i = 0; i < abnormalStatuses.Count; i++)
        {
            AbnormalStatus status = abnormalStatuses[i];
            status.ReduceTime(Time.deltaTime);
            if (status.Duration <= 0)
            {
                status.Deactivate(this);
            }
        }
    }

    private bool IsPowerUpBox(Collider otherObject)
    {
        return ConstantsHelper.POWERUP_BOX.Equals(otherObject.tag);
    }
    private bool CanHaveNewPowerUp()
    {
        return myPowerUp == null;
    }
    private void RemoveObject(Collider otherObject)
    {
        StartCoroutine(ShowBox(otherObject.gameObject));
        otherObject.gameObject.SetActive(false);
    }

    IEnumerator ShowBox(GameObject gameObj)
    {
        yield return new WaitForSeconds(5);
        gameObj.SetActive(true);
    }
    internal Vector3 GetLauncherPosition()
    {
        return missileLauncher.position;
    }
    internal Quaternion GetLauncherRotation()
    {
        return missileLauncher.transform.rotation;
    }
    internal Transform GetPowerUp(int powerUp)
    {
        return PowerUps.powerUps[powerUp];
    }

    #endregion

    private void AddLap(Collider checkPoint)
    {
        lapCounter++;
        if (lapCounter < 2)
        {
            middleText.text = "Lap: " + (lapCounter + 1) + "/2";
            lapText.text = "";
            StartCoroutine(ResetTexts());
        } 
        ResetCheckPointList();
        CheckIfRaceIsOver(checkPoint);

    }

    IEnumerator ResetTexts()
    {
        yield return new WaitForSeconds(3);
        middleText.text = "";
        lapText.text = "Lap: " + (lapCounter + 1) + "/2";

    }

    private void ResetCheckPointList()
    {
        for(int i = 0; i < activeCheckpoints.Length; i++)
        {
            activeCheckpoints[i] = false;
        }
    }

    private void CheckIfRaceIsOver(Collider checkPoint)
    {

        if(lapCounter == 2)
        {
            int position = checkPoint.GetComponent<CheckPointBehavior>().nextPos;
            middleText.text = position + "/2";
            checkPoint.GetComponent<CheckPointBehavior>().SendMessage("AddNextPos", 1);
            //this.GetComponent<Rigidbody>().freezeRotation(true);
            //this.GetComponent<Rigidbody>().freezeRotation(1);
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ| RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private bool IsCheckPoint(Collider otherObject)
    {
        return otherObject.tag.Contains("CheckPoint");
    }
}

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
    private int nextCheckPoint;
    private float nextCheckPointDistance;
    private bool[] activeCheckpoints;
    private List<AbnormalStatus> abnormalStatuses = new List<AbnormalStatus>();
    private SpecialPower myPowerUp;
    private Transform missileLauncher;
    private bool controlsEnabled;
    [SerializeField]
    private string FIRE_AXIS;
    [SerializeField]
    int lapNumbers;
    [SerializeField]
    Text middleText;
    [SerializeField]
    Text timerText;
    [SerializeField]
    Text lapText;
    //[SerializeField]
    //Text countDownText;
    float lapTime;
    public PowerUpsHolderObject PowerUps { get { return powerUps; } }

    // Use this for initialization
    void Start()
    {
        lapCounter = 0;
        activeCheckpoints = new bool[] { false, false, false };
        middleText.text = "3";
        timerText.text = "";
        lapText.text = "Lap: 1/"+lapNumbers;
        lapTime = 0f;
        controlsEnabled = false;
        nextCheckPoint = 1;
        StartCoroutine(BeginCountDown());

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
        if (controlsEnabled)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        if (lapCounter < lapNumbers && controlsEnabled)
        {
            lapTime += Time.deltaTime;
            int milliseconds = (int)(lapTime * 1000) % 1000;
            int seconds = (int)(lapTime % 60);
            int minutes = (int)(lapTime / 60) % 60;

            string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds,milliseconds);
            timerText.text = timeText;
        }
        nextCheckPointDistance = CalculateDistanceToNextCheckPoint();
        
    }

    void OnTriggerEnter(Collider otherObject)
    {
        PowerUpBoxTriggerEnter(otherObject);
        CheckpointTriggerEnter(otherObject);
    }

    float CalculateDistanceToNextCheckPoint()
    {
        GameObject nextCPoint = GameObject.FindGameObjectsWithTag("CheckPoint_"+nextCheckPoint)[0];
        return Vector3.Distance(transform.position, nextCPoint.transform.position);
    }

    private void CheckpointTriggerEnter(Collider otherObject)
    {
        if (IsCheckPoint(otherObject))
        {
            string checkPointNumber = otherObject.tag.Split('_')[1];
            switch (checkPointNumber)
            {
                case "1":
                    if (activeCheckpoints[1] && activeCheckpoints[2])
                    {
                        activeCheckpoints[0] = true;
                        AddLap(otherObject);
                    }
                    nextCheckPoint = 2;
                    break;
                case "2":
                    if (!activeCheckpoints[0] && !activeCheckpoints[2])
                    {
                        activeCheckpoints[1] = true;
                    }
                    nextCheckPoint = 3;
                    break;
                case "3":
                    if (!activeCheckpoints[0] && activeCheckpoints[1])
                    {
                        activeCheckpoints[2] = true;
                    }
                    nextCheckPoint = 4;
                    break;
                case "4":
                    nextCheckPoint = 5;
                    break;
                case "5":
                    nextCheckPoint = 1;
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
        if (lapCounter < lapNumbers)
        {
            middleText.text = "Lap: " + (lapCounter + 1) + "/"+lapNumbers;
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
        lapText.text = "Lap: " + (lapCounter + 1) + "/"+lapNumbers;

    }

    IEnumerator BeginCountDown()
    {
        yield return new WaitForSeconds(1);
        middleText.text = "2";
        yield return new WaitForSeconds(1);
        middleText.text = "1";
        yield return new WaitForSeconds(1);
        middleText.text = "GO!";
        controlsEnabled = true;
        yield return new WaitForSeconds(3);
        middleText.text = "";


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

        if(lapCounter == lapNumbers)
        {
            int position = checkPoint.GetComponent<CheckPointBehavior>().nextPos;
            if(position == 1)
            {
                middleText.text = "WINNER!";
            }
            if(position == 2)
            {
                middleText.text = position + "nd";
            }
            if (position == 3)
            {
                middleText.text = position + "rd";
            }
            if (position == 4)
            {
                middleText.text = position + "th";

            }
            checkPoint.GetComponent<CheckPointBehavior>().SendMessage("AddNextPos", 1);
            controlsEnabled = false;
        }
    }

    private bool IsCheckPoint(Collider otherObject)
    {
        return otherObject.tag.Contains("CheckPoint");
    }
}

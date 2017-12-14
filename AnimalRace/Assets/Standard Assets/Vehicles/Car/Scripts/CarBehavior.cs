using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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
    [SerializeField]
    private Transform missileLauncher;
    private bool controlsEnabled;
    private int currentPosition;
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
    [SerializeField]
    Text positionText;
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
        currentPosition = 1;
    }

    internal void ChangeMaxSpeed(float multiplier)
    {
        CarController stolenScript = gameObject.GetComponent<CarController>();
        float newMaxSpeed = stolenScript.MaxSpeed * multiplier;
        if (newMaxSpeed <= 150)
        {
            stolenScript.MaxSpeed = newMaxSpeed;
        }
        else
            stolenScript.Torque = stolenScript.Torque * multiplier;
    }

    public void SetCurrentPosition(int newPos)
    {
        currentPosition = newPos;
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
            timerText.text = timeText ;
        }
        nextCheckPointDistance = CalculateDistanceToNextCheckPoint();
        GameObject raceManager = GameObject.FindGameObjectsWithTag("RaceManager")[0];
        int numberOfPlayers = raceManager.GetComponent<RaceManager>().GetCarNumbers();
        positionText.text ="POS: " + currentPosition.ToString() + "/"+ numberOfPlayers;
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
                case "8":
                    if (!activeCheckpoints[0] && activeCheckpoints[1])
                    {
                        activeCheckpoints[2] = true;
                    }
                    nextCheckPoint = 9;
                    break;
                case "12":
                    nextCheckPoint = 1;
                    break;
                default:
                    nextCheckPoint = Int32.Parse( checkPointNumber) + 1;
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

    internal string GetPowerUpName(int powerUpPosition)
    {
        return myPowerUp.ToString();
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
            UpdateTrackTimes();
        }
    }

    private void UpdateTrackTimes()
    {
        GameObject raceManager = GameObject.FindGameObjectsWithTag("RaceManager")[0];

        string trackName = raceManager.GetComponent<RaceManager>().getTrackName();
        XmlDocument xml = new XmlDocument();
        string currentPath = Directory.GetCurrentDirectory();
        xml.Load(currentPath + "/AnimalRace_Data/times.xml"); 

        XmlNode xnLastTime = xml.SelectNodes("/tracks/track[@name='" + trackName + "']/time[@index='4']/count").Item(0);
        float lastTime=  float.Parse( xnLastTime.InnerText);
        if(lapTime < lastTime)
        {
            var nextPos = 0;
            XmlNode[] auxNodes = new XmlNode[5];
            XmlNodeList xnTimeNodes = xml.SelectNodes("/tracks/track[@name='" + trackName + "']/time");
            foreach (XmlNode xn in xnTimeNodes)
            {
                if (lapTime < float.Parse(xn.FirstChild.InnerText))
                {
                    XmlNode newTimeNode = xml.CreateElement("time");
                    XmlNode newCountNode = xml.CreateElement("count");
                    XmlNode newNameNode = xml.CreateElement("name");
                    XmlAttribute indexAttribute = xml.CreateAttribute("index");
                    indexAttribute.Value = xn.Attributes["index"].Value;
                    newCountNode.InnerText = lapTime.ToString();
                    newTimeNode.Attributes.Append(indexAttribute);
                    newNameNode.InnerText = "John Doe-" + xn.Attributes["index"].Value;
                    newTimeNode.AppendChild(newCountNode);
                    newTimeNode.AppendChild(newNameNode);
                    auxNodes[int.Parse(xn.Attributes["index"].Value)] = newTimeNode;
                    nextPos = int.Parse(xn.Attributes["index"].Value);
                    break;
                }
                else
                {
                    auxNodes[int.Parse(xn.Attributes["index"].Value)] = xn;
                }
            }

            foreach (XmlNode xn in xnTimeNodes)
            {
                if (nextPos <= int.Parse(xn.Attributes["index"].Value) && int.Parse(xn.Attributes["index"].Value) < 4)
                {
                    XmlNode newTimeNode = xml.CreateElement("time");
                    XmlNode newCountNode = xml.CreateElement("count");
                    XmlNode newNameNode = xml.CreateElement("name");
                    XmlAttribute indexAttribute = xml.CreateAttribute("index");
                    indexAttribute.Value = (int.Parse(xn.Attributes["index"].Value) + 1).ToString();
                    newCountNode.InnerText = xn.FirstChild.InnerText;
                    newTimeNode.Attributes.Append(indexAttribute);
                    newNameNode.InnerText = xn.LastChild.InnerText;
                    newTimeNode.AppendChild(newCountNode);
                    newTimeNode.AppendChild(newNameNode);
                    auxNodes[int.Parse(xn.Attributes["index"].Value) + 1] = newTimeNode;
                }
            }

            foreach (XmlNode xn in xnTimeNodes)
            {
                XmlNode auxNode = auxNodes[int.Parse(xn.Attributes["index"].Value)];
                xn.FirstChild.InnerText = auxNode.FirstChild.InnerText;
                xn.LastChild.InnerText = auxNode.LastChild.InnerText;
            }

            xml.Save(currentPath + "/AnimalRace_Data/times.xml");
        }
       
    }

    private bool IsCheckPoint(Collider otherObject)
    {
        return otherObject.tag.Contains("CheckPoint");
    }

    public int GetCurrentLap()
    {
        return lapCounter;
    }

    public int GetNextCheckPoint()
    {
        return nextCheckPoint;
    }

    public float GetNextCheckPointDistance()
    {
        return nextCheckPointDistance;
    }
}

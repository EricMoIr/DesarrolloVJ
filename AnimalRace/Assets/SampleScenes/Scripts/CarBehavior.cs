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
    public bool HasShield = false;
    public bool HasTurbo = false;
    public Transform MineLauncher;
    [SerializeField]
    private PowerUpsHolderObject powerUps;
    [SerializeField]
    private string playerName;
    private int lapCounter;
    private int nextCheckPoint;
    private float nextCheckPointDistance;
    private bool[] activeCheckpoints;
    private List<AbnormalStatus> abnormalStatuses = new List<AbnormalStatus>();
    private DateTime powerUpGottenTime;
    private TimeSpan POWER_UP_WAITING_TIME = new TimeSpan(0, 0, 0);
    private SpecialPower myPowerUp;
    [SerializeField]
    public Transform missileLauncher;
    static public bool controlsEnabled;
    private bool controlsEnabledEnd = true;
    private int currentPosition;
    [SerializeField]
    private string FIRE_AXIS;
    [SerializeField]
    int lapNumbers;
    Text middleText;
    Text timerText;
    Text lapText;
    Text positionText;
    [SerializeField]
    int nroplayer;
    float lapTime;
    static public int finished = 0;
    [SerializeField]
    private string PAUSE_AXIS;
    [SerializeField]
    private GameObject pauseMenu;
    private GameObject lastPowerImage;

    public PowerUpsHolderObject PowerUps { get { return powerUps; } }

    // Use this for initialization
    void Start()
    {
        if (nroplayer == 1)
        {
            middleText = GameObject.FindGameObjectWithTag("OneText").GetComponent<Text>();
            timerText = GameObject.FindGameObjectWithTag("OneTimerText").GetComponent<Text>();
            lapText = GameObject.FindGameObjectWithTag("OneLapText").GetComponent<Text>();
            positionText = GameObject.FindGameObjectWithTag("OnePositionText").GetComponent<Text>();
            playerName = RaceData.RaceDataHolder.isNew.PlayerName1;
            middleText.text = "3";
        }
        else if (nroplayer == 2)
        {
            timerText = GameObject.FindGameObjectWithTag("TwoTimerText").GetComponent<Text>();
            lapText = GameObject.FindGameObjectWithTag("TwoLapText").GetComponent<Text>();
            positionText = GameObject.FindGameObjectWithTag("TwoPositionText").GetComponent<Text>();
            playerName = RaceData.RaceDataHolder.isNew.PlayerName2;
        }
        else if (nroplayer == 3)
        {
            timerText = GameObject.FindGameObjectWithTag("ThreeTimerText").GetComponent<Text>();
            lapText = GameObject.FindGameObjectWithTag("ThreeLapText").GetComponent<Text>();
            positionText = GameObject.FindGameObjectWithTag("ThreePositionText").GetComponent<Text>();
            playerName = RaceData.RaceDataHolder.isNew.PlayerName3;
        }
        else if (nroplayer == 4)
        {
            timerText = GameObject.FindGameObjectWithTag("FourTimerText").GetComponent<Text>();
            lapText = GameObject.FindGameObjectWithTag("FourLapText").GetComponent<Text>();
            positionText = GameObject.FindGameObjectWithTag("FourPositionText").GetComponent<Text>();
            playerName = RaceData.RaceDataHolder.isNew.PlayerName4;
        }
        lapCounter = 0;
        activeCheckpoints = new bool[] { false, false, false };
        timerText.text = "";
        lapText.text = "Lap: 1/" + lapNumbers;
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
            stolenScript.Torque = 5000;
        }
        else
        {
            stolenScript.MaxSpeed = 150;
            stolenScript.Torque = stolenScript.Torque * multiplier;
        }
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
        Pause();
        if (controlsEnabled && controlsEnabledEnd)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        if (lapCounter < lapNumbers && controlsEnabled && controlsEnabledEnd)
        {
            lapTime += Time.deltaTime;
            int milliseconds = (int)(lapTime * 1000) % 1000;
            int seconds = (int)(lapTime % 60);
            int minutes = (int)(lapTime / 60) % 60;

            string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            timerText.text = timeText;
        }

        nextCheckPointDistance = CalculateDistanceToNextCheckPoint();
        GameObject raceManager = GameObject.FindGameObjectsWithTag("RaceManager")[0];
        int numberOfPlayers = RaceData.RaceDataHolder.isNew.NumberOfPlayers;
        positionText.text = currentPosition.ToString();
        if (currentPosition == 1)
            positionText.text += "st";
        else if (currentPosition == 2)
            positionText.text += "nd";
        else if (currentPosition == 3)
            positionText.text += "rd";
        else if (currentPosition == 4)
            positionText.text += "th";
    }

    private DateTime pressedTime;
    private void Pause()
    {
        if (Input.GetButtonDown(PAUSE_AXIS))
        {
            if (DateTime.Now < pressedTime + new TimeSpan(0, 0, 1) && pressedTime != DateTime.MinValue)
                return;
            pressedTime = DateTime.Now;
            if (IsPaused())
            {
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
            }
            else
            {
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
            }
        }
    }

    private bool IsPaused()
    {
        return false;//return pauseMenu.activeInHierarchy;
    }

    void OnTriggerEnter(Collider otherObject)
    {
        PowerUpBoxTriggerEnter(otherObject);
        CheckpointTriggerEnter(otherObject);
    }

    float CalculateDistanceToNextCheckPoint()
    {
        GameObject nextCPoint = GameObject.FindGameObjectsWithTag("CheckPoint_" + nextCheckPoint)[0];
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
                    nextCheckPoint = Int32.Parse(checkPointNumber) + 1;
                    break;
            }
        }
    }

    #region PowerUp

    private void FireSpecialPower()
    {
        if (Input.GetAxisRaw(FIRE_AXIS) != 0)
        {
            if (myPowerUp != null && DateTime.Now > powerUpGottenTime + POWER_UP_WAITING_TIME)
            {
                myPowerUp.Activate(this);
                myPowerUp = null;
                lastPowerImage.SetActive(false);
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
                powerUpGottenTime = DateTime.Now;
                GameObject[] canvases = new GameObject[2];
                string[] fu = new string[] { "TwoPlayerCanvas", "FourPlayerCanvas" };
                canvases[0] = GameObject.Find("TwoPlayerCanvas");
                canvases[1] = GameObject.Find("FourPlayerCanvas");
                for (int i = 0; i < canvases.Length; i++)
                {
                    if (!canvases[i]) continue;
                    string a = fu[i] + "/UI/PowerUp" + nroplayer + "/" + myPowerUp.GetName();
                    lastPowerImage = GameObject.Find(fu[i] + "/UI/PowerUp" + nroplayer + "/" + myPowerUp.GetName());
                    lastPowerImage.SetActive(true);
                }
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
            middleText.text = "Lap: " + (lapCounter + 1) + "/" + lapNumbers;
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
        lapText.text = "Lap: " + (lapCounter + 1) + "/" + lapNumbers;

    }

    IEnumerator BeginCountDown()
    {
        if (nroplayer == 1)
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
    }

    private void ResetCheckPointList()
    {
        for (int i = 0; i < activeCheckpoints.Length; i++)
        {
            activeCheckpoints[i] = false;
        }
    }

    private void CheckIfRaceIsOver(Collider checkPoint)
    {

        string correctText = "";

        if (nroplayer == 1)
        {
            correctText = "OneLapText";
        }
        else if (nroplayer == 2)
        {
            correctText = "TwoLapText";
        }
        else if (nroplayer == 3)
        {
            correctText = "ThreeLapText";
        }
        else if (nroplayer == 4)
        {
            correctText = "FourLapText";
        }


        if (lapCounter == lapNumbers)
        {
            if (RaceData.RaceDataHolder.isNew.NumberOfPlayers != 1)
            {
                int position = checkPoint.GetComponent<CheckPointBehavior>().nextPos;
                if (position == 1)
                {
                    GameObject.FindGameObjectWithTag(correctText).GetComponent<Text>().text = position + "st";
                }
                if (position == 2)
                {
                    GameObject.FindGameObjectWithTag(correctText).GetComponent<Text>().text = position + "nd";
                }
                if (position == 3)
                {
                    GameObject.FindGameObjectWithTag(correctText).GetComponent<Text>().text = position + "rd";
                }
                if (position == 4)
                {
                    GameObject.FindGameObjectWithTag(correctText).GetComponent<Text>().text = position + "th";

                }
                checkPoint.GetComponent<CheckPointBehavior>().SendMessage("AddNextPos", 1);
                controlsEnabledEnd = false;
                finished++;
                print("finished ahora es: " + finished);
            }
            else
            {
                middleText.text = "1st";
                controlsEnabledEnd = false;
                finished++;
                print("finished ahora es: " + finished);

            }

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

        XmlNode xnLastTime = xml.SelectNodes("/tracks/track[@name='" + trackName + "']/time[@index='5']/count").Item(0);
        float lastTime = float.Parse(xnLastTime.InnerText);
        if (lapTime < lastTime)
        {
            var nextPos = 0;
            XmlNode[] auxNodes = new XmlNode[6];
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
                    newNameNode.InnerText = playerName;
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
                if (nextPos <= int.Parse(xn.Attributes["index"].Value) && int.Parse(xn.Attributes["index"].Value) < 5)
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

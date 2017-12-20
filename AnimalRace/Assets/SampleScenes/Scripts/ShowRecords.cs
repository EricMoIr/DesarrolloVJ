using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RaceData;
using System.Xml;
using System.IO;
using System;

public class ShowRecords : MonoBehaviour
{
    [SerializeField]
    public Text trackNameText;
    [SerializeField]
    public Text timesText;
    // Use this for initialization
    void Start()
    {
        trackNameText.text = "Camino Rocoso";

        string trackName = "Track1";
        string times = "";
        XmlDocument xml = new XmlDocument();
        string currentPath = Directory.GetCurrentDirectory();
        xml.Load(currentPath + "/AnimalRace_Data/times.xml");
        XmlNodeList xnTimeNodes = xml.SelectNodes("/tracks/track[@name='" + trackName + "']/time");
        foreach (XmlNode xn in xnTimeNodes)
        {

            string playerName = xn.LastChild.InnerText;
            string line;
            float playerTime = float.Parse(xn.FirstChild.InnerText);
            string timeText;
            if (playerTime >= 99999)
            {
                playerName = "N/A";
                timeText = "00:00:000";
            }
            else
            {
                int milliseconds = (int)(playerTime * 1000) % 1000;
                int seconds = (int)(playerTime % 60);
                int minutes = (int)(playerTime / 60) % 60;

                timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            }



            string dots = new String('.', 21 - playerName.Length);
            line = playerName + dots + timeText;
            times += line + "\n";

        }

        timesText.text = times;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTimes(string trackName)
    {
        if(trackName == "Track1")
        {
            trackNameText.text = "Camino Rocoso";
        }
        else
        {
            trackNameText.text = "Parque de Agua";
        }

        string times = "";
        XmlDocument xml = new XmlDocument();
        string currentPath = Directory.GetCurrentDirectory();
        xml.Load(currentPath + "/AnimalRace_Data/times.xml");
        XmlNodeList xnTimeNodes = xml.SelectNodes("/tracks/track[@name='" + trackName + "']/time");
        foreach (XmlNode xn in xnTimeNodes)
        {

            string playerName = xn.LastChild.InnerText;
            string line;
            float playerTime = float.Parse(xn.FirstChild.InnerText);
            string timeText;
            if (playerTime >= 99999)
            {
                playerName = "N/A";
                timeText = "00:00:000";
            }
            else
            {
                int milliseconds = (int)(playerTime * 1000) % 1000;
                int seconds = (int)(playerTime % 60);
                int minutes = (int)(playerTime / 60) % 60;

                timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            }

         

            string dots = new String('.', 21 - playerName.Length);
            line = playerName + dots + timeText;
            times += line + "\n";

        }

        timesText.text = times;
    }


}
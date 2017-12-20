using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaceData
{
    public class RaceDataHolder : MonoBehaviour
    {
        public static RaceDataHolder isNew;
        [SerializeField]
        private int numberOfPlayers;
        [SerializeField]
        private string selectedTrack;
        [SerializeField]
        private string playerName1;
        private string playerName2;
        private string playerName3;
        private string playerName4;

        public int NumberOfPlayers
        {
            get
            {
                return numberOfPlayers;
            }
            set
            {
                numberOfPlayers = value;
            }
        }

        public string SelectedTrack
        {
            get
            {
                return selectedTrack;
            }
            set
            {
                selectedTrack = value;
            }
        }

        public string PlayerName1
        {
            get
            {
                return playerName1;
            }
            set
            {
                playerName1 = value;
            }
        }

        public string PlayerName2
        {
            get
            {
                return playerName2;
            }
            set
            {
                playerName2 = value;
            }
        }

        public string PlayerName3
        {
            get
            {
                return playerName3;
            }
            set
            {
                playerName3 = value;
            }
        }

        public string PlayerName4
        {
            get
            {
                return playerName4;
            }
            set
            {
                playerName4 = value;
            }
        }

        void Awake()
        {
            if (isNew == null)
            {
                isNew = this;
                DontDestroyOnLoad(gameObject);

            }
            else if (isNew != this)
                Destroy(gameObject);
        }
    }
}

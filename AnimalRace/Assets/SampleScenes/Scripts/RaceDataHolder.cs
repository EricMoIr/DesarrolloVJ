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
        private string playerName;

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

        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                playerName = value;
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

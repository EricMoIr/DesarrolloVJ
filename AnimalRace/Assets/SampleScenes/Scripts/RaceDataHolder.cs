using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RaceData
{
    public static class RaceDataHolder
    {
        private static int numberOfPlayers;
        private static string selectedTrack;

        public static int NumberOfPlayers
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

        public static string SelectedTrack
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

    }
}

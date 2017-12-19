using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] cars;
    [SerializeField]
    private string trackName;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MergeSort(0, cars.Length-1);
        for(int i = 0; i < cars.Length; i++)
        {
            GameObject car = cars[i];
            car.GetComponent<CarBehavior>().SendMessage("SetCurrentPosition", i + 1);
        }
	}

    public int GetCarNumbers()
    {
        return cars.Length;
    }

    public string getTrackName()
    {
        return trackName;
    }

    public  void MergeSort(int low, int high)
    {
        if (low < high)
        {
            int middle = (low / 2) + (high / 2);
            MergeSort(low, middle);
            MergeSort( middle + 1, high);
            Merge(low, middle, high);
        }
    }

    public  void MergeSort()
    {
        MergeSort(0, cars.Length - 1);
    }

    private  void Merge( int low, int middle, int high)
    {

        int left = low;
        int right = middle + 1;
        GameObject[] tmp = new GameObject[(high - low) + 1];
        int tmpIndex = 0;

        while ((left <= middle) && (right <= high))
        {
            if (IsLessThan(cars[left], cars[right]))
                {
                tmp[tmpIndex] = cars[left];
                left = left + 1;
            }
            else
            {
                tmp[tmpIndex] = cars[right];
                right = right + 1;
            }
            tmpIndex = tmpIndex + 1;
        }

        if (left <= middle)
        {
            while (left <= middle)
            {
                tmp[tmpIndex] = cars[left];
                left = left + 1;
                tmpIndex = tmpIndex + 1;
            }
        }

        if (right <= high)
        {
            while (right <= high)
            {
                tmp[tmpIndex] = cars[right];
                right = right + 1;
                tmpIndex = tmpIndex + 1;
            }
        }

        for (int i = 0; i < tmp.Length; i++)
        {
            cars[low + i] = tmp[i];
        }

    }


    bool IsLessThan(GameObject car1, GameObject car2)
    {
        int car1Lap =  car1.GetComponent<CarBehavior>().GetCurrentLap();
        int car2Lap =  car2.GetComponent<CarBehavior>().GetCurrentLap();

        int car1CheckPoint = car1.GetComponent<CarBehavior>().GetNextCheckPoint();
        int car2CheckPoint = car2.GetComponent<CarBehavior>().GetNextCheckPoint();

        float car1CheckPointDistance = car1.GetComponent<CarBehavior>().GetNextCheckPointDistance();
        float car2CheckPointDistance = car2.GetComponent<CarBehavior>().GetNextCheckPointDistance();

        if(car2Lap < car1Lap)
        {
            return true;
        }
        else
        {
            if(car2CheckPoint < car1CheckPoint)
            {
                return true;
            }
            else
            {
                return car2CheckPointDistance > car1CheckPointDistance;
            }
        }

    }


}

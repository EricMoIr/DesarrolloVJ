using Assets.Scripts.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehavior : MonoBehaviour
{
    [SerializeField]
    private float speed;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collisioned)
    {
        if (collisioned.gameObject.tag == "Player")
        {
            CarBehavior car = collisioned.gameObject.GetComponent<CarBehavior>();
            if (car.HasShield) return;
            MissileCollisionStatus status = new MissileCollisionStatus();
            status.Activate(car);
            car.AddAbnormalStatus(status);
        }
        gameObject.SetActive(false);
    }
}
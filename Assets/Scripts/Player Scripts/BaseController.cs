using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseController : MonoBehaviour
{
    [Header("Movement Parameters")]
    public Vector3 speed;
    public float xSpeed = 7.5f, zSpeed = 15f, accelerated = 25f, decelerated = 5f;
    public float maxAltitude = 3.5f, normalAltitude = 2f, minAltitude = 0.5f, upLift = 1f, downLift = 1f;
    public float speedIncrementPeriod = 30f;
    public bool isFlyingVehicle;

    [Header("On-screen buttons")]
    public GameObject shootButton;
    public GameObject upButton;
    public GameObject downButton;
    public GameObject rightButton;
    public GameObject leftButton;

    [Header("Monitoring through editor")]
    public float timePassed;

    protected float rotationSpeed = 10f, maxAngle = 10f;

    private AudioSource soundManager;
    private bool isSlow;
    private bool isFast;

    private void Awake()
    {
        soundManager = GetComponent<AudioSource>();
        speed = new Vector3(0f, 0f, zSpeed);
        timePassed = 0f;
    }

    protected void MoveNormal()
    {
        if (isFlyingVehicle)
        {
            speed = new Vector3(speed.x, 0f, zSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            speed = new Vector3(speed.x, 0f, zSpeed);
        }
    }

    //forward movement
    protected void MoveStraight()
    {
        speed = new Vector3(0f, 0f, speed.z);
    }

    //sideways movement towards left
    protected void MoveLeft()
    {
        speed = new Vector3(-xSpeed / 2f, 0f, speed.z);
    }
    //sideways movement towards right
    protected void MoveRight()
    {
        speed = new Vector3(xSpeed / 2f, 0f, speed.z);
    }
    
    //for ground vehicles
    //Increase speed
    protected void MoveFast()
    {
        speed = new Vector3(speed.x, 0f, accelerated);
    }
    //Decrease speed
    protected void MoveSlow()
    {
        speed = new Vector3(speed.x, 0f, decelerated);
    }

    //for air vehicles
    //Increase altitude
    public void MoveUp()
    {
        speed = new Vector3(speed.x, upLift, speed.z);
    }
    //Decrease altitude
    public void MoveDown()
    {
        speed = new Vector3(speed.x, -downLift, speed.z);
    }
}

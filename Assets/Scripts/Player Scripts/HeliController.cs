using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliController : BaseController
{
    public Transform bullet_StartPoint;
    public GameObject bullet_Prefab;
    public ParticleSystem shootFX;

    public float bulletSpeed = 2000f;

    private Rigidbody myBody;
    private Animator shootSliderAnim;

    [HideInInspector]
    public bool canShoot;

    void Start()
    {
        myBody = GetComponent<Rigidbody>();

        //shootSliderAnim = GameObject.Find("Firepower Bar").GetComponent<Animator>();
        shootSliderAnim = GameObject.Find("UI Holder").GetComponentInChildren<Animator>(true);

        shootButton = GameObject.Find("Shoot Button");
        upButton = GameObject.Find("Up Button");
        downButton = GameObject.Find("Down Button");
        rightButton = GameObject.Find("Right Button");
        leftButton = GameObject.Find("Left Button");

        //Shoot Button
        shootButton.GetComponent<HoldToClick>().onLongClickUp.AddListener(ShootingControl);

        //On screen Up button
        if (isFlyingVehicle)
        {
            upButton.gameObject.GetComponent<HoldToClick>().onLongClickDown.AddListener(MoveUp);
        }
        else
        {
            upButton.gameObject.GetComponent<HoldToClick>().onLongClickDown.AddListener(MoveFast);
        }
        upButton.gameObject.GetComponent<HoldToClick>().onLongClickUp.AddListener(MoveNormal);

        //On screen Down button
        if (isFlyingVehicle)
        {
            downButton.gameObject.GetComponent<HoldToClick>().onLongClickDown.AddListener(MoveDown);
        }
        else
        {
            downButton.gameObject.GetComponent<HoldToClick>().onLongClickDown.AddListener(MoveSlow);
        }
        downButton.gameObject.GetComponent<HoldToClick>().onLongClickUp.AddListener(MoveNormal);

        //On screen Right button
        rightButton.gameObject.GetComponent<HoldToClick>().onLongClickDown.AddListener(MoveRight);
        rightButton.gameObject.GetComponent<HoldToClick>().onLongClickUp.AddListener(MoveStraight);

        //On screen Left button
        leftButton.gameObject.GetComponent<HoldToClick>().onLongClickDown.AddListener(MoveLeft);
        leftButton.gameObject.GetComponent<HoldToClick>().onLongClickUp.AddListener(MoveStraight);

        canShoot = true;

        StartCoroutine(IncreaseSpeed());
    }

    void Update()
    {
        ControlMovementWithKeyboard();

        ShootThroughKeyboard();

        timePassed += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        MoveHeli();
        ChangeRotation();
    }

    void MoveHeli()
    {
        myBody.MovePosition(myBody.position + speed * Time.deltaTime);
    }

    void ControlMovementWithKeyboard()
    {
        //Rotating left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            MoveLeft();
        }
        //rotating right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            MoveRight();
        }
        //increasing forward movement speed or altitude
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (isFlyingVehicle)
            {
                MoveUp();
            }
            else
            {
                MoveFast();
            }
        }
        //reducing forward movement speed or altitude
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (isFlyingVehicle)
            {
                MoveDown();
            }
            else
            {
                MoveSlow();
            }
        }

        //the player moves straight when we let go(key comes up) of the left movement key
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            MoveStraight();
        }
        //the player moves straight when we let go(key comes up) of the right movement key
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            MoveStraight();
        }
        //resetting back to normal speed when we let go of Speed Up key
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            MoveNormal();
        }
        //resetting back to normal speed when we let go of Speed Down key
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            MoveNormal();
        }
    }

    void ShootThroughKeyboard()
    {
        //Shooting through space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootingControl();
        }
    }

    void ChangeRotation()
    {
        if (speed.x > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, maxAngle, 0f), Time.deltaTime * rotationSpeed);
        }
        else if (speed.x < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, -maxAngle, 0f), Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * rotationSpeed);
        }
    }

    public void ShootingControl()
    {
        if (Time.timeScale != 0)
        {
            if (canShoot)
            {
                GameObject bullet = Instantiate(bullet_Prefab, bullet_StartPoint.position, Quaternion.identity);
                bullet.GetComponent<BulletScript>().Move(bulletSpeed);
                shootFX.Play();

                canShoot = false;
                shootSliderAnim.Play("Fill");
            }
        }
    }

    IEnumerator IncreaseSpeed()
    {
        yield return new WaitForSeconds(speedIncrementPeriod);

        zSpeed += 5;
        accelerated += 5;
        decelerated += 5;
        MoveNormal();

        StartCoroutine(IncreaseSpeed());

    }
}

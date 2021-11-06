using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    InputSystem inputActions;               //new input system

    public Transform barrel;                //location from where the bullet is fired 
    public PlayerBullet[] playerBullets;    //pool of bullets which are used when shooting
    public int currentBullet;               //what bullet in the pool is being used 

    [HideInInspector]
    public IEnumerator shotTimer;           //corountine for the firing of bullets
    protected IEnumerator shotCoolOff;      //corountine for a cooldown between shots 
    public float shotDelay;                 //time between each shot

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();                                        // gets sprite renderer
        //Input system
        inputActions = new InputSystem();                                                       //creates an Input system to get inputs from 
        inputActions.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();  //movement input
        inputActions.Player.Shoot.started += _ => StartShooting();                              //Starts shooting
        inputActions.Player.Shoot.canceled += _ => StopShooting();                              //stops shooting and activates cooldown
    }
    void Update()
    {
        MovementUpdate();
    }
    //Called Every update and moves the player in based on InputActions
    void MovementUpdate()
    {
        Vector2 _currentPosition;                                                   //for where the player is 
        Vector2 _newDirection;                                                      //the vector of where the player is tring to go

        _newDirection =  moveDirection * movementSpeed * Time.deltaTime;            //is the current move input x speed x time(time is for smoother movement) 

        _currentPosition = PixelPerfectClamp(transform.position, 16);               //this rounds the current Vector2 to the nearest virtual pixel 
        _newDirection = SubPixelMovment(_newDirection, 16);                         //this sets the Vector2 to the neaserst virtual pixel + any leftovers

        Rect tempBounds = GameManager.staticGameManager.PlayerMovementBounds;       //gets the bounds of the map from the GameMananger

        Vector2 newPosition = _currentPosition + _newDirection;                     //

        if (newPosition.x > tempBounds.xMin && 
            newPosition.y > tempBounds.yMin && 
            newPosition.x < tempBounds.xMax && 
            newPosition.y < tempBounds.yMax)
        {
            transform.position = _currentPosition + _newDirection;
        }

    }    

    //when shoot input is down shoot bullet and start shotTimer
    public virtual void StartShooting()
    {
        if (shotCoolOff == null)
        {
            ShootBullet();
            shotTimer = ShotDelay();
            StartCoroutine(shotTimer);
        }
    }
    //when shoot input is up stop shotTimer and start shotCoolOff
    protected virtual void StopShooting()
    {
        if (shotTimer != null)
        {
            StopCoroutine(shotTimer);
            shotTimer = null;
            shotCoolOff = CoolOffDelay();
            StartCoroutine(shotCoolOff);
        }
    }
    //corountine for held rapid firing
    protected virtual IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        ShootBullet();
        yield return 0;
        shotTimer = ShotDelay();
        StartCoroutine(shotTimer);
    }
    //corountine that delays the time between new shoot inputs. this means the player can't spam the shoot input to fire faster
    protected virtual IEnumerator CoolOffDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        yield return 0;
        shotCoolOff = null;
    }
    //this activaes the current bullet in the pool 
    void ShootBullet()
    {
        if (playerBullets[currentBullet])
        {
            playerBullets[currentBullet].transform.position = barrel.position;
            playerBullets[currentBullet].ActivateBullet(new Vector2(1, 0), damage);

            if (currentBullet < playerBullets.Length - 1)
            {
                currentBullet++;
            }
            else
            {
                currentBullet = 0;
            }
        }
        //soundManager.PlaySound(SoundType.Shoot);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    InputSystem inputActions;
    public Transform barrel;
    public float shotDelay;
    public PlayerBullet[] playerBullets;
    //public int availableBullets;
    public int currentBullet;
    public int damage;

    [HideInInspector]
    public IEnumerator shotTimer;
    protected IEnumerator shotCoolOff;

    // Start is called before the first frame update
    void Awake()
    {
        inputActions = new InputSystem();
        inputActions.Player.Move.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
        inputActions.Player.Shoot.started += _ => StartShooting();
        inputActions.Player.Shoot.canceled += _ => StopShooting();
    }

    public virtual void StartShooting()
    {
        if (shotCoolOff == null)
        {
            ShootBullet();
            shotTimer = ShotDelay();
            StartCoroutine(shotTimer);
        }
    }
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

    protected virtual IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        ShootBullet();
        yield return 0;
        shotTimer = ShotDelay();
        StartCoroutine(shotTimer);
    }
    protected virtual IEnumerator CoolOffDelay()
    {
        yield return new WaitForSeconds(shotDelay);
        yield return 0;
        shotCoolOff = null;
    }

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


    // Update is called once per frame
    void FixedUpdate()
    {
        MovementUpdate();
    }

    void MovementUpdate()
    {
        Vector2 _currentPosition;
        Vector2 _newDirection;

        _newDirection =  moveDirection * movementSpeed * Time.deltaTime;

        _currentPosition = PixelPerfectClamp(transform.position, 16);
        _newDirection = SubPixelMovment(_newDirection, 16);

        Rect tempBounds = GameManager.staticGameManager.PlayerMovementBounds;

        Vector2 newPosition = _currentPosition + _newDirection;

        if (newPosition.x > tempBounds.xMin && newPosition.y > tempBounds.yMin && newPosition.x < tempBounds.xMax && newPosition.y < tempBounds.yMax)
        {
            transform.position = _currentPosition + _newDirection;
        }

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

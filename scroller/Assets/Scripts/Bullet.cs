using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    public Entity parentObject;
    public bool isShot;
    public int damage;
    // Start is called before the first frame update
    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void ActivateBullet(Vector2 _dir, int _setDamage)
    {
        transform.parent = null;
        moveDirection = _dir;
        spriteRenderer.enabled = true;
        isShot = true;
        damage = _setDamage;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (isShot)
        {
            MovementUpdate();
        }
    }

    protected virtual void MovementUpdate()
    {
        Vector2 _currentPosition;
        Vector2 _newDirection;

        _newDirection = (moveDirection * movementSpeed) * Time.deltaTime;

        _currentPosition = PixelPerfectClamp(transform.position, 32);
        _newDirection = SubPixelMovment(_newDirection, 32);

        Rect tempBounds = GameManager.staticGameManager.PlayerMovementBounds;
        Vector2 newPosition = _currentPosition + _newDirection;
        if (newPosition.x > tempBounds.xMin && newPosition.y > tempBounds.yMin && newPosition.x < tempBounds.xMax && newPosition.y < tempBounds.yMax)
        {
            if (!CheckHit())
            {
                transform.position = newPosition;
            }
            else
            {
                BulletEnd();
            }
        }
        else
        {
            BulletEnd();
        }

    }
    public virtual void BulletEnd()
    {
        transform.parent = parentObject.transform;
        transform.localPosition = Vector2.zero;
        spriteRenderer.enabled = false;
        isShot = false;
    }
}

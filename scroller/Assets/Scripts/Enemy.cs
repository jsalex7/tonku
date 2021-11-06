using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    // Start is called before the first frame update
    protected void Awake()
    {
        moveDirection = new Vector2(-1,0);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        MovementUpdate();
    }

    protected virtual void MovementUpdate()
    {
        Vector2 _currentPosition;
        Vector2 _newDirection;

        _newDirection = (moveDirection * movementSpeed) * Time.deltaTime;

        _currentPosition = PixelPerfectClamp(transform.position, 16);
        _newDirection = SubPixelMovment(_newDirection, 16);

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
                Death();
            }
        }
        else
        {
            Death();
        }

    }
    public virtual void Death()
    {
        Destroy(gameObject);
    }
}

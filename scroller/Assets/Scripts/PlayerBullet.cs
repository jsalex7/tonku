using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override bool CheckHit()
    {
        LayerMask mask = (1 << 0);

        Hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), .5f, moveDirection, Time.deltaTime * movementSpeed, mask);

        foreach (RaycastHit2D hit in Hits)
        {
            if (hit.transform != this.transform)
            {
                if (hit.transform.GetComponent<Enemy>())
                {
                    hit.transform.GetComponent<Enemy>().TakeDamage(damage);

                    return true;
                }
            }
            else
                return false;
        }
        return false;

    }
}

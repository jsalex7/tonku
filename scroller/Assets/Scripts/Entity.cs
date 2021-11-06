using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region PixelPerfectAjustment
    Vector2 storedSubPixelVector;                        //stored vector used for subpixel movement
    #endregion
    protected Vector2 moveDirection;
    public float movementSpeed;
    public SpriteRenderer spriteRenderer;
    public int healthCurrent;
    public int healthTotal;

    //public EntityType entityType;
    protected RaycastHit2D[] Hits = new RaycastHit2D[2];      //this checks the area infornt of the player when they are moving

    protected virtual bool CheckHit() { return false; }
    protected Vector2 PixelPerfectClamp(Vector2 mMoveVector, float mPixelsPerUnit)
    {
        //Vector2 x ppu as intergers then divided by ppu
        Vector2 vectorInPixels = new Vector2(
            Mathf.RoundToInt(mMoveVector.x * mPixelsPerUnit),
            Mathf.RoundToInt(mMoveVector.y * mPixelsPerUnit));

        return vectorInPixels / mPixelsPerUnit;        //returns the clamped Vector2
    }
    //this allows the movement vector to use floats without rounding to interger
    protected Vector2 SubPixelMovment(Vector2 mMoveVector, float mPixelsPerUnit)
    {
        //Vector2 x ppu as intergers
        Vector2 vectorInPixels = new Vector2(
            Mathf.RoundToInt(mMoveVector.x * mPixelsPerUnit),
            Mathf.RoundToInt(mMoveVector.y * mPixelsPerUnit));

        float excessPosX = (mMoveVector.x * mPixelsPerUnit) - Mathf.RoundToInt(mMoveVector.x * mPixelsPerUnit);     //take remainder off of rounded vector       //i.e. if player moves 1.5 then this will result in 0.5
        float excessPosY = (mMoveVector.y * mPixelsPerUnit) - Mathf.RoundToInt(mMoveVector.y * mPixelsPerUnit);     //take remainder off of rounded vector       //i.e. if player moves 1.5 then this will result in 0.5


        Vector2 excessVector = new Vector2(Mathf.RoundToInt(storedSubPixelVector.x + excessPosX), Mathf.RoundToInt(storedSubPixelVector.y + excessPosY));       //Combinded excess from last check and current check

        if (mMoveVector.x == 0)
            storedSubPixelVector.x = 0;                                                                 //when the input vector is 0 then reset stored vector
        else
            storedSubPixelVector.x = (storedSubPixelVector.x + excessPosX) - excessVector.x;            //removes the rounded excess from the stored vector

        if (mMoveVector.y == 0)
            storedSubPixelVector.y = 0;                                                                 //when the input vector is 0 then reset stored vector
        else
            storedSubPixelVector.y = (storedSubPixelVector.y + excessPosY) - excessVector.y;            //removes the rounded excess from the stored vector

        vectorInPixels += excessVector;             //Adds stored vector to clamped vector

        return vectorInPixels / mPixelsPerUnit;     //returns the clamped Vector2

    }
    public virtual void TakeDamage(int _sentDamage)
    {
        healthCurrent -= _sentDamage;

        if (healthCurrent <= 0)
        {
            healthCurrent = 0;
            Death();
        }

    }
    public virtual void Death()
    {
        Destroy(gameObject);
    }
}

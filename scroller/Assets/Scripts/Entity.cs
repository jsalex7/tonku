using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Null
}

public class Entity : MonoBehaviour
{
    #region Components
    public EntityType entityType;           
    public SpriteRenderer spriteRenderer;   //SpriteRenderer for this entity
    #endregion
    #region PixelPerfectAjustment
    Vector2 storedSubPixelVector;                        //stored vector used for subpixel movement
    #endregion
    #region Movement
    protected Vector2 moveDirection;        //where the entity is going 
    public float movementSpeed;             //how fast the entity moves
    #endregion
    #region Health
    public int healthTotal;                 //max health of the entity
    protected int healthCurrent;            //health that revices damage and if <= 0 the entity will die 
    #endregion
    public int damage;                      //damage entity puts out 

    protected RaycastHit2D[] Hits = new RaycastHit2D[2];      //stored hits from CheckHit

    protected virtual bool CheckHit() { return false; }//this checks the area infornt of the entity when it is moving
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
    //Called when a Entity needs to recive damage, virtual so Entity's with other onDamage effects
    public virtual void TakeDamage(int _sentDamage)
    {
        healthCurrent -= _sentDamage;   //removes Damage sent from the script calling this method from the current health of the entity

        //checks if the enitiy's health has fallen to or below 0
        if (healthCurrent <= 0)
        {
            healthCurrent = 0;          //avoids any issue that come from having an entity w/ negitive health vaules. i.e. UI changes
            Death();                    //Calls the death method to kill the entity
        }
    }
    //Called when a Entity needs to die, virtual so Entity's with other onDeath effects
    public virtual void Death()
    {
        Destroy(gameObject);// temp destroy before we make batter pooling solution
    }
}

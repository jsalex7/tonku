using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager staticGameManager;
    public Rect PlayerMovementBounds;

    // Start is called before the first frame update
    private void Awake()
    {
        if (staticGameManager == null)
        {
            staticGameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (staticGameManager != this)
        {
            Destroy(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

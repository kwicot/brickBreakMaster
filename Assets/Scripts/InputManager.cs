using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    Vector2 startPosition;
    Vector2 movedPosition;
    Vector2 endedPosition;

    Vector2 Direct;

    public Text start;
    public Text move;
    public Text end;

    void Start()
    {
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                movedPosition = touch.position;
            }
            if(touch.phase == TouchPhase.Ended)
            {
                endedPosition = touch.position;
            }
        }


        start.text = startPosition.ToString();
        move.text = movedPosition.ToString();
        end.text = endedPosition.ToString();

        Debug.Log(startPosition + " ; " + movedPosition + " ; " + endedPosition);

    }

    void CalcDirect()
    {
        Direct = startPosition - movedPosition;
    }

    void Shoot()
    {
        CalcDirect();

    }
}

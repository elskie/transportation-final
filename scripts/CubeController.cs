using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int myX, myY;
    GameController myGameController;

    // Start is called before the first frame update
    void Start()
    {
       myGameController = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        myGameController.ClickProcess(gameObject, myX, myY);

    }
}

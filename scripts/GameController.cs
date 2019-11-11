using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cubePrefab;
    Vector3 cubePosition; 
    public static GameObject ActiveCube;
    // Start is called before the first frame update
    int AirplaneXPos, AirplaneYPos;
    GameObject[,] cubeGrid; //2d array, a collection of game objects
    int gridX, gridY;
    bool airplaneActive;
    float TurnDuration,NextTurn;
    public static int cargo;
    int CargoMax;
    int StartXPos, StartYPos;
    int depotX, depotY;
    int moveX, moveY;
    int TargetX, TargetY;


    void Start()
    {
        TurnDuration = 1.5f;//how many seconds pass before the new turn
        NextTurn = TurnDuration; //a variable to act as a way to continuously mine at the miningSpeed
        gridX = 16; //make variables if you are repeating numbers that make it a pain to go in a individually retype
        gridY = 9;
        cubeGrid = new GameObject[gridX, gridY]; //keep track of each cube you made by putting them all in an array
        CargoMax = 90;
        cargo = 0;
        

        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                cubePosition = new Vector3(x * 2, y * 2, 0);
                cubeGrid[x, y] = Instantiate(cubePrefab, cubePosition, Quaternion.identity);//make cube, tell grid to store it in array
                cubeGrid[x, y].GetComponent<CubeController>().myX =  x;//the cubegrid x,y specifies where the cube is in the array, referencing the one that already exists
                cubeGrid[x, y].GetComponent<CubeController>().myY = y;//we set this is in the cube controller so each cube now has a myX myY that we assign to grid x and grid y here
            }
        }

        StartXPos = 0;
        StartYPos = gridY - 1;
        depotX = gridX -1; 
        depotY = 0;
        AirplaneXPos = StartXPos;//this needs to be above the color render in order to make the cube red in the beginning before it was ever clicked
        AirplaneYPos = StartYPos;
        cubeGrid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
        cubeGrid[AirplaneXPos, AirplaneYPos].GetComponent<Renderer>().material.color = Color.red;
        airplaneActive = false;
        moveX = 0;
        moveY = 0;
        TargetX = AirplaneXPos;
        TargetY = AirplaneYPos;
    }

    public void ClickProcess(GameObject clickedCube, int x, int y)
    {
        if (x == AirplaneXPos && y == AirplaneYPos) //did i click on the plane
        {
            if (airplaneActive) //was it active when i clicked already
            {
                //deactivate the active plane, put this first to make code clearer to read without no
                airplaneActive = false;
                clickedCube.transform.localScale /= (1.5f);
                TargetX = AirplaneXPos;
                TargetY = AirplaneYPos;
            }
            else 
            {
                airplaneActive = true;
                clickedCube.transform.localScale *= (1.5f);
            }
        } 
        else if (airplaneActive)//did i click anywhere but the plane
        {
            TargetX = x;
            TargetY = y;
        }
    }
    void DetermineDiretion()
    {
        if (AirplaneYPos > TargetY)
        {
            moveY = -1;
        }
        else if (AirplaneYPos < TargetY)
        {
            moveY = 1;
        }
        else
        {
            moveY = 0;
        }

        if (AirplaneXPos < TargetX)
        {
            moveX = 1;
        }
        else if (AirplaneXPos > TargetX)
        {
            moveX = -1;
        }
        else
        {
            moveX = 0;
        }
    }

    void MoveAirplane()
    {
        DetermineDiretion();

        if (airplaneActive)
        {
            if (AirplaneXPos == depotX && AirplaneYPos == depotY)
            {
                cubeGrid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                cubeGrid[AirplaneXPos, AirplaneYPos].GetComponent<Renderer>().material.color = Color.white;
            }
            cubeGrid[AirplaneXPos, AirplaneYPos].transform.localScale /= (1.5f);

            //move the airplane too new spot
            AirplaneXPos += moveX;
            AirplaneYPos += moveY;

            //check for out of bounds
            if (AirplaneXPos >= gridX)
            {
                AirplaneXPos = gridX - 1;   
            }
            else if (AirplaneXPos < 0)
            {
                AirplaneXPos = 0;
            }
            
            if (AirplaneYPos >= gridY)
            {
                AirplaneYPos = gridY - 1;
            }
            else if (AirplaneYPos < 0)
            {
                AirplaneYPos = 0;
            }
           
            cubeGrid[AirplaneXPos,AirplaneYPos].GetComponent<Renderer>().material.color = Color.red;
            cubeGrid[AirplaneXPos, AirplaneYPos].transform.localScale *= (1.5f);
        }
        moveX = 0;
        moveY = 0;
    }
  
    // Update is called once per frame
    void Update()
    {
        if (Time.time > NextTurn)
        {
            NextTurn += TurnDuration;
            MoveAirplane();

            // is the plane in the upper left? give cargo
            if (AirplaneXPos == StartXPos && AirplaneYPos == StartYPos && cargo < CargoMax)
            {
                cargo += 10;
                print(cargo + " tons loaded");
            }
            // is the plane is the lower right? give points
            if (AirplaneXPos == depotX && AirplaneYPos == depotY)
            {
                ScoreScript.ScoreValue += (cargo / 10);
                cargo = 0;
            }
            print("Carrying " + cargo + "   Score: " + ScoreScript.ScoreValue);

        }

    }
}

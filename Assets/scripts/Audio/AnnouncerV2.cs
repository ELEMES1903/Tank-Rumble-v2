using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerV2 : MonoBehaviour
{

    // Slots for CapturePoint game objects
        public RadiusController PointA;
        public RadiusController PointB;
        public RadiusController PointC;

        // Slots for red and blue tank
        public HealthSystem redTank;
        public HealthSystem blueTank;
        // cap point score tracker
        public int redScore;
        public int blueScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Array of capture points
        RadiusController[] capturePoints = { PointA, PointB, PointC };

        // Loop through each capture point
        foreach (var point in capturePoints)
        {
            if (point.redPoint && point.captured)
            {
                redScore++;
                blueScore--;
            }
            else if (point.bluePoint && point.captured)
            {
                blueScore++;
                redScore--;
            }

            if(blueScore > 3){blueScore = 3;}
            if(redScore > 3){redScore = 3;}
            if(blueScore < 0){blueScore = 0;}
            if(redScore < 0){redScore = 0;}
        }



        Check();

    }

    void Check()
    {
        if(blueScore == 2)
        {
            
        }
    }












}

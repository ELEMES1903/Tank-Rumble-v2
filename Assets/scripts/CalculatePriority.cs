using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePriority : MonoBehaviour
{

    public TankAI tankAI;

    public float movePriority;
    float firePriority;
    public float findHealthPriority;
    public float capturePriority;
    public float roamPriority;

    public float progress;

    void Update(){
        
        if(gameObject.CompareTag("Tank1"))
        {
            progress = tankAI.checkTeam1Progress;
        } else 
        {
            progress = tankAI.checkTeam2Progress;
        }
    }

    public float CalculateMovePriority()
    {
        if(tankAI.enemy != null){
            
            movePriority = 60f + (100 - tankAI.enemyHealth);
        } else {
            movePriority = 0f;
        }
    
        return movePriority;
    }


    public float CalculateFindHealthPriority()
    {
        findHealthPriority = 100 - tankAI.healthSystem.currentHealth;
        return findHealthPriority;
    }
 
    public float CalculateCapturePriority()
    {
        if(tankAI.capturepoint != null && progress != 10)
        {
            capturePriority = 50f;

        } else 
        {
            capturePriority = 0f;
        }
            
        return capturePriority;
    }

    public float CalculateRoamPriority()
    {
        
        if(tankAI.enemy == null && tankAI.capturepoint == null)
        {
            roamPriority = 200f;

        } else if(tankAI.enemy == null && tankAI.capturepoint != null)
        {
            if(progress == 10)
            {
                roamPriority = 200f;
            } else{
                roamPriority = 0f;
            }

        } else
        {
            roamPriority = 0f;
        }

        return roamPriority;
    }
}

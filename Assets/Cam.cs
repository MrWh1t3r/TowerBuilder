using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private int targetID;
    public Transform target;
    private bool gameover;
    public float movingSpeed = 20;
    public int y = 1;
    public int z = -2;
    Quaternion rot0 = Quaternion.Euler(0, 0, 0);
    Quaternion rot1 = Quaternion.Euler(15, 0, 0);


    void Update()
    {
        
        gameover = Game.getGameover();
        targetID = Game.getCurrentPoolElementID();
        if (gameover)
        {
            targetID++;           
            
            Vector3 newTarget = new Vector3(target.GetChild(targetID / 2).position.x, target.GetChild(targetID / 2).position.y, target.GetChild(targetID).position.z - (targetID)); //            
            this.transform.position = Vector3.Lerp(this.transform.position, newTarget, this.movingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, rot0, this.movingSpeed * Time.deltaTime);
        }
        else
        {            
            Vector3 newTarget = new Vector3(target.GetChild(targetID).position.x, target.GetChild(targetID).position.y + y, target.GetChild(targetID).position.z + z);
            this.transform.position = Vector3.Lerp(this.transform.position, newTarget, this.movingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(this.transform.rotation, rot1, this.movingSpeed * Time.deltaTime);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rotation : MonoBehaviour
{
    private Vector3 mPos;
    
    private Quaternion mRot;
    
   
    private void Chech_Mouse_Pos() 
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; 

       
        Vector3 direction = mousePos - transform.position;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

     
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    private void Update()
    {
        Chech_Mouse_Pos();
       
    }


}

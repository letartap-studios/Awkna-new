﻿using UnityEngine;

public class HorizontalPatrollingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    private bool movingRight = true;
    public Transform groundDetection;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.85f);
        Debug.DrawRay(groundDetection.position, Vector2.down);



        RaycastHit2D wallInfoR = Physics2D.Raycast(groundDetection.position, Vector2.right, 0.05f);
        RaycastHit2D wallInfoL = Physics2D.Raycast(groundDetection.position, Vector2.left, 0.05f);


        if (groundInfo.collider == false)
        {

            if (movingRight == true)
            {
                //Debug.Log("O sa pic");
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }





        if (movingRight == true && wallInfoR.collider == true
            && wallInfoR.collider.gameObject.tag != "PatrollingEnemy"
            /*&& wallInfoR.collider.IsTouchingLayers(LayerMask.GetMask("Platform"))*/)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }


        }
        if (movingRight == false && wallInfoL.collider == true
            && wallInfoL.collider.gameObject.tag != "PatrollingEnemy"
           /* && wallInfoR.collider.IsTouchingLayers(LayerMask.GetMask("Platform"))*/)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }

        }
    }
}

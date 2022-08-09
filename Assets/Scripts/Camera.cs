using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    int CubeNumber = 0;
    GameObject highestCube;
    public float Number = 0.2f;
    float StartPosition = 0;
    private float moveSpeed = 0.5f;
    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        StartPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        CubeNumber = GameObject.FindGameObjectsWithTag("platte").Length;
        if (CubeNumber > 5) {
            highestCube = GameObject.FindGameObjectsWithTag("platte")[^1];

            float highestCubeY = highestCube.transform.position.y + 2.5f;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, highestCubeY, transform.position.z), ref velocity, moveSpeed);

        }



        /*
         * if (transform.position.y < StartPosition + (CubeNumber-5) * Number && CubeNumber>4) {

            transform.position += new Vector3(0,1,0) * Time.deltaTime * moveSpeed;
        }*/



    }
}

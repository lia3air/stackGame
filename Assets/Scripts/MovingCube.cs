using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    bool hasPhysics = true;

    [SerializeField]

    private float moveSpeed = 1f;

    private void OnEnable()
    {
        if (GameObject.FindGameObjectsWithTag("platte").Length % 15 == 0)
        {
            hasPhysics = false;
            GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
        }
        else {
            GetComponent<Renderer>().material.color = GetRandomColor();
        }
        if (LastCube == null)
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();

        CurrentCube = this;
        

        transform.localScale = new Vector3(LastCube.transform.localScale.x,transform.localScale.y, LastCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        Color[] colors = new Color[4];
        colors[0] = new Color(221f / 255f, 129f / 255f, 186f / 255f);
        colors[1] = new Color(163f / 255f, 96f/255f, 255f / 255f);
        colors[2] = new Color(255f / 255f, 96f / 255f, 169f / 255f);
        colors[3] = new Color(255f / 255f, 96f / 255f, 96f / 255f);
        return colors[UnityEngine.Random.Range(0, 4)];
    }

    internal void Stop()
    {
        moveSpeed = 0;
        float hangover = GetHangover();
        float max = MoveDirection == MoveDirection.Z ? LastCube.transform.localScale.z : LastCube.transform.localScale.x;

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        if (rigidbody) {
            rigidbody.isKinematic = false;
        }
        if (rigidbody && hasPhysics == false) {
            StartCoroutine(DeactivatePhysics(rigidbody));
        }
        

        if (Mathf.Abs(hangover) >= max )
        {
            LastCube = null;
            CurrentCube = null;
            //SceneManager.LoadScene(0);

        }

       /*float direction = hangover > 0 ? 1f : -1f;
        if (MoveDirection == MoveDirection.Z)
            SplitCubeOnZ(hangover, direction);
        else
            SplitCubeOnX(hangover, direction);*/

        LastCube = this;
    }

    private IEnumerator DeactivatePhysics(Rigidbody r) {
        yield return new WaitForSeconds(1f);
        r.isKinematic = true;
    }

    private float GetHangover()
    {
        if (MoveDirection == MoveDirection.Z)
            return transform.position.z - LastCube.transform.position.z;
        else
            return transform.position.x - LastCube.transform.position.x;
    }

    private void SplitCubeOnX(float hangover, float direction)
    {
        float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.x - newXSize;

        float newXPosition = LastCube.transform.position.x + (hangover / 2);
        transform.localScale = new Vector3(newXSize,transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition,transform.position.y, transform.position.z );

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);
        }
        else {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.z );
        }

        

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

        Destroy(cube.gameObject, 1f);

    }

    private void Update()

    {
        if (MoveDirection == MoveDirection.Z)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }

        if (transform.position.y < -5) {
            SceneManager.LoadScene(0);
        }

    }
}

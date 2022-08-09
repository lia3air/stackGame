using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int CubeNumber = 0;
    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner currentSpawner;
    public TextMeshProUGUI text;


    private void Awake()
    {
        spawners = FindObjectsOfType<CubeSpawner>();
    }
    private void Update()
    {

        CubeNumber = GameObject.FindGameObjectsWithTag("platte").Length;
        text.text = CubeNumber.ToString();

        if (Input.GetKeyDown("space"))
        {
            if (MovingCube.CurrentCube != null)
                MovingCube.CurrentCube.Stop();
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            currentSpawner = spawners[spawnerIndex];


            currentSpawner.SpawnCube();
        }

    }
}

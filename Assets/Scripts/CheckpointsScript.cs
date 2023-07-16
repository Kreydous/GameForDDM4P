using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static int numOfChekpoints = 0;
    public GameObject[] checkpoints = new GameObject[numOfChekpoints];
    int currentChekpoint = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(currentChekpoint < checkpoints.Length)
        {
          
            if (checkpoints[currentChekpoint].activeSelf == false)
            {
                currentChekpoint++;
                checkpoints[currentChekpoint].SetActive(true);

            }
        }
        if(GameObject.FindGameObjectWithTag("Start")==null)
        {
            currentChekpoint = 0;
            checkpoints[currentChekpoint].SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
    AudioSource m_Source;
    // Start is called before the first frame update
    void Start()
    {
       m_Source = GetComponent<AudioSource>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Start") != null){
            m_Source.volume = 1;
        }
        else
        {
            m_Source.volume = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}

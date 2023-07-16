using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float tiltThreshold=10f;
    public float yTiltThreshold = 0.25f;
    bool rotate = true;
    bool move = false;
    bool collided = false;
    Rigidbody2D rgb;
    public float speed = 1;
    Vector2 direction;
    bool isStarted = false;
    private AudioSource audioSource;
    public AudioClip leftSound,rightSound,upSound,downSound;
    public AudioClip startGameSound,playSound, instuctionsSound,GameWonClip;
    private float y_start = 0;
    bool isCalibrated = false;
    public GameObject start;
    private Transform startPos;
    private void Start()
    {
        startPos = transform;
        start.gameObject.SetActive(false);
        Invoke("Calibrate", 3);
        rgb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Invoke("ReadStartInstructions", 1f);
    }
    private void Update()
    {
        if (isCalibrated && isStarted)
        {
            float tiltAngle = Input.acceleration.x * Mathf.Rad2Deg;
            if (Mathf.Abs(tiltAngle) > tiltThreshold && rotate)
            {
                Vector2 objectUp = transform.up;
                Debug.Log("Rotate");
                rotate = false;
                int targetRot = tiltAngle > 0 ? -90 : 90;
                transform.rotation = Quaternion.Euler(0, 0, targetRot);
                PlaySound();

            }
            float yTilt = Input.acceleration.y - y_start;
            Debug.Log(yTilt);
            if (Mathf.Abs(yTilt) > yTiltThreshold && rotate)
            {
                rotate = false;
                int targetRot = yTilt > 0 ? 0 : 180;
                transform.rotation = Quaternion.Euler(0, 0, targetRot);
                PlaySound();
            }
            if (Mathf.Abs(tiltAngle) < 10 && rotate == false && Mathf.Abs(yTilt) < 0.5)
            {
                rotate = true;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                move = true;
            }
            else
            {
                move = false;
            }

            if (speed > 0 && collided)
            {
                Debug.Log("vibrate");
                Handheld.Vibrate();
            }
        }
        else
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.position.x < Screen.width / 2)
                {
                    PlayClip(instuctionsSound);
                }else if(touch.position.x > Screen.width /2)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        PlayClip(playSound);
                        isStarted = true;
                        start.gameObject.SetActive(true);
                        transform.position = new Vector3(5, -3.14f, 0);
                    }
                    
                }
            }
        }
        


    }

    void PlaySound()
    {
        if(transform.rotation.z == 0)
        {
            PlayClip(upSound);
        }else if(transform.rotation.z == 90)
        {
            PlayClip(leftSound);
        }else if(transform.rotation.z == -90)
        {
            PlayClip(rightSound);
        }
        else if(transform.rotation.z ==180)
        {
            PlayClip(downSound);
        }
    }

    private void FixedUpdate()
    {
        if (move)
        {
            direction = transform.up;
            speed = 2;
        }
        else
        {
            speed = 0;
        }
        rgb.velocity = direction * speed;

    }

    void ReadStartInstructions()
    {
        audioSource.clip = startGameSound;
        audioSource.Play();
    }

    void PlayClip(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            collided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            collided = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            //WIN
            start.gameObject.SetActive(false);
            isStarted = false;
            transform.position = startPos.position;
            PlayClip(GameWonClip);
        }
    }
    private void Calibrate()
    {
        y_start = Input.acceleration.y;
        isCalibrated = true;
    }
}

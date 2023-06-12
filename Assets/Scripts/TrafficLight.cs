using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;
    public List<TrafficLight> oppositeTrafficLights;

    public float greenLightDuration = 5f;
    public float yellowLightDuration = 2f;
    public float redLightDuration = 5f;

    private bool isCarInCollider = false;
    private float timer = 0f;
    private int state = 2; // 0 = green, 1 = yellow, 2 = red (default)

    private void Start()
    {
        redLight.SetActive(true);
        yellowLight.SetActive(false);
        greenLight.SetActive(false);
    }

    private void Update()
    {
        if (isCarInCollider)
        {
            timer += Time.deltaTime;
            bool isOppositeTrafficLightRed = true;
            foreach (TrafficLight oppositeTrafficLight in oppositeTrafficLights)
            {
                if (oppositeTrafficLight.state != 2)
                {
                    isOppositeTrafficLightRed = false;
                    break;
                }
            }

            if (isOppositeTrafficLightRed)
            {
                if (state == 2)
                {
                    timer = 0f;
                    redLight.SetActive(false);
                    yellowLight.SetActive(false);
                    greenLight.SetActive(true);
                    state = 0;
                }
                else if (state == 1 && timer >= yellowLightDuration)
                {
                    timer = 0f;
                    yellowLight.SetActive(false);
                    redLight.SetActive(true);
                    state = 2;
                }
            }
            else
            {
                timer = 0f;
                greenLight.SetActive(false);
                redLight.SetActive(true);
                state = 2;
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (state == 0 && timer >= greenLightDuration) // If the traffic light is green and the green light duration has elapsed
            {
                timer = 0f;
                greenLight.SetActive(false);
                yellowLight.SetActive(true);
                state = 1;
            }
            else if (state == 1 && timer >= yellowLightDuration) // If the traffic light is yellow and the yellow light duration has elapsed
            {
                timer = 0f;
                yellowLight.SetActive(false);
                redLight.SetActive(true);
                state = 2;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            isCarInCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            isCarInCollider = false;
        }
    }
}
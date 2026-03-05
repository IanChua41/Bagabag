using UnityEngine;

public class LightBehavior : MonoBehaviour
{
    [SerializeField] private GameObject lightObject;
    [SerializeField] private GameObject SpotLight;
    [SerializeField] private float lightWaitTime = 5f;
    [SerializeField] private float lightInitialTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnCollisionEnter(Collision collision)
    {
        lightInitialTime += Time.deltaTime;

        if (collision.gameObject.tag == "Player")
        {

            if(lightInitialTime >= lightWaitTime)
            {
                lightObject.SetActive(true);
                lightInitialTime = 0f;
            }
            Debug.Log("Player has entered the trigger");
        }
    }

}

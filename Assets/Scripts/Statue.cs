using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Statue : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject statue;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] bool isFixed = false;

    private void Awake()
    {
        statue.SetActive(isFixed);
    }

    public void Interact()
    {
        if (!isFixed)
        {
            statue.SetActive(true);
            // baseLight.SetActive(false);

            Debug.Log("Statue fixed.");
            isFixed = true;

            GetComponent<Statue>().enabled = false;
        }
        else
        {
            StartCoroutine(DisplayMessage());
            Debug.Log("Follow the statue.");
        }
    }

    IEnumerator DisplayMessage()
    {
        message.text = "Follow the statue.";
        yield return new WaitForSeconds(3);
        message.text = "";
    }
}

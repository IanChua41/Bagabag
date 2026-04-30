using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Items:
    // - Gun
    // - Flashlight
    // - 
    
    
    public bool hasGun = false;
    public bool hasFlashlight = false;

    [SerializeField] GameObject gunImg;
    [SerializeField] GameObject flashlightImg;


    private void Start()
    {
        gunImg.SetActive(false);
        flashlightImg.SetActive(false);
    }

    private void Update()
    {
        gunImg?.SetActive(hasGun);
        flashlightImg?.SetActive(hasFlashlight);
    }


    //public void AcquireFlashlight()
    //{
    //    hasFlashlight = true;
    //    flashlightImg.SetActive(true);
    //}

    //public void DropFlashlight()
    //{
    //    hasFlashlight = false;
    //    flashlightImg.SetActive(false);
    //}


    //public void AcquireGun()
    //{
    //    hasGun = true;
    //    gunImg.SetActive(true);
    //}

    //public void DropGun()
    //{
    //    hasGun = false;
    //    gunImg.SetActive(false);
    //}
}

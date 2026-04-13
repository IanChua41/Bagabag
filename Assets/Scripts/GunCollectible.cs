using UnityEngine;

public class Gun : MonoBehaviour
{
    //[SerializeField] float rotationSpeed = 100f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private int value;
    private bool hasTriggered;
    private Rigidbody gunRb;
    private Collider gunCollider;

    private GunCollectibleManager gunCollectibleManager;
    private PlayerInventory playerInventory;

    private void Start()
    {
        gunCollectibleManager = GunCollectibleManager.instance;
        gunRb = GetComponent<Rigidbody>();
        gunCollider = GetComponent<Collider>();

        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void Update()
    {
        //transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            gunCollectibleManager.CollectGun(value);
            //Destroy(gameObject);
            CollectGun();
        }   
    }

    void CollectGun()
    {
        //rotationSpeed = 0;
        // 1. Disable physics so it doesn't fly away or freak out
        if (gunRb != null) gunRb.isKinematic = true;
        gunCollider.enabled = false;

        // 2. Set the Gun's parent to the Hold Point
        transform.SetParent(holdPoint);

        // 3. Reset position and rotation to match the Hold Point exactly
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // 4. Update player inventory
        playerInventory.hasGun = true;
    }
}

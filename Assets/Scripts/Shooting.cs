using NUnit.Framework;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Animator anim;
    bool isShooting;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float damage;
    public Camera fpsCamera;


    // Update is called once per frame
    void Update()
    {
        anim = gameObject.GetComponent<Animator>();
        if (Input.GetButtonDown("Fire1"))
        {
            isShooting = true;
            if (isShooting == true)
            {
                anim.SetBool("IsShooting", true);
                Shoot();
                Debug.Log("Shoot!");
            }

        }
        else
        {

            isShooting = false;
            if (isShooting == false)
            {
                anim.SetBool("IsShooting", false);
            }
        }

    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
        }

    }
}

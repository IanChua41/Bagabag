using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Animator anim;
    bool isShooting;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float range;
    public Camera fpsCamera;
    [SerializeField] private AudioClip shootSound;
    private float shootVolume = 1f;



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
        if (shootSound != null)
        {
            Vector3 audioPosition = bulletSpawnPoint != null ? bulletSpawnPoint.position : transform.position;
            AudioSource.PlayClipAtPoint(shootSound, audioPosition, shootVolume);
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if (hit.collider.CompareTag("Monster"))
            {
                MonsterBehavior monster = hit.collider.GetComponentInParent<MonsterBehavior>();
                if (monster != null)
                {
                    monster.TakeDamage();
                    return;
                }

                MiniMonsterBehavior miniMonster = hit.collider.GetComponentInParent<MiniMonsterBehavior>();
                if (miniMonster != null)
                {
                    miniMonster.TakeDamage();
                    return;
                }

                // Fallback: if tagged but no known behavior script, destroy as before.
                Destroy(hit.collider.gameObject);
            }
        }

    }
}

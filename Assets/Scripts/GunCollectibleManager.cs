using UnityEngine;
using TMPro;

public class GunCollectibleManager : MonoBehaviour
{
    public static GunCollectibleManager instance;
    public int gun;
    [SerializeField] private TMP_Text gunDisplay;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void OnGUI()
    {
        gunDisplay.text = gun.ToString();
    }

    public void CollectGun(int amount)
    {
        gun += amount;
    }
}

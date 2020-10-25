using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunAmmoUI : MonoBehaviour
{
    public Transform currentAmmoUI;
    public Transform carriedAmmoField;
    public static GunAmmoUI instance;

    TextMeshProUGUI currentAmmoText;
    TextMeshProUGUI carriedAmmoFieldText;

    private void Awake()
    {
        instance = this;
        currentAmmoText = currentAmmoUI.GetComponent<TextMeshProUGUI>();
        carriedAmmoFieldText = carriedAmmoField.GetComponent<TextMeshProUGUI>();
    }

    public void UpdateAmmoUI(int currentAmmo, int carriedAmmo)
    {
        if(int.Parse(currentAmmoText.text) != currentAmmo)
        {
            currentAmmoText.text = currentAmmo.ToString();
        }
         if(int.Parse(carriedAmmoFieldText.text) != carriedAmmo)
        {
            carriedAmmoFieldText.text = carriedAmmo.ToString();
        }

    }
}

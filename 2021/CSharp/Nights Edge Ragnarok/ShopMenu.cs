using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public static bool openShop = false;

    public GameObject shopMenuUI;

    public void CloseShop()
    {
        shopMenuUI.SetActive(false);
        openShop = false;
    }

    public void OpenShop()
    {
        shopMenuUI.SetActive(true);
        openShop = true;
    }
}

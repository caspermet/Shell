using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    private bool isPause;
    public static PauseMenu Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        if (isPause)
        {
            MenuManager.Instance.CloseMenu("Pause");
        }
        else
        {
            MenuManager.Instance.OpenMenu("Pause");
        }
        isPause = !isPause;
        Cursor.visible = isPause;
    }

    public void Option()
    {
        MenuManager.Instance.OpenMenu("Option menu");
    }

    public void back()
    {
        MenuManager.Instance.OpenMenu("Pause");
    }

    public bool IsPause()
    {
        return isPause;
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

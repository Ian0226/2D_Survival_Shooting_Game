using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPanelController : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    public void SetPanelState(bool state)
    {
        settingPanel.SetActive(state);
        //0 is StartScene.
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            //not finish yet.
        }
    }
}

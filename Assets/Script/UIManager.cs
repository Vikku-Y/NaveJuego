using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image hpBar;
    public Image counterBar;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        hpBar.type = Image.Type.Filled;
        counterBar.type = Image.Type.Filled;

        hpBar.fillMethod = Image.FillMethod.Horizontal;
        counterBar.fillMethod = Image.FillMethod.Horizontal;
    }
}

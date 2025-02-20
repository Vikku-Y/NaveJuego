using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image hpBar;
    public Image energyBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        hpBar.type = Image.Type.Filled;
        energyBar.type = Image.Type.Filled;

        hpBar.fillMethod = Image.FillMethod.Horizontal;
        energyBar.fillMethod = Image.FillMethod.Horizontal;
    }
}

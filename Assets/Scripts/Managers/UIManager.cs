using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text tbPointTotal;
    public static UIManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Update the total points visual
    /// </summary>
    public void UpdateTotalPoints()
    {
        //Update the text box with the value from game manager
        tbPointTotal.text = Convert.ToString(GameManager.Instance.PointTotal);
    }
}

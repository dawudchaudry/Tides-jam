using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField]
    public float baseFloodTime;
    [SerializeField]
    public float baseDroughtTime;
    public bool isFlooding;
    public float timeUntilFlood;
    public float timeUntilDrought;
    public float floodLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFlooding = false;
        timeUntilFlood = baseFloodTime;
    }

    // Update is called once per frame
    void Update()
    {
        //If the ground is not flooded
        if (!isFlooding)
        {
            var quarterDone = baseFloodTime / 4;
            var halfTime = baseFloodTime / 2;
            var quarterLeft = quarterDone * 3;
            timeUntilFlood -= Time.deltaTime;
            if (timeUntilFlood <= 0)
            {
                isFlooding = true;
                timeUntilDrought = baseDroughtTime;
            }else if (timeUntilFlood > quarterDone && floodLevel != 0.75f)
            {
                floodLevel = 0.75f;
            } else if (timeUntilFlood > halfTime && floodLevel != 0.5f)
            {
                floodLevel = 0.5f;    
            } else if (timeUntilFlood > quarterLeft && floodLevel != 0.25f)
            {
                floodLevel = 0.25f;
            } else
            {
                floodLevel = 0;
            }
        } else
        {
            var quarterLeft = baseDroughtTime / 4;
            var halfTime = baseDroughtTime / 2;
            var quarterDone = quarterLeft * 3;
            timeUntilDrought -= Time.deltaTime;
            if (timeUntilDrought <= 0)
            {
                isFlooding = false;
                timeUntilFlood = baseFloodTime;
            }  else if (timeUntilDrought > quarterDone && floodLevel != 0.25f)
            {
                floodLevel = 0.25f;
            } else if (timeUntilDrought > halfTime && floodLevel != 0.5f)
            {
                floodLevel = 0.5f;    
            } else if (timeUntilDrought > quarterLeft && floodLevel != 0.75f)
            {
                floodLevel = 0.75f;
            } else
            {
                floodLevel = 1;
            }
        }
    }
}

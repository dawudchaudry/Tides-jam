using NUnit.Framework;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField]
    public float baseFloodTime;
    [SerializeField]
    public float baseDroughtTime;
    public bool isGroundFlooded;
    public float timeUntilFlood;
    public float timeUntilDrought;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGroundFlooded = false;
        timeUntilFlood = baseFloodTime;
    }

    // Update is called once per frame
    void Update()
    {
        //If the ground is not flooded
        if (!isGroundFlooded)
        {
            timeUntilFlood -= Time.deltaTime;
            if (timeUntilFlood <= 0)
            {
                isGroundFlooded = true;
                timeUntilDrought = baseDroughtTime;
            }
        } else
        {
            timeUntilDrought -= Time.deltaTime;
            if (timeUntilDrought <= 0)
            {
                isGroundFlooded = false;
                timeUntilFlood = baseFloodTime;
            }
        }
    }
}

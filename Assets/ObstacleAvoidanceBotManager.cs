using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBotManager : MonoBehaviour
{
    private ObstacleAvoidanceBot[] bots;

    private void Start()
    {
        bots = FindObjectsOfType<ObstacleAvoidanceBot>();

    }

    public void SetCanMove(bool value)
    {
        foreach(ObstacleAvoidanceBot bot in bots)
        {
            bot.SetCanMove(value);
        }
    }
}

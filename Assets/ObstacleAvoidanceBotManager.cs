using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBotManager : MonoBehaviour
{
    // The colllection of bots
    public Transform spheres;

    private Dictionary<string, ObstacleAvoidanceBot> bots;
    private Dictionary<string, string> botLinks;

    private void Start()
    {
        bots = new Dictionary<string, ObstacleAvoidanceBot>();
        botLinks = new Dictionary<string, string>();

        addBotsToDictionary();
        addLinkWithOtherBot();
    }

    private void addBotsToDictionary()
    {
        foreach (Transform value in spheres)
        {
            ObstacleAvoidanceBot bot = value.GetComponent<ObstacleAvoidanceBot>();
            bots.Add(bot.name, bot);
        }
    }

    private void addLinkWithOtherBot()
    {
        foreach (KeyValuePair<string, ObstacleAvoidanceBot> bot in bots)
        {
            // The manager saved the sphere connection and remove the old one in case it exists
            // A timer is used to make the next connection possible, so it got time to escape
            bot.Value.TriggerSphereLink += (GameObject otherSphere) => 
            {
                if (botLinks.ContainsKey(otherSphere.name))
                {
                    RemoveBotLink(otherSphere.name);
                    WaitTimeForNextLink(otherSphere.name);

                }

                botLinks.Add(otherSphere.name, bot.Value.name);
            };
        }
    }

    public void SetCanMove(bool value)
    {
        foreach(KeyValuePair<string, ObstacleAvoidanceBot> bot in bots)
        {
            bot.Value.SetCanMove(value);
        }
    }

    public void ResetToStartPosistion()
    {
        foreach (KeyValuePair<string, ObstacleAvoidanceBot> bot in bots)
        {
            bot.Value.ResetSphere();
        }
    }

    private void RemoveBotLink(string botName)
    {
        bots[botName].RemoveOtherSphere();
        botLinks.Remove(botName);
    }

    private void WaitTimeForNextLink(string botName)
    {
        if(bots[botName].getCanLink())
            bots[botName].ActivateWaitTimeForNextLink();
    }
}

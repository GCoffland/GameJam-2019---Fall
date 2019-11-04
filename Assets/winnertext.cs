using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winnertext : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string winningColor = "";
        switch(ConditionChecker.winner)
        {
            case (Player.TEAM.ONE):
                winningColor = "Yellow";
                break;
            case (Player.TEAM.TWO):
                winningColor = "Red";
                break;
            case (Player.TEAM.THREE):
                winningColor = "Purple";
                break;
            case (Player.TEAM.FOUR):
                winningColor = "Blue";
                break;
            case (Player.TEAM.FIVE):
                winningColor = "Green";
                break;
        }
        GetComponent<Text>().text = winningColor + " team wins!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

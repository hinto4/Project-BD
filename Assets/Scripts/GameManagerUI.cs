using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : GameManager
{
    public Text MatchCountDownTxt;
    public Text WaveTxt;

    void Update()
    {
        // If match has not started call match prep UI update
        TextManagment();
    }
    // TODO Managable game settings, warmup time, player limit, etc.
    private void TextManagment()
    {
        // Message output, depends on game prep state.
        switch (GameState())
        {
            case GameStates.Match_Started:
                MatchCountDownTxt.text = string.Empty;
                WaveTxt.text = string.Empty;
                break;
            case GameStates.Wait_Players:
                MatchCountDownTxt.text = "Waiting for players...";
                break;
            case GameStates.Start_Match:
                MatchCountDownTxt.text = countDownTimer.ToString();
                if(countDownTimer == 1)
                {
                    WaveTxt.text = "";
                    MatchCountDownTxt.color = Color.green;
                    MatchCountDownTxt.text = "Good Luck!";
                }
                break;
        }
    }
}

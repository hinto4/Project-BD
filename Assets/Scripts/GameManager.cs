using UnityEngine;
using System.Linq;

[RequireComponent(typeof(GameNetworkManager))]
public class GameManager : MonoBehaviour
{
    private GameNetworkManager _gameNetworkManager;
    private GameTimer _gameTimer;
    private float _startTimer;
    private int _minPlayers;

    public static int countDownTimer = 5;      // countdown to start match.

    public enum GameStates
    {
        Wait_Players,
        Start_Match,
        Match_Started,
    };

    void Start()
    {
        _gameNetworkManager = GameObject.FindObjectOfType<GameNetworkManager>();
        _gameTimer = GameObject.FindObjectOfType<GameTimer>();
        // Set value from GameNetworkManager minimum players to start the match.
        _minPlayers = _gameNetworkManager.MinPlayers;
    }

    void Update()
    {
        if(GameState() == GameStates.Match_Started)
        {
            foreach(ObstacleSpawner obsSpawner in FindObjectsOfType<ObstacleSpawner>())
            {
                obsSpawner.Spawn();
            }

            foreach(SpinnerObstacle spinnerObstacle in FindObjectsOfType<SpinnerObstacle>())
            {
                spinnerObstacle.SpinObstacles();
            }
        }
        if(GameState() == GameStates.Start_Match)
        {
            countDownTimer = _gameTimer.CountDownTimer();
        }

        Debug.Log(GameState());
    }

    public GameStates GameState()
    {
        // Get's "player" tags objects in the scene.
        int connectedPlayers = GameObject.FindGameObjectsWithTag("Player").Count<GameObject>();

        if(connectedPlayers >= _minPlayers)
        {          
            if(countDownTimer > 0)
            {
                return GameStates.Start_Match;
            }
            else
            {
                return GameStates.Match_Started;
            }
        }
        else
        {
            return GameStates.Wait_Players;
        }
    }
}

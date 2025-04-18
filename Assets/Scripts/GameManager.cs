using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const int NUM_LEVELS = 1;

    private Ball ball;
    private Paddle paddle;
    private Brick[] bricks;

    public int level { get; private set; } = 1;
    public int score { get; private set; } = 0;
    private bool isGameOver = false;
    private int remainingBricks;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindSceneReferences();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void FindSceneReferences()
    {
        ball = FindObjectOfType<Ball>();
        paddle = FindObjectOfType<Paddle>();
        bricks = FindObjectsOfType<Brick>();
    }

    private void NewGame()
    {
        score = 0;
        isGameOver = false;
        LoadLevel(1);
    }

    private void LoadLevel(int level)
    {
        this.level = level;

        if (level > NUM_LEVELS)
        {
            WinGame();
            return;
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Game")
        {
            SceneManager.sceneLoaded += OnLevelLoaded;
            SceneManager.LoadScene("Game");
        }
        else
        {
            OnLevelLoaded(scene, LoadSceneMode.Single);
        }
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        FindSceneReferences();
        remainingBricks = CountBreakableBricks();
        
        if (ball != null) ball.ResetBall();
        if (paddle != null) paddle.ResetPaddle();
        
        isGameOver = false;
    }

    private int CountBreakableBricks()
    {
        int count = 0;
        foreach (Brick brick in bricks)
        {
            if (!brick.unbreakable) count++;
        }
        return count;
    }

    public void OnBrickHit(Brick brick)
    {
        score += brick.points;

        if (!brick.gameObject.activeInHierarchy) // Brick was destroyed
        {
            remainingBricks--;
            if (remainingBricks <= 0)
            {
                Debug.Log("Level Complete!");
                LoadLevel(level + 1);
            }
        }
    }

    public void OnBallLost()
    {
        GameOver();
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log($"Game Over! Final Score: {score}");
            // Return to main menu after 2 seconds
            Invoke(nameof(ReturnToMainMenu), 2f);
        }
    }

    private void WinGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log($"Congratulations! You've completed all levels! Final Score: {score}");
            // Return to main menu after 2 seconds
            Invoke(nameof(ReturnToMainMenu), 2f);
        }
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        // Add escape key to return to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }
}

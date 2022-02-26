using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public float playTime = 50;

    [Header("GameObject Binding")]
    [SerializeField] Player player;
   
    [Header("UI Binding")]
    [SerializeField] GameObject gameView;
    [SerializeField] GameObject LoseView;
    [SerializeField] GameObject ResultView;
    [SerializeField] Text timeText;
    [SerializeField] Text coinText;
    [SerializeField] Text resultCoinText;

    protected int coin;
    public float time;
    protected bool isPlaying = false;
    protected bool waitForNewCoin = false;


    // Start is called before the first frame update
    void Start()
    {
        SetupPlayer();
        StartGame();
    }

	void SetupPlayer()
	{
        player.onCollectCoin = () =>
        {
			if(isPlaying)
			{
                AddCoin();
			}
        };
	}
    void Update()
    {
		if(isPlaying)
		{
            HandlePlayLogic();
        }
    }

	void HandlePlayLogic()
	{
        time -= Time.deltaTime;
		if(time < 0)
		{
            EndGame();
            return;
		}

 
        UpdateTimeValue();
	}

	void Reset()
	{
        coin = 0;
		time = playTime + 0.3f;
        waitForNewCoin = false;
    }

	public void StartGame()
	{
        Reset();
        ShowGameView();
     
        isPlaying = true;

    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
        Reset();
        ShowGameView();
       

        isPlaying = true;
    }

    public void EndGame()
    {
        isPlaying = false;
        ShowResultView();
    }

    public void GameResult()
    {
        isPlaying = false;
        Result();
    }

	public void AddCoin()
	{
        coin++;
        UpdateCoinValue();
	}

	public void ShowGameView()
	{
        UpdateTimeValue();
        UpdateCoinValue();
        gameView.SetActive(true);
        LoseView.SetActive(false);
	}

    public void ShowResultView()
    {
        resultCoinText.text = coin.ToString();

        gameView.SetActive(false);
        LoseView.SetActive(true);
    }

    public void Result()
    {
        resultCoinText.text = coin.ToString();

        gameView.SetActive(false);
        ResultView.SetActive(true);
    }

    void UpdateTimeValue()
	{
        timeText.text = time.ToString("00");
	}

	void UpdateCoinValue()
	{
        coinText.text = coin.ToString();
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

   

}

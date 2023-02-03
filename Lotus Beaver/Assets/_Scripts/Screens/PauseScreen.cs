public class PauseScreen : Screen
{
    public void ResumeGame()
    {
        if (GameManager.GameState == GameState.GameOver)
        {
            GameManager.StartNewGame();
        }

        SetActive(false);
    }

    public override void SetActive(bool active)
    {
        base.SetActive(active);

        GameManager.SetGameState(active? GameState.Paused : GameState.Ingame);
    }
}
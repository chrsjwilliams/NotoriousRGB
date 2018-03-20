using UnityEngine;
using UnityEngine.UI;

public class TitleSceneScript : Scene<TransitionData>
{
    public KeyCode startGame = KeyCode.Space;

    [SerializeField]private float SECONDS_TO_WAIT = 0.5f;

    private TaskManager _tm = new TaskManager();

    private Text title;
    private Text clickToStart;

    private Color bandColor;

    internal override void OnEnter(TransitionData data)
    {
        title = GameObject.Find("Title").GetComponent<Text>();
        clickToStart = GameObject.Find("CLICK TO PLAY").GetComponent<Text>();
        bandColor = new Color(132f/256 , 128f/256, 146f/256);

    }

    internal override void OnExit()
    {

    }

    private void StartGame()
    {
        _tm.Do
        (
                    new LERPColor(title, bandColor, Color.white, 0.5f))
              .Then(new LERPColor(clickToStart, Color.white, bandColor, 0.25f))
              .Then(new WaitTask(SECONDS_TO_WAIT))
              .Then(new ActionTask(ChangeScene)
        );
    }

    private void TitleTransition()
    {

    }

    private void ChangeScene()
    {
        Services.Scenes.Swap<GameSceneScript>();
    }

    private void Update()
    {
        _tm.Update();
        if (Input.GetKeyDown(startGame) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Services.AudioManager.PlayClip(Clips.CLICK);
            StartGame();
        }
    }
}

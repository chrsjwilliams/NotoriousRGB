using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private KeyCode RESTART_GAME = KeyCode.Backspace;

    private const string RELOAD_GAME = "_Main";

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    public float [] targetColorArray { get; private set; }
    public bool shuffled { get; private set; }

    [SerializeField] private int _numPlayers;
    public int NumPlayers
    {
        get { return _numPlayers; }
        private set
        {
            if (_numPlayers <= 0)
            {
                _numPlayers = 1;
            }
            else
            {
                _numPlayers = value;
            }
        }
    }

    public Color previousGoalColor { get; private set; }


    [SerializeField] private Camera _mainCamera;
    public Camera MainCamera
    {
        get { return _mainCamera; }
    }

    public void Init()
    {
        NumPlayers = 1;
        _mainCamera = Camera.main;
        previousGoalColor = Color.white;
        shuffled = false;
        GenerateColorArray();
        Services.EventManager.Register<KeyPressedEvent>(OnKeyPressed); 
    }

    private void OnKeyPressed(KeyPressedEvent e)
    {
        if (e.key == RESTART_GAME) ReloadGame();
    }


    private void GenerateColorArray()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);

        targetColorArray = new float[] { r, g, b };
    }

    public void ShowInstructions()
    {
        Services.Scenes.PushScene<InstructionSceneScript>();
    }

    public void PopScene()
    {
        Services.Scenes.PopScene();
    }

    public void ReloadGame()
    {
        GenerateColorArray();
        SceneManager.LoadScene(RELOAD_GAME);
    }

    public void RefreshButton()
    {
        Debug.Assert(Services.Scenes.CurrentScene is GameSceneScript);

        Color textColor = float_array_to_Color(targetColorArray);
        shuffled = true;
        RefreshGame(textColor);
    }

    public void RefreshGame(Color textColor)
    {
        previousGoalColor = textColor;
        Services.Scenes.PopScene();
        GenerateColorArray();
        Services.Scenes.PushScene<GameSceneScript>();    
    }

	// Use this for initialization
	public void Init (int players)
    {
        NumPlayers = players;
        _mainCamera = Camera.main;
	}
	
    public void ChangeCameraTo(Camera camera)
    {
        _mainCamera = camera;
    }

    public void ResetShuffle() { shuffled = false; }

    // Update is called once per frame
    void Update()
    {
        Services.InputManager.Update();

        if (Input.GetMouseButtonDown(LEFT_CLICK))
        {
            Services.AudioManager.PlayClip(Clips.CLICK);
        }
    }

    public static Color float_array_to_Color(float[] arr)
    {
        Debug.Assert(arr.Length == 3);

        return new Color(arr[0], arr[1], arr[2]);
    }

    public static float[] color_to_float_array(Color color)
    {
        return new float[] { color.r, color.g, color.b };
    }

    private Color vector3_to_color(Vector3 vec)
    {
        return new Color(vec.x, vec.y, vec.z);
    }

    private Vector3 color_to_vector3(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }
}

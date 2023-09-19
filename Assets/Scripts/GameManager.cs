using Assets.Scripts;
using BindingOfChars;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public PlayerController PlayerController { get; private set; }
    public Health PlayerHealth { get; private set; }
    public CharacterStats PlayerStats { get; private set; }
    
    private static GameManager _instance;
    
    public static GameManager Instance { 
        get
        {
            _instance = FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                _instance = obj.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        var go = Instantiate(Prefabs.Player, Vector3.zero, Quaternion.identity);
        PlayerController = go.GetComponent<PlayerController>();
        PlayerHealth = go.GetComponent<Health>();
        PlayerStats = go.GetComponent<CharacterStats>();
    }

    private void Update()
    {
        //TODO: CHECK Is not GAMEOVER STATE PREVIOUS
        if (!PlayerHealth.IsDead && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        if (PlayerHealth.IsDead && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

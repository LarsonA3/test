using UnityEngine;

public class GameManager : MonoBehaviour
{

    // GAME MANAGER
    // will control the level, spawns, moving to next, score, lives, game over, etc


    //make sure it doesnt get destroyed when switching scenes.
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

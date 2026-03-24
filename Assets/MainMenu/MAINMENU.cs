using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MAINMENU : MonoBehaviour
{

    public Button playButton;
    private AudioSource audiosrc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audiosrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButtonClick()
    {
        print("button clicked");
        audiosrc.Play();
        SceneManager.LoadScene("ShooterGame");
        
    }
}

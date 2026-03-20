using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    // GAME MANAGER
    // will control the level, spawns, moving to next, score, lives, game over, etc
    public Slider sliderEnergy;
    public TextMeshProUGUI ScoreText;

    public Image Lives1; public Image Lives2; public Image Lives3;

    public static GameManager instance; // so other things can call this
    //vars
    public int Lives = 3;
    public int Points = 0;
    public int maxEnergy = 20; // bullet = 2 energy

    public int Energy = 20; // current energy
    
    //make sure it doesnt get destroyed when switching scenes.
    private void Awake()
    {
        Lives = 3;
        instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Lives <= 0)
        {
            //GAME OVER
        }

        ////increase energy by 1 each second
        //If 1 second or more has passed
        if (timer >= 1f)
        {
            if (Energy > maxEnergy - 1) return;
            if (Energy > 9)
            {
                Energy = Energy + 2; // gives player 2 energy if above half. rewards player for conservation and punishes spam
                
            } else
            {
                Energy++;
            }
            timer = 0f;   // Reset timer
        }

        if (Points < 0) Points = 0; //makes sure points cant go below 0. might be moved to method later
        sliderEnergy.value = (float)Energy / maxEnergy; //set 



        

    }


    public void RestoreLives()
    {
        print("Game Manager restored lives to 3");
        Lives = 3;
    }

    //function that removes 1 life
    public void LoseLife()
    {
        Lives--;
        print("lost life");
        //additionally, remove x points
        removePoints(10);
        print("removed pts");
        //CHANGE UI TO REPRESENT CORRECT LIVES
        if (Lives == 0)
        {
            Lives1.gameObject.SetActive(false);
            Lives2.gameObject.SetActive(false);
            Lives3.gameObject.SetActive(false);
        }
        else if (Lives == 1)
        {
            Lives1.gameObject.SetActive(true);
            Lives2.gameObject.SetActive(false);
            Lives3.gameObject.SetActive(false);
        }
        else if (Lives == 2)
        {
            Lives1.gameObject.SetActive(true);
            Lives2.gameObject.SetActive(true);
            Lives3.gameObject.SetActive(false);
        }
        else if (Lives == 3)
        {
            Lives1.gameObject.SetActive(true);
            Lives2.gameObject.SetActive(true);
            Lives3.gameObject.SetActive(true);
        }
        else
        {
            print("ERROR: NOT ENOUGH UI SPRITES TO REPRESENT HEALTH, OR OUT OF RANGE");
            Lives1.gameObject.SetActive(false);
            Lives2.gameObject.SetActive(false);
            Lives3.gameObject.SetActive(false);
        }


        // GAME OVER LOGIC HANDLED IN PLAYER SCRIPT
    }

    //function that player calls to see if they can fire or not
    public bool CanFire()
    {
        if (Energy >1)
        {
            return true;
        } else
        {
            return false;
        }

    }

    //function that removes an energy (player calls upon firing)
    public void RemoveEnergy()
    {
        Energy = Energy - 2;
    }

    public void awardPoints(int pts, string msg)
    {
        Points = Points + pts;
        ScoreText.text = "SCORE: " + Points;
        Debug.Log(msg + ", awarded " + pts + " points!");
    }
    public void removePoints(int pts)
    {
        Points = Points - pts;
        ScoreText.text = "SCORE: " + Points;
        Debug.Log($"REMOVED {pts} points!"); // string interpolation example
    }

}

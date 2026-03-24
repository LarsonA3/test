using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // GAME MANAGER
    public Slider sliderEnergy;
    public TextMeshProUGUI ScoreText;

    public Image Lives1; public Image Lives2; public Image Lives3;

    public static GameManager instance;

    public int Lives = 3;
    public int Points = 0;
    public int maxEnergy = 20;
    public int Energy = 20;

    private bool isInvulnerable = false;

    // LEVEL / KILL TRACKING
    public int KillCount = 0;
    public int CurrentLevel = 1;
    private const int KillsPerLevel = 5;
    private const int MaxLevel = 4; // level 4 = boss

    // Event so DEMOgame can listen for level ups
    public System.Action<int> OnLevelUp;
    // Event so DEMOgame knows when boss level is reached
    public System.Action OnBossLevel;

    private void Awake()
    {
        Lives = 3;
        instance = this;
        KillCount = 0;
        CurrentLevel = 1;
        Energy = maxEnergy;
    }

    void Start() { }

    float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;

        if (Lives <= 0)
        {
            // GAME OVER - handled in Player.cs
        }

        // Time-based energy regen (baseline)
        if (timer >= 1f)
        {
            if (Energy < maxEnergy)
            {
                if (Energy > 14)
                {
                    Energy = Mathf.Min(Energy + 3, maxEnergy);
                }
                else if (Energy > 4)
                {
                    Energy = Mathf.Min(Energy + 2, maxEnergy);
                }
                else
                {
                    Energy = Mathf.Min(Energy + 1, maxEnergy);
                }
            }
            timer = 0f;
        }

        if (Points < 0) Points = 0;
        sliderEnergy.value = (float)Energy / maxEnergy;
    }

    // Call this on every kill (from enemy/asteroid death)
    public void RegisterKill()
    {
        KillCount++;
        Debug.Log($"RegisterKill called — KillCount: {KillCount}, CurrentLevel: {CurrentLevel}");

        // Refill 50% of max energy on kill, capped at max
        Energy = Mathf.Min(Energy + (maxEnergy / 2), maxEnergy);

        // Check for level up
        if (CurrentLevel < MaxLevel)
        {
            if (KillCount % KillsPerLevel == 0)
            {
                CurrentLevel++;
                if (CurrentLevel >= MaxLevel)
                {
                    // Boss level reached
                    OnBossLevel?.Invoke();
                }
                else
                {
                    OnLevelUp?.Invoke(CurrentLevel);
                }
            }
        }
    }

    public void RestoreLives()
    {
        print("Game Manager restored lives to 3");
        Lives = 3;
        Lives1.gameObject.SetActive(true);
        Lives2.gameObject.SetActive(true);
        Lives3.gameObject.SetActive(true);
    }

    public void LoseLife()
    {
        if (isInvulnerable) return;
        Lives--;
        print("lost life");
        removePoints(10);

        if (Lives < 1)
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
    }

    public bool CanFire()
    {
        return Energy > 1;
    }

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
        Debug.Log($"REMOVED {pts} points!");
    }

    public void setInvulnerable(bool i)
    {
        isInvulnerable = i;
    }
}
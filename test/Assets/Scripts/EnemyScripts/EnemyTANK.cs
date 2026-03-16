using UnityEngine;

public class EnemyTANK : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        // spawn logic - spawns randomly on right
    }

    // Update is called once per frame
    void Update()
    {
        //move left onto screen until in right position, then NO MORE. can be tied to fps, is okay.

        //move on y axis randomly slowly

        //shoot Enemyprojectile aimed directly at player every so often (maybe random chance to shoot missile..?)

    }

    //check for if shot (award points)

    //check for if physically touched (dmg plr then destroy)

    //check for asteroid hit
}

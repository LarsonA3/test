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

        //if health = 0 award points and destroy
    }

    //check for if shot (reduce hp)

    //check for if physically touched (dmg plr then destroy)

    //check for asteroid hit (reduce hp and destroy asteroid)
}

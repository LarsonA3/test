using UnityEngine;

public class EnemyFRONTfollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Awake()
    {
        // spawn logic - spawns randomly on right
        // aim toward player, with some slight deviation of like -15 to 15 degrees smthn
    }

    // Update is called once per frame
    void Update()
    {
        //slowly moves toward player left

        //if past screen x or y, destroy.

        //shoot Enemyprojectile forward aimlessly every so often

    }

    //check for if shot (award points)

    //check for if physically touched (dmg plr then destroy)

    //check for asteroid hit

}

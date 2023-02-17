using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControls : MonoBehaviour
{
    [SerializeField] GameObject playerCharacter;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerWalking();

    }
    public void PlayerWalking()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            playerCharacter.GetComponent<Animator>().Play("Walk");
        }
        else if (Input.GetButtonUp("Vertical"))
        {
            playerCharacter.GetComponent<Animator>().Play("Idle");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [SerializeField] int knockBack;
    [SerializeField] bool push;
    [SerializeField] int dmg;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!push)
                gameManager.instance.playerScript.PushBackDir((transform.position - gameManager.instance.player.transform.position).normalized * knockBack);
            else
                gameManager.instance.playerScript.PushBackDir((gameManager.instance.player.transform.position - transform.position).normalized * knockBack);

        }
        else if (other.GetComponent<IDamage>() != null)
        {
            other.GetComponent<Collider>().GetComponent<IDamage>().takeDamage(dmg);
        }
    }
}

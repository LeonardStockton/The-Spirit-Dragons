using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove_BullHole : MonoBehaviour
{
    [SerializeField] SpriteRenderer bullHole;
    [SerializeField] float fadeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fadeout());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator Fadeout()
    {
        Color tempClr = bullHole.color;
        while (tempClr.a > 0f)
        {
            tempClr.a -= Time.deltaTime / fadeTime;
            if(tempClr.a <= 0f)
            {
                tempClr.a = 0.0f;
            }
            yield return null;
        }
        bullHole.color = tempClr;
    }
}

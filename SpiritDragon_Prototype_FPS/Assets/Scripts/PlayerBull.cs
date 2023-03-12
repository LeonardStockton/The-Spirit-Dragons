using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBull : MonoBehaviour
{
    private Vector3 shootDir;
    [SerializeField] float speed;
    [SerializeField] int bulletDmg;
    [SerializeField] float timer;
    public void SendBull(Vector3 shootDir)
    {
        this.shootDir = shootDir;
    }
    void Start()
    {
        Destroy(gameObject, timer);
    }
    private void Update()
    {
        transform.position += shootDir * Time.deltaTime * speed;
    }
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.GetComponent<IDamage>() != null)
        {
            obj.GetComponent<IDamage>().takeDamage(bulletDmg);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
    public float getSpeed()
    {
        return speed;
    }
}

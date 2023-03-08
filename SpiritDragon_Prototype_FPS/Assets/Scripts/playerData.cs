using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class playerData 
{
    public int _Level;
    public int _Health;
    public float[] postion;

    public  playerData( playerController _player)
    {
        _Level = SceneManager.GetActiveScene().buildIndex;
        _Health = _player.GetPlayerHealth();
        postion = new float[3];
        postion[0] = _player.transform.position.x;
        postion[1] = _player.transform.position.y;
        postion[2] = _player.transform.position.z;
    }
}

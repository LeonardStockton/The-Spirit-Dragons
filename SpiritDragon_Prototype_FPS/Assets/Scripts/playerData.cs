using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class playerData 
{
    public int _Level;
    public int _Health;
    public float[] postion;

    public playerData( gameManager.instance.playerController _player)
    {
        _Level = _player._Level;;
        _Health = _player._Health;
        postion = new float[3];
        postion[0] = _player.transform.position.x;
        postion[1] = _player.transform.position.y;
        postion[2] = _player.transform.position.z;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models 
{
    PlayerModel _playerModel = new PlayerModel();
    TimeModel _timeModel = new TimeModel();


    public PlayerModel playerModel => _playerModel;
    public TimeModel timeModel => _timeModel;
}

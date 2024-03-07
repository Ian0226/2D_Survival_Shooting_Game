using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGame2DSystems
{
    private static ShootingGame2DSystems _instance;
    public static ShootingGame2DSystems Instance
    {
        get { if (_instance == null) { _instance = new ShootingGame2DSystems();}
            return _instance;
        }
    }
    private ShootingGame2DSystems() { }

    public void Initialize()
    {

    }

    public void Update()
    {

    }
}

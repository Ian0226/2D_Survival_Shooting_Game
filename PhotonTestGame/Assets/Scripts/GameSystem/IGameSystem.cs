using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game systems shared interface.
/// </summary>
public class IGameSystem
{
    protected ShootingGame2DSystems _shootingGame2D = null;
    public IGameSystem(ShootingGame2DSystems shootingGame2D)
    {
        _shootingGame2D = shootingGame2D;
    }

    public virtual void Initialize() { }
    public virtual void Update() { }
    public virtual void Release() { }
}

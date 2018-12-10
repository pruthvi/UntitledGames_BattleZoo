using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    public PlatformState platformState;
    protected abstract void OnPlatformMoving();
 //   protected abstract void OnFalling();
      
}

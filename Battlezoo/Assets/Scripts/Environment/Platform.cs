using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    protected PlatformState State { get; private set; }
    protected abstract void OnPlatformMoving();
 //   protected abstract void OnFalling();
      
}

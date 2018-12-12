using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Platform : NetworkBehaviour
{
    public PlatformState platformState;
    [ClientRpc]
    protected abstract void RpcOnPlatformMoving();
 //   protected abstract void OnFalling();
      
}

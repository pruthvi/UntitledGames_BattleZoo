using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterTransformSync : BasicTransformSync {

    public Character character;

    public FaceDirection InitialFacing = FaceDirection.Right;

    [SyncVar(hook = ("OnBarrelAngleChanged"))]
    public float barrelAngle;

    [SyncVar(hook = ("OnDirectionChanged"))]
    public int Direction;

    [SyncVar]
    public int ammoLeft = 30;

    void Start()
    {
        Direction = (int)InitialFacing;
    }

    // Barrel Angle

    void OnBarrelAngleChanged(float newAngle)
    {
        barrelAngle = newAngle;
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, barrelAngle));
        character.barrel.transform.rotation = Direction == -1 ? Quaternion.Inverse(rotation) : rotation;
    }

    public void OnChangingBarrelAngle(float newAngle)
    {
        CmdChangeBarrelAngle(newAngle);
    }

    [Command]
    void CmdChangeBarrelAngle(float newAngle)
    {
        barrelAngle = newAngle;
    }

    // ============

    // Player Direction

    // Call back for SyncVar - Direction
    // This will change the local property when Sync from the server
    void OnDirectionChanged(int newDir)
    {
        Direction = newDir;
        character.characterSprites.localScale = new Vector3(newDir, transform.localScale.y, transform.localScale.z);
    }
    // Called when change value
    public void OnChangingDirection(int newDir)
    {
        CmdChangeDirecion(newDir);
    }
    // Notice the server
    [Command]
    void CmdChangeDirecion(int newDir)
    {
        //  Debug.Log("CmdChangeDirecion | " + Direction);
        Direction = newDir;
    }

    // This will change the local property when Sync from the server
    void OnPositionChanged(float x, float y, float z)
    {   
        transform.Translate(x, y, z);
    }
    // Called when change value
    public void OnChangingPosition(float x, float y, float z)
    {
        CmdChangePosition(x, y, z);
    }
    // Notice the server
    [Command]
    void CmdChangePosition(float x, float y, float z)
    {
        transform.Translate(x, y, z);
    }
}

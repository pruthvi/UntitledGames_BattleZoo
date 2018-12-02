using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "BattleZoo/Bullet")]
public class BulletControllerScriptableObject : ScriptableObject {

    public GameObject bullet;
    public float damage;
    public float bulletLifeTime;
    public float speed;

    // Bullet
    [Header("Bullet Reloading")]
    public int _maxAmmo = 6;           // Total amount of bullets in Magazine
    public float _bulletReloadTime = 5;  // total Time takes to reload the magazine

    
}

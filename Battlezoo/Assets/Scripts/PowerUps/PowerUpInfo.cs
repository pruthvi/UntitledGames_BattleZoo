using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up", menuName ="BattleZoo/Power Up")]
public class PowerUpInfo : ScriptableObject {

	public string powerUpName;
	public string powerUpDescription;
	public Sprite powerUpIcon;
	public float duration = 1;
	public bool useMultiplier = false;
	public float multiplier = 1;
	public float amount;
	public PowerUpType PowerUpType;
}

// Balance Platform
public enum PlatformState { Idle, Active, Return };
public enum GravityPlatformState { Up, Down };
public enum BalancePlatformState { Balanced, Left, Right };

// Power Up
public enum PowerUpType { NoEffect, Speed, Strength, JumpStrength, Heal, Poison };

// Poison Gas Zone
public enum GasZoneState { SafeTimeBegins, SafeTime, SafeTimeEnds, ShrinkingBegins, Shrinking, ShrinkingEnds };

// Combat
public enum DamageSourceType { Player, NPC, PoisonZone };
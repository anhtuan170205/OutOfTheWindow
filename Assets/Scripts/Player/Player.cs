using UnityEngine;

public class Player : SingletonMonoBehaviour<Player>
{
    [SerializeField] private Health health;
    [SerializeField] private ActiveWeapon activeWeapon;

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private FloatVariable playerMaxHealth;
    [SerializeField] private FloatVariable lightAttackDamage;
    
    [Header("Event System")]
    [SerializeField] private GameEvent deathEvent;
    [SerializeField] private GameEvent damageEvent;
    [SerializeField] private UnityEvent DamageEvent;
    [SerializeField] private UnityEvent DeathEvent;
    
   
    
    
    private void Awake()
    {
        playerHealth.SetValue(playerMaxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Damage Player");
            playerHealth.SetValue(playerHealth.Value - lightAttackDamage.Value);
            DamageEvent.Invoke();
            damageEvent.Raise();
        }

        if (playerHealth.Value == 0)
        {
            DeathEvent.Invoke();
            deathEvent.Raise();
        }
    }
}

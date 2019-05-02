﻿using UnityEngine;

// This script kills the player when it collides with the KillTile.

public class KillTile : MonoBehaviour
{
    private void Update()
    {
        Collider2D other = Physics2D.OverlapBox(transform.position, transform.localScale, 0);
        if(other.CompareTag("Player"))
        {
            PlayerStats.Instance.TakeDamage(PlayerStats.Instance.MaxHealth);
        }
    }
}
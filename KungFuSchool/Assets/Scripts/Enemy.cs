﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public float health = 100;
    private GameObject target;
    float currentHealth;
    public float enemyExp = 600;
    public Image healthBar;

    // Use this for initialization
    void Start () {
        currentHealth = health;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDameged(int dameged){
        currentHealth -= dameged;
        healthBar.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();
            col.enabled = false;
            target.gameObject.GetComponent<PlayerLevel>().SendMessage("addExp", enemyExp);
            //anim.SetTrigger("Dead");
            //Do everything you want with this part, but before destroying the enemy, add this:
            string[] spawnerName = gameObject.name.Split('_');
            GameObject.Find(spawnerName[0]).GetComponent<Spawner>().Death = true;
            Destroy(gameObject, 0.5f);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public float health = 100;
    private GameObject target;
    public QuestManager questManager;
    float currentHealth;
    public float enemyExp = 600;
    public Image healthBar;
    public GameObject[] dropingItem;
    public GameObject Gold;
    public int goldDropping;
    // Use this for initialization
    void Start () {
        questManager = FindObjectOfType<QuestManager>();
        currentHealth = health;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDameged(int dameged){
        currentHealth -= dameged;
        healthBar.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            CapsuleCollider2D col = gameObject.GetComponent<CapsuleCollider2D>();
            col.enabled = false;
            if (goldDropping > 0)
            {
                int index = Random.Range(0, dropingItem.Length + 1);
                if (index == dropingItem.Length)
                {
                    InteractionObject intaer = Gold.GetComponent<InteractionObject>();
                    intaer.gold = goldDropping;
                    Instantiate(Gold, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(dropingItem[index], transform.position, Quaternion.identity);
                }
            }
            else
            {   
                if (dropingItem.Length > 0)
                {
                    int index = Random.Range(0, dropingItem.Length);
                    Instantiate(dropingItem[index], transform.position, Quaternion.identity);
                }

            }
            //target.gameObject.GetComponent<PlayerLevel>().SendMessage("addExp", enemyExp);
            string[] spawnerName = gameObject.name.Split('_');
            GameObject.Find(spawnerName[0]).GetComponent<Spawner>().Death = true;
            Destroy(gameObject, 0.5f);
        }
    }
}

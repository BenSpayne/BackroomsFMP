using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisable : MonoBehaviour
{
    [SerializeField] GameObject Enemy;

    void Start()
    {
        Enemy.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
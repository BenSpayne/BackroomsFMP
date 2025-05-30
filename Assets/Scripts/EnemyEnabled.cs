using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabled : MonoBehaviour
{
    [SerializeField] GameObject Enemy;

    private bool EnemyObj = false;
    // Start is called before the first frame update
    void Start()
    {
        Enemy.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Enemy.gameObject.SetActive(true);
            EnemyObj = true;
            this.gameObject.SetActive(false);
        }
        else
        {
            Enemy.gameObject.SetActive(false);
            EnemyObj = false;
        }
            
    }
        
}

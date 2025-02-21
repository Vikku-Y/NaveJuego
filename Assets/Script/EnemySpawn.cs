using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject gameplay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && other.GetComponent<EnemyClass>().spawnable)
        {
            other.GetComponent<EnemyClass>().spawnable = false;

            GameObject spawnedEnemy = Instantiate(other.gameObject, new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z), other.transform.rotation, gameplay.transform);
            spawnedEnemy.GetComponent<EnemyClass>().enabled = true;
            spawnedEnemy.GetComponent<MeshRenderer>().enabled = true;

            

            if (other.transform.parent.name == "Boss")
            {
                spawnedEnemy.GetComponentsInChildren<EnemyShoot>()[0].activated = true;
                spawnedEnemy.GetComponentsInChildren<EnemyShoot>()[1].activated = true;
            } else
            {
                spawnedEnemy.GetComponentInChildren<EnemyShoot>().activated = true;
            }
        }
    }
}

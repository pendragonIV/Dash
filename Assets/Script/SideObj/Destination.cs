using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.IsGameWin() || GameManager.instance.IsGameLose())
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Win!");
            GameManager.instance.Win();
            //Time.timeScale = 0;
        }
    }
}

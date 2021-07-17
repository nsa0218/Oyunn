using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    private GameObject player;


    public GameObject deathEffect;

    public float respawnTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
       if (Launcher2.instance.selectedCharacter==0)
       {
            Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();
             player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
       }
       if (Launcher2.instance.selectedCharacter==1)
       {
           Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();
              player = PhotonNetwork.Instantiate(playerPrefab2.name, spawnPoint.position, spawnPoint.rotation);
       }
         if (Launcher2.instance.selectedCharacter==2)
       {
           Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();
              player = PhotonNetwork.Instantiate(playerPrefab3.name, spawnPoint.position, spawnPoint.rotation);
       }
        if (Launcher2.instance.selectedCharacter==3)
       {
           Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();
              player = PhotonNetwork.Instantiate(playerPrefab4.name, spawnPoint.position, spawnPoint.rotation);
       }
      
    }

    public void Die(string damager)
    {
        

        UIController.instance.deathText.text = "You were killed by " + damager;
       

        //PhotonNetwork.Destroy(player);

        //SpawnPlayer();

        MatchManager.instance.UpdateStatsSend(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);

        if(player != null)
        {
            StartCoroutine(DieCo());
        }
    }

    public IEnumerator DieCo()
    {
        PhotonNetwork.Instantiate(deathEffect.name, player.transform.position, Quaternion.identity);

        PhotonNetwork.Destroy(player);
        player = null;
     //  UIController.instance.ReturnToMainMenu(); 
       UIController.instance.deathScreen.SetActive(true);
        

         yield return new WaitForSeconds(respawnTime);

        UIController.instance.deathScreen.SetActive(false);

        if (MatchManager.instance.state == MatchManager.GameState.Playing && player == null)
        {
           SpawnPlayer();
        }
    }
}

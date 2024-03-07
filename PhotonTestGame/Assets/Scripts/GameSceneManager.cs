using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class GameSceneManager : MonoBehaviour
{
    public GameObject testEnemy;

    [Header("UI Properties")]
    public GameObject LosePanel;
    public GameObject PlayTimeDisplay;

    [Header(" ")]
    [SerializeField]
    private List<Transform> playerList = new List<Transform>();

    [Header("InstantiateObjectPoint")]
    public List<Transform> playerInsPos;
    public List<Transform> enemyInsPos;

    private float insEnemyRate = 3f;
    private float nextTimeToIns = 0f;

    private bool gameIsStart = false;

    private bool gameIsLose = false;
    public bool GameIsLose
    {
        get { return gameIsLose; }
        set { gameIsLose = value; }
    }

    private bool gameIsWin = false;
    public bool GameIsWin
    {
        get { return gameIsWin; }
        set { gameIsWin = value; }
    }

    public List<Transform> GetPlayerList()
    {
        return playerList;
    }

    private void Awake()
    {
        //Call game system initialize.
        ShootingGame2DSystems.Instance.Initialize();
    }
    void Start()
    {
        Cursor.visible = false;
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            //Initialize Game
            StartCoroutine(DelayInit(1f));

            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                StartCoroutine(GameIsStart());
            }
        }
    }

    void Update()
    {
        if (gameIsStart)
        {
            //Call game system update.
            ShootingGame2DSystems.Instance.Update();
            //Instantiate Enemy
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (Time.timeSinceLevelLoad >= nextTimeToIns)
                {
                    GameObject currentInitEnemy = EnemyInstantiateSysForPhoton.EnemyInitSystemForPhoton.
                        StartInstantiateSystemOneTime(testEnemy, TransformListToVectorList.ListConvert(enemyInsPos));
                    //currentInitEnemy.GetComponent<EnemyController>().SetCurrentTarget(playerList);
                    nextTimeToIns = Time.timeSinceLevelLoad + insEnemyRate;
                }
            }
            GameWinHandler();
        }
        playerList.RemoveAll(Transform => Transform == null);
    }

    private void GameWinHandler()
    {
        if(playerList.Count == 0)
        {
            gameIsLose = true;
            LosePanel.SetActive(true);
            Cursor.visible = true;
        }
    }

    public void ReturnToRoom()
    {
        SceneManager.LoadScene("RoomScene");
    }

    IEnumerator DelayInit(float sec)
    {
        yield return new WaitForSeconds(sec);
        InitGame();
    }
    public void InitGame()
    {
        float spawnPointX = Random.Range(-3, 3);
        float spawnPointY = 2;

        GameObject playerObj = PhotonNetwork.Instantiate("PhotonPlayer", new Vector3(spawnPointX, spawnPointY, 0), Quaternion.identity);
        PhotonView playerPhotnView = playerObj.GetComponent<PhotonView>();

        HashTable table = new HashTable();
        table.Add("PlayerViewID", playerPhotnView.ViewID);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);

        StartCoroutine(UpdatePlayerListAfterDelay());
    }

    IEnumerator UpdatePlayerListAfterDelay()
    {
        yield return new WaitForSeconds(1);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.ContainsKey("PlayerViewID"))
                {
                    PhotonView photonView = PhotonView.Find((int)player.CustomProperties["PlayerViewID"]);

                    if (photonView != null)
                    {
                        playerList.Add(photonView.gameObject.transform);
                    }
                }
            }
        }
    }
    
    IEnumerator GameIsStart()
    {
        yield return new WaitForSeconds(3);

        gameIsStart = true;
    }
    
}

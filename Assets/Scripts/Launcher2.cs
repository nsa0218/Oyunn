using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class Launcher2 : MonoBehaviourPunCallbacks
{
    public static Launcher2 instance;

    [SerializeField] private GameObject findOpponentPanel = null;
    [SerializeField] private GameObject waitingStatusPanel = null;
    [SerializeField] private GameObject nameInputPanel = null;
    [SerializeField] private GameObject mainPanel = null;

    [SerializeField] private TextMeshProUGUI waitingStatusText = null;
    [SerializeField] private InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;
    [SerializeField] private Text nameInputText =null;
    public int selectedCharacter = 0;
    private const string PlayerPrefsNameKey = "Player";

    private bool isConnecting = false;

    private const int MaxPlayersPerRoom = 2;
    public string[] allMaps;
    public static bool isSignedin = false;

    private void Start()
    {
        instance = this;
        SetUpInputField();
        // if (!isSignedin)
        // {
        //     DatabaseManager.instance.StartSign();
            
        // }
        // else{
            PhotonNetwork.Disconnect();
            PhotonSavePlayerName("deneme");
            OpenFindOpponentPanel();
            
     //   }
        

    }
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    public void FindOpponent()
    {
        isConnecting = true;
        findOpponentPanel.SetActive(false);
        waitingStatusPanel.SetActive(true);
        waitingStatusText.text = "Searching..";
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected)
        {

            PhotonNetwork.JoinRandomRoom();
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        waitingStatusPanel.SetActive(false);
        findOpponentPanel.SetActive(true);

        Debug.Log($"Disconnected due to: {cause}");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No clients are waiting for an opponent, creating a new room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom, IsOpen = true });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Client succesfuly joined a room");
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCount != MaxPlayersPerRoom)
        {
            waitingStatusText.text = " Waiting for opponent..";
            Debug.Log("Client waiting for an opponent");
        }
        else
        {
            Debug.Log("Matching is ready to begin");
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;

            waitingStatusText.text = "Opponent found";
            Debug.Log("Match is ready to begin");

            PhotonNetwork.LoadLevel(allMaps[Random.Range(0, allMaps.Length)]);
           // PhotonNetwork.LevelLoadingProgress;
        }
    }
    private void SetUpInputField()
    {

        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
        nameInputField.text = defaultName;
        SetPlayerName(defaultName);


    }
    private void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }
    public void PhotonSavePlayerName(string playerName)
    {
        mainPanel.SetActive(false);
        findOpponentPanel.SetActive(true);
        PhotonNetwork.NickName = playerName;
        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);

    }

    public void NameInput()
    {
        mainPanel.SetActive(false);
        nameInputPanel.SetActive(true);
    }
    public void CreateNewPlayerOnDatabase(string playerName)
    {

        DatabaseManager.instance.CreateEmptyData(playerName);
     
    }

    public void OpenFindOpponentPanel(){
        mainPanel.SetActive(false);
        findOpponentPanel.SetActive(true);
    }

    public bool ValidatePlayername(string playerNameInput){
        if(playerNameInput == null){
            nameInputText.text = "Kullanıcı adı boş olamaz.";
            return false;

        }
        else if(playerNameInput.Length < 4){
            nameInputText.text = "Kullanıcı adı 4 karakterden büyük olmalıdır.";
            return false;
        }
        else return true;
     
    }
    public void SavePlayerName(){
        if (ValidatePlayername(nameInputField.text))
        {
            CreateNewPlayerOnDatabase(nameInputField.text);
        }
    }


}

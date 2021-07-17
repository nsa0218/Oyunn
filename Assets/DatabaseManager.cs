
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;



public class DatabaseManager : MonoBehaviour
{


    [SerializeField]

    DatabaseReference usersRef;


    public static string webClientId = "53031202573-i3drj8tvop3c4f92m0ft2djql45tsdcl.apps.googleusercontent.com";

    FirebaseAuth auth;
    FirebaseUser _currentUser;
    private GoogleSignInConfiguration configuration;
    public static DatabaseManager instance;

    public Text logtext;

    public void StartSign()
    {


        StartCoroutine(Initialization());
      

        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        





    }

    private void Awake()
    {
        
        instance = this;
        auth = FirebaseAuth.DefaultInstance;
        usersRef = FirebaseDatabase.DefaultInstance.RootReference;
    }


    public IEnumerator Initialization()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();
        while (!task.IsCompleted)
        {
            yield return null;
        }
        if (task.IsCanceled || task.IsFaulted)
        {
            Debug.LogError("database error:" + task.Exception);
        }

        var dependencyStatus = task.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            usersRef = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("init completed");
            SignInWithGoogle();
        }

        else
        {
            Debug.LogError("database error");
        }

    }
    public void GetGameData()
    {
        // auth = GoogleSignInDemo.instance.auth;


        usersRef.Child(_currentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Database Hata");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.GetRawJsonValue() == null)
                {

                    Launcher2.instance.NameInput();


                }
                else
                {


                    string nick = snapshot.Child("Username").Value.ToString();
                    Launcher2.instance.PhotonSavePlayerName(nick);




                }
            }
        });
    }

    public void CreateEmptyData(string name)
    {
        //  var auth = signInManager.auth;
        //usersRef = FirebaseDatabase.DefaultInstance.GetReference("Users");


        // string emptyJson = JsonUtility.ToJson(emptyData);
        // usersRef.Child("auth.CurrentUser.UserId").SetRawJsonValueAsync(emptyJson);
        Dictionary<string, object> user = new Dictionary<string, object>();
        user["Username"] = name;
        user["Level"] = 1;
        user["Exp"] = 10;

        usersRef.Child(_currentUser.UserId).UpdateChildrenAsync(user);
        GetGameData();
    }

    public void SendExp(double changedExp)
    {
        logtext.text = "senexp çalıştı";



        usersRef.Child(_currentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Database Hata");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.GetRawJsonValue() == null)
                {

                    Debug.Log("Veri yok");
                    logtext.text = "veri yok";


                }
                else
                {
                   logtext.text = "else çalıştı";
                   
                    PlayerModel playerModel = JsonUtility.FromJson<PlayerModel>(snapshot.GetRawJsonValue());

                   var _exp = playerModel.Exp;
                    

                  



                    var exp = _exp + changedExp; 
                     logtext.text = "exp degeri" + exp;
                    Dictionary<string, object> sendExp = new Dictionary<string, object>();

                    sendExp["Exp"] = exp;
                    usersRef.Child(_currentUser.UserId).UpdateChildrenAsync(sendExp);



                }
            }
        }
       );


    }

    public void SignOut()
    {

    }
    public void SignIn()
    {

    }
    public void Show()
    {
        Debug.Log(auth.CurrentUser.UserId);
    }
    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else { }
                //  AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                //  AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        // AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        // AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        // AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    //  AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    //  AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            // AddToInformation("Canceled");
        }
        else
        {
            // AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            // AddToInformation("Email = " + task.Result.Email);
            // AddToInformation("Google ID Token = " + task.Result.IdToken);
            // AddToInformation("Email = " + task.Result.Email);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0)) { }
                //AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {



                _currentUser = task.Result;
                GetGameData();
                Launcher2.isSignedin = true;
                //AddToInformation("Sign In Successful.");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        // AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        //   AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

}
public class PlayerModel
{
    // Start is called before the first frame update
    public string Username;
    public double Level;
    public double Exp;
    
    public PlayerModel(string username, double level, double exp)
    {
        this.Username = username;
        this.Level = level;
        this.Exp = exp;
    }
}



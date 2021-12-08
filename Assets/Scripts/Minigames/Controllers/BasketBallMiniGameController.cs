using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;

namespace Flamingo
{
public class BasketBallMiniGameController : MonoBehaviour
{
    [SerializeField] private Mateo _mateo;                          /// <summary>Mateo's Reference.</summary>
    [SerializeField] private BasketBallMiniGame _basketMiniGame;    /// <summary> Reference to BasketMiniGame that initialize the minigame       
    [SerializeField] private Vector3 _ballInitialPos;               /// <summary>Spawn Position to reset the ball on ring passed.</summary>
    [Space(5f)]
    [SerializeField] private int _cheerLocalSoundIndex;             /// <summary>Cheer's Local Sound Index.</summary>
    [SerializeField] private int _cheerVisitorSoundIndex;           /// <summary>Cheer's Visitor Sound Index.</summary>
    [Space(5f)]
    [Header("UI: ")]
    [SerializeField] private TextMesh _timeText;                    /// <summary>Time's Text.</summary>
    [SerializeField] private TextMesh _localScoreText;              /// <summary>Local Score's Text.</summary>
    [SerializeField] private TextMesh _visitorScoreText;            /// <summary>Visitor Score's Text.</summary>
    [SerializeField] private TextMesh _resultText;                  /// <summary>Result's Text.</summary>
    
#region Getters/Setters:
    /// <summary>Gets and Sets basketMiniGame property.</summary>
    public BasketBallMiniGame basketMiniGame { get { return _basketMiniGame; } }
    
    /// <summary>Get and Sets ballInitialPos property.</summary>
    public Vector3 ballInitialPos
    {
        set { _ballInitialPos = value; }
        get { return _ballInitialPos; }
    }

    /// <summary>Gets cheerLocalSoundIndex property.</summary>
    public int cheerLocalSoundIndex { get { return _cheerLocalSoundIndex; } }

        /// <summary>Gets cheerVisitorSoundIndex property.</summary>
    public int cheerVisitorSoundIndex { get { return _cheerVisitorSoundIndex; } }


    /// <summary>Gets timeText property.</summary>
    public TextMesh timeText { get { return _timeText; } }

    /// <summary>Gets localScoreText property.</summary>
    public TextMesh localScoreText { get { return _localScoreText; } }

    /// <summary>Gets visitorScoreText property.</summary>
    public TextMesh visitorScoreText { get { return _visitorScoreText; } }

    /// <summary>Gets resultText property.</summary>
    public TextMesh resultText { get { return _resultText; } }
#endregion

    /// <summary>Draws Gizmos on Editor.</summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(ballInitialPos, 0.5f);
    }

    /// <summary>Callback invoked when this is created.</summary>
    private void Awake()
    {
        BeginMiniGame();
    }

    /// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
    private void Start()
    {
        if(_mateo == null) return;

        PlayerInputController controller = PlayerInputsManager.Get(1);
        controller.AssignCharacterToMateoController(_mateo);

        Game.AddTargetToCamera(_mateo.cameraTarget);
    }

    /// <summary>Updates BaskatBallMiniGameController's instance at each frame.</summary>
    private void Update()
    {
        if(basketMiniGame.running) timeText.text = basketMiniGame.clock.ToString();
    }

    /// <summary>Callback invoked before this Object is sent to the Garbage Collector.</summary>
    private void OnDestroy()
    {
        EndMiniGame(); 
    }

#region Functions:
    /// <summary>Begins Mini-Game.</summary>
    public void BeginMiniGame()
    {
        VCameraTarget target = basketMiniGame.ball.GetComponent<VCameraTarget>();
        if(target != null) Game.AddTargetToCamera(target);
        basketMiniGame.Initialize(this, OnMiniGameEvent);
        //SubscribeBall();
    }

    /// <summary>Ends Mini-Game.</summary>
    public void EndMiniGame()
    {
        VCameraTarget target = basketMiniGame.ball.GetComponent<VCameraTarget>();
        if(target != null) Game.RemoveTargetToCamera(target);
        basketMiniGame.Terminate(this);
        //Unsubscribe();  
    }

    /// <summary>Function to subscribe ResetBall event to rings.</summary>
    private void SubscribeBall()
    {
        basketMiniGame.visitorRing.onRingPassed += ResetBallOnRingPassed;
        basketMiniGame.localRing.onRingPassed += ResetBallOnRingPassed;
    }

    private void Unsubscribe()
    {
        basketMiniGame.visitorRing.onRingPassed -= ResetBallOnRingPassed;
        basketMiniGame.localRing.onRingPassed -= ResetBallOnRingPassed;
    }

    /// <summary>Override delegate for OnRingedPassed .</summary>
    public void ResetBallOnRingPassed(Collider2D _collider)
    {
        basketMiniGame.ball.rigidbody.Sleep();
        basketMiniGame.ball.transform.position = ballInitialPos;
    }
#endregion

#region Callbacks:
    /// <summary>Callback invoked when the Basket-Ball Mini-Game invokes an Event.</summary>
    /// <param name="_miniGame">Mini-Game's Instance that invoked the Event.</param>
    /// <param name="_ID">Event's ID.</param>
    private void OnMiniGameEvent(MiniGame _miniGame, int _ID)
    {
        switch(_ID)
        {
            case MiniGame.ID_EVENT_MINIGAME_ENDED:
            timeText.text = basketMiniGame.CurrentWinner();
            EndMiniGame();
            break;

            case MiniGame.ID_EVENT_MINIGAME_SCOREUPDATE_LOCAL:
            AudioController.PlayOneShot(SourceType.Scenario, 0, cheerLocalSoundIndex);
            localScoreText.text = basketMiniGame.localScore.ToString();
            break;

            case MiniGame.ID_EVENT_MINIGAME_SCOREUPDATE_VISITOR:
            AudioController.PlayOneShot(SourceType.Scenario, 0, cheerVisitorSoundIndex);
            visitorScoreText.text = basketMiniGame.visitorScore.ToString();
            break;
        }

        ResetBallOnRingPassed(null);
    }
#endregion
}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class TalkingPerson : InteractiveObject, IGameScreen
{
    protected AudioSource personSource;
    [SerializeField] protected Transform cameraPlace;
    protected Transform lastCameraParent;
    protected Vector3 neededPosition;
    protected Quaternion neededRotation;
    protected int personLevel = 1;
    [SerializeField] protected string personName = "N/A";
    [SerializeField] protected string fraction = "N/A";
    [SerializeField] protected string personRelationAtPlayer = "N/A";
    protected enum DialogType { Opponent, Player }
    [SerializeField] protected bool canLeaveFromDialog;
    protected bool answerInDialogHasTaked = false;
    protected List<(DialogType dt, string screenText, string answerText, bool IsBreakDialog)> dialogs;
    protected Missions.MissionInteractiveObject mtaskChecker;
    protected int currentDialog = 1;
    protected float clipLingth;
    protected Animator mAnimator;
    protected NavMeshAgent mAgent;
    protected Transform target;
    protected bool interactionTakesPlace;
    private TaskDrawer taskDrawer;
    protected abstract string PathToClips();
    public override void Interact()
    {
        if (interactionTakesPlace)
            return;
        interactionTakesPlace = true;
        var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
        ddrawer.SetEnableAll(true);
        ddrawer.SetNameAndLevel(personName, personLevel);
        ddrawer.SetRelationAtPlayer(personRelationAtPlayer);
        ddrawer.SetFraction(fraction);

        var cameraPlayer = Camera.main.transform;
        lastCameraParent = cameraPlayer.parent;
        cameraPlayer.SetParent(null);
        neededPosition = cameraPlace.position;
        neededRotation = cameraPlace.rotation;
        ScreensManager.SetScreen(this);
        PlayDialogsTraker();
    }
    protected override void Awake()
    {
        personSource = GetComponent<AudioSource>();
        mtaskChecker = GetComponent<Missions.MissionInteractiveObject>();
        mAnimator = GetComponent<Animator>();
        mAgent = GetComponent<NavMeshAgent>();
        base.Awake();
    }
    protected virtual void Start()
    {
        if (cameraPlace == null)
            Debug.LogError("Camera place is null!");

        taskDrawer = FindObjectOfType<TaskDrawer>();
    }
    public bool Hide()
    {
        if (canLeaveFromDialog)
        {
            FinishDialog();
            return true;
        }
        return false;
    }

    public KeyCode HideKey() => KeyCode.Escape;
    public void Say(AudioClip audioClip) => personSource.PlayOneShot(audioClip);
    public void LateUpdate()
    {
        if (lastCameraParent)
        {
            var cameraPlayer = Camera.main.transform;
            cameraPlayer.position = neededPosition;
            cameraPlayer.rotation = neededRotation;
        }
    }
    public void PlayDialogsTraker()
    {
        FindObjectOfType<Dialogs.DialogDrawer>().ClearDialogs();
        StartCoroutine(nameof(DialogsTraker));
    }
    protected abstract IEnumerator DialogsTraker();
    protected void TakeAnswerInDialog() => answerInDialogHasTaked = true;
    public void FinishDialog()
    {
        Missions.MissionsManager.GetActiveMission().Report();
        var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
        ddrawer.SetEnableAll(false);


        var cameraPlayer = Camera.main.transform;
        cameraPlayer.SetParent(lastCameraParent);
        cameraPlayer.localPosition = Vector3.zero;
        cameraPlayer.localRotation = Quaternion.identity;
        lastCameraParent = null;
        ScreensManager.SetScreen(null);
        interactionTakesPlace = false;
    }
    public void SetRunningState(bool v) => mAnimator.SetBool("IsRunning", v);
    public void SetTarget(Transform t) => target = t;
    private void Update()
    {
        if (target)
        {
            if ((mAgent.steeringTarget - transform.position) != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(mAgent.steeringTarget - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);

                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            }
        }
    }
    private float CalculateDistance(Vector3 pos)
    {
        NavMeshPath path = new NavMeshPath();
        float dist = float.PositiveInfinity;
        if (mAgent.isOnNavMesh && mAgent.CalculatePath(pos, path))
        {
            dist = 0;
            for (int x = 1; x < path.corners.Length; x++)
                dist += Vector3.Distance(path.corners[x - 1], path.corners[x]);
        }
        return dist;
    }
    private void FixedUpdate()
    {
        if (target)
        {
            mAgent.SetDestination(target.position);
            if (CalculateDistance(target.position) < mAgent.stoppingDistance)
            {
                target = null;
                SetRunningState(false);
                taskDrawer.SetVisible(false);
                if (currentDialog > 2 && dialogs[currentDialog - 2].IsBreakDialog)
                {
                    dialogs[currentDialog - 2] = (dialogs[currentDialog - 2].dt, dialogs[currentDialog - 2].screenText, dialogs[currentDialog - 2].answerText, false);
                }
                Interact();
            }
        }

    }
}

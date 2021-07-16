using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected List<(DialogType dt, string screenText, string answerText)> dialogs;
    protected TaskChecker mtaskChecker;
    protected int currentDialog = 1;
    protected float clipLingth;
    protected abstract string PathToClips();
    protected override void Awake()
    {
        personSource = GetComponent<AudioSource>();
        mtaskChecker = GetComponent<TaskChecker>();
        base.Awake();
    }
    protected virtual void Start()
    {
        if (cameraPlace == null)
            Debug.LogError("Camera place is null!");
    }
    public abstract void Hide();

    public abstract KeyCode HideKey();
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
    public abstract void FinishDialog();
}

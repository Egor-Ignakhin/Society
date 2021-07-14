using UnityEngine;

sealed class TalkingPerson : InteractiveObject, IGameScreen
{
    [SerializeField] private Transform cameraPlace;
    private Transform lastCameraParent;
    private Vector3 neededPosition;
    private Quaternion neededRotation;
    private int personLevel = 1;
    [SerializeField] private string personName = "N/A";
    [SerializeField] private string personRelationAtPlayer = "N/A";

    private void Start()
    {
        if (cameraPlace == null)
            Debug.LogError("Camera place is null!");
    }
    public override void Interact()
    {
        var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
        ddrawer.SetEnableAll(true);
        ddrawer.SetNameAndLevel(personName, personLevel);
        ddrawer.SetRelationAtPlayer(personRelationAtPlayer);

        var cameraPlayer = Camera.main.transform;
        lastCameraParent = cameraPlayer.parent;
        cameraPlayer.SetParent(null);
        neededPosition = cameraPlace.position;
        neededRotation = cameraPlace.rotation;
        ScreensManager.SetScreen(this);
    }
    public void FinishDialog()
    {
        MissionsManager.GetCurrentMission().Report();
        var ddrawer = FindObjectOfType<Dialogs.DialogDrawer>();
        ddrawer.SetEnableAll(false);


        var cameraPlayer = Camera.main.transform;
        cameraPlayer.SetParent(lastCameraParent);
        cameraPlayer.localPosition = Vector3.zero;
        cameraPlayer.localRotation = Quaternion.identity;
        lastCameraParent = null;
        ScreensManager.SetScreen(null);
    }
    public void LateUpdate()
    {
        if (lastCameraParent)
        {
            var cameraPlayer = Camera.main.transform;
            cameraPlayer.position = neededPosition;
            cameraPlayer.rotation = neededRotation;
        }
    }

    public void Hide()
    {
        FinishDialog();
    }

    public KeyCode HideKey()
    {
        return KeyCode.Escape;
    }
}

using UnityEngine;

sealed class TalkingPerson : InteractiveObject
{
    [SerializeField] private Transform cameraPlace;
    private Transform lastCameraParent;
    private Vector3 neededPosition;
    private Quaternion neededRotation;
    private void Start()
    {
        if (cameraPlace == null)
            Debug.LogError("Camera place is null!");
    }
    public override void Interact()
    {
        FindObjectOfType<Dialogs.DialogDrawer>().SetEnableAll(true);

        var cameraPlayer = Camera.main.transform;
        lastCameraParent = cameraPlayer.parent;
        cameraPlayer.SetParent(null);
        neededPosition = cameraPlace.position;
        neededRotation = cameraPlace.rotation;
    }
    public void FinishDialog()
    {
        MissionsManager.GetCurrentMission().Report();
        FindObjectOfType<Dialogs.DialogDrawer>().SetEnableAll(false);

        var cameraPlayer = Camera.main.transform;
        cameraPlayer.SetParent(lastCameraParent);
        cameraPlayer.localPosition = Vector3.zero;
        cameraPlayer.localRotation = Quaternion.identity;
        lastCameraParent = null;
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
}

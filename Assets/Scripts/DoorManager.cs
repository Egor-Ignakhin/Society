using System;
using UnityEngine;

public sealed class DoorManager : MonoBehaviour// класс реализует взаимодействие, а так же движение двери
{
    [SerializeField] private State currentState;
    public bool IsOpen { get; private set; }// открытость двери
    private bool canInteract { get; set; } = true;// возможность взаимодействия

    [SerializeField] private Vector3 openState = new Vector3(0, -90, 0);// открытое состояние
    [SerializeField] private Vector3 lockState = new Vector3(0, 0, 0);// закрытое состояние

    private float lerpRate { get; set; } = 1;// скорость движения двери
    private DoorMesh lastDoorMesh;
    public void Interact(DoorMesh doorMesh)
    {
        if (currentState == State.locked)
            return;
        if (!canInteract)
            return;

        canInteract = !canInteract;
        IsOpen = !IsOpen;
        lastDoorMesh = doorMesh;
        lastDoorMesh.SetType("None");
    }
    public void SetState(State state)
    {
        currentState = state;
    }
    private void FixedUpdate()
    {
        if (IsOpen && !canInteract)
        {
            if (canInteract = Rotate(openState))
                SetDescription();
        }
        else if (!canInteract)
        {
            if (canInteract = Rotate(lockState))
                SetDescription();
        }
    }
    private bool Rotate(Vector3 state)
    {
        Quaternion rotation = Quaternion.Euler(state);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, lerpRate * Time.deltaTime);

        return Math.Round(transform.localRotation.y, 5) == Math.Round(rotation.y, 5);
    }
    private void SetDescription()
    {
        lastDoorMesh.SetType(IsOpen ? InteractiveObject.Types.OpenedDoor : InteractiveObject.Types.ClosedDoor);
    }
}

using UnityEngine;

public abstract class StepPlayer
{
    protected StepSoundData stepSoundData;    
    protected AudioSource stepPlayerSource;
    public StepPlayer() => stepSoundData = StepSoundData.Instance;

    public abstract void OnInit(IMovableController controller);        

    public virtual void OnStep(int physicMaterialIndex, StepSoundData.TypeOfMovement movementType)
    {        
        var key = (movementType, physicMaterialIndex);
        if (stepSoundData.ContainsKey(key))
        {
            if (!stepPlayerSource.isPlaying || (movementType == StepSoundData.TypeOfMovement.JumpLand))
            {
                stepPlayerSource.clip = stepSoundData.GetClipFromIndex(key);
                stepPlayerSource.Play();
            }
        }
    }
}
public interface IMovableController
{

}

using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] PlayerCharacter mPlayerCharacterPrefab;
    [SerializeField] GameplayWidget mGameplayWidgetPrefab;
    [SerializeField] CameraRig mCameraRigPrefab;

    private void Awake()
    {
        PlayerCharacter playerCharacter = Instantiate(mPlayerCharacterPrefab); 
        GameplayWidget gameplayWidget = Instantiate(mGameplayWidgetPrefab);

        CameraRig cameraRig = Instantiate(mCameraRigPrefab);
        playerCharacter.SetCameraRig(cameraRig);

        playerCharacter.SetGameplayWidget(gameplayWidget);
    }
}

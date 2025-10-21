using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] PlayerCharacter mPlayerCharacterPrefab;
    [SerializeField] GameplayWidget mGameplayWidgetPrefab;
    [SerializeField] CameraRig mCameraRigPrefab;
    public PlayerCharacter PlayerCharacter { get; private set; }

    public static GameMode Main;

    private void Awake()
    {
        if(Main != null)
        {
            Destroy(this);
        }
        Main = this;
        PlayerCharacter = Instantiate(mPlayerCharacterPrefab); 
        GameplayWidget gameplayWidget = Instantiate(mGameplayWidgetPrefab);

        CameraRig cameraRig = Instantiate(mCameraRigPrefab);
        PlayerCharacter.SetCameraRig(cameraRig);

        PlayerCharacter.SetGameplayWidget(gameplayWidget);
    }
    void OnDestroy()
    {
        if(Main == this)
        {
            Main = null;
        }
    }
}

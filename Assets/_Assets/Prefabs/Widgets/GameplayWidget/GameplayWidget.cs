using UnityEngine;

public class GameplayWidget : UserWidget
{
    [SerializeField] JoyStick mMoveStick;
    [SerializeField] JoyStick mAimStick;

    public JoyStick MoveStick => mMoveStick;
    public JoyStick AimStick => mAimStick;

    public override void SetOwner(GameObject owner)
    {
        base.SetOwner(owner);

        UserWidget[] childUserWidgets = GetComponentsInChildren<UserWidget>();
        foreach (UserWidget childUserWidget in childUserWidgets)
        {
            if (childUserWidget.gameObject != gameObject)
            {
                childUserWidget.SetOwner(owner);
            }
        }
    }
}

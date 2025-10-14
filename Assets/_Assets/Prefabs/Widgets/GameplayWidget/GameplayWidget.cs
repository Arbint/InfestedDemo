using UnityEngine;

public class GameplayWidget : UserWidget
{
    [SerializeField] JoyStick mMoveStick;
    [SerializeField] JoyStick mAimStick;
    [SerializeField] ChildSwitcher mWidgetSwitcher;
    [SerializeField] GameObject mGameOverWidget;

    public JoyStick MoveStick => mMoveStick;
    public JoyStick AimStick => mAimStick;

    public void SwitchToGameOverState()
    {
        AimStick.enabled = false;
        MoveStick.enabled = false;
        mWidgetSwitcher.SetActiveChild(mGameOverWidget);
    }

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

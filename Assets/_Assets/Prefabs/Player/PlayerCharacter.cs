using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(InventoryComponent))]
[RequireComponent(typeof(HealthComponent))]
public class PlayerCharacter : MonoBehaviour, ITeamInterface, IShakingInterface
{
    [SerializeField] float mRotationLerpRate = 20f;
    [SerializeField] float mAnimTurnSpeedLerpRate = 20f;
    [SerializeField] uint mTeamId = 0;

    Vector2 mMoveInput;
    Vector2 mAimInput;

    CharacterController mCharacterController;

    CameraRig mCameraRig;
    GameplayWidget mGameplayWidget;

    Animator mAnimator;

    float mAnimatorTurnSpeed = 0f;

    public bool Dead => mAttributeSet.Health.CurrentValue == 0;

    public bool mDeathStarted;

    int mFowardSpeedAnimationHash = Animator.StringToHash("forwardSpeed");
    int mRightSpeedAnimationHash = Animator.StringToHash("rightSpeed");
    int mTurnSpeedAnimationHash = Animator.StringToHash("turnSpeed");
    int mSwitchWeaponTriggerHash = Animator.StringToHash("switchWeapon");
    int mIsFiringAnimationHash = Animator.StringToHash("isFiring");
    int mDeadAnimationHash = Animator.StringToHash("dead");

    InventoryComponent mInventoryComponent;

    HealthComponent mHealthComponent;

    AttributeSet mAttributeSet;

    public void SetCameraRig(CameraRig cameraRig)
    {
        mCameraRig = cameraRig;
        mCameraRig.SetFollowTransform(transform);
    }

    private void Awake()
    {
        mCharacterController = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();

        mInventoryComponent = GetComponent<InventoryComponent>();
        mHealthComponent = GetComponent<HealthComponent>();
        mHealthComponent.onHealthEmpty += StartDeath;
        mAttributeSet = GetComponent<AttributeSet>();
    }

    private void StartDeath()
    {
        if (mDeathStarted)
        {
            return;
        }
        mDeathStarted = true;

        mAnimator.SetTrigger(mDeadAnimationHash);
        mGameplayWidget.SwitchToGameOverState();
    }

    private void HandleMoveInput(Vector2 inputValue)
    {
        mMoveInput = inputValue;
    }
    private void HandleAimInput(Vector2 inputValue)
    {
        mAimInput = inputValue;
        if(!Dead)
            mAnimator.SetBool(mIsFiringAnimationHash, inputValue.sqrMagnitude > 0);
    }

    bool ShouldTurn()
    {
        return mMoveInput.magnitude != 0 && mAimInput.sqrMagnitude == 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        if (Dead)
            return;

        Vector3 moveDir = JoystickInputToWorldDir(mMoveInput);
        Vector3 aimDir = JoystickInputToWorldDir(mAimInput);

        if (aimDir.sqrMagnitude == 0)
        {
            aimDir = moveDir;
        }

        mCharacterController.Move(moveDir * Time.deltaTime * mAttributeSet.MoveSpeed.CurrentValue + Vector3.down * Time.deltaTime * Physics.gravity.magnitude);

        float currentTurnSpeed = 0f;
        if (aimDir.sqrMagnitude != 0)
        {
            Quaternion prevRot = transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                              Quaternion.LookRotation(aimDir, Vector3.up),
                                              Time.deltaTime * mRotationLerpRate
                                              );

            currentTurnSpeed = (transform.rotation.eulerAngles.y - prevRot.eulerAngles.y) / Time.deltaTime;
        }

        mAnimatorTurnSpeed = Mathf.Lerp(mAnimatorTurnSpeed, currentTurnSpeed, mAnimTurnSpeedLerpRate * Time.deltaTime);
        mAnimator.SetFloat(mTurnSpeedAnimationHash, mAnimatorTurnSpeed);

        mCameraRig.SetTurnInput(ShouldTurn() ? mMoveInput.x : 0);

        float forwardSpeed = Vector3.Dot(moveDir, transform.forward);
        float rightSpeed = Vector3.Dot(moveDir, transform.right);

        mAnimator.SetFloat(mFowardSpeedAnimationHash, forwardSpeed);
        mAnimator.SetFloat(mRightSpeedAnimationHash, rightSpeed);
    }

    Vector3 JoystickInputToWorldDir(Vector2 inputValue)
    {
        Vector3 viewRight = Camera.main.transform.right;
        Vector3 viewUp = Vector3.Cross(viewRight, Vector3.up);

        return viewRight * inputValue.x + viewUp * inputValue.y;
    }

    internal void SetGameplayWidget(GameplayWidget gameplayWidget)
    {
        mGameplayWidget = gameplayWidget;
        mGameplayWidget.SetOwner(gameObject);
        mGameplayWidget.MoveStick.onInputValueChanged += HandleMoveInput;
        mGameplayWidget.AimStick.onInputValueChanged += HandleAimInput;
        mGameplayWidget.AimStick.onTapped += StartSwitchingWeapon;
    }

    private void StartSwitchingWeapon()
    {
        mAnimator.SetTrigger(mSwitchWeaponTriggerHash);
    }

    public void WeaponSwitchPoint()
    {
        mInventoryComponent.SwitchToNextWeapon();
    }

    public void DamagePoint()
    {
        mInventoryComponent.FireCurrentWeapon(); 
    }

    public uint GetTeamId()
    {
        return mTeamId;
    }

    public void StartShake()
    {
        mCameraRig.StartShake();
    }
}

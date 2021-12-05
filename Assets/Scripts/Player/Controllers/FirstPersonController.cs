using Society.Effects;
using Society.GameScreens;
using Society.Settings;

using UnityEngine;

namespace Society.Player.Controllers
{
    [RequireComponent(typeof(CapsuleCollider)), RequireComponent(typeof(Rigidbody)), AddComponentMenu("First Person Controller")]

    public sealed class FirstPersonController : MonoBehaviour, IMovableController
    {
        #region Variables    
        #region Look Settings
        private float VerticalRotationRange = 0f;
        private readonly float HeadMaxY = 90;
        private readonly float HeadMinY = -90;

        public float SensivityM { get; set; } = 1;
        private float slidingH;
        private float slidingV;
        public float AdditionalXMouse { get; set; } = 0;
        private readonly float CameraSmoothing = 5f;

        private Camera PlayerCamera;
        private Transform PlayerCameraTr;

        private Vector3 targetAngles;
        private Vector3 followAngles;
        private Vector3 followVelocity;

        private PlayerSoundsCalculator playerSoundsCalculator;
        #endregion

        #region Movement Settings    
        private bool Sprint = false;
        private bool CanSprinting = true;
        public void SetPossibleSprinting(bool v) => CanSprinting = v;
        private readonly float WalkSpeed = 4f;
        private float RecumbentingSpeed;

        private float SprintSpeed = 8f;
        private readonly float JumpPower = 5f;
        private bool Jump;
        private bool didJump;

        private float speed;
        private float WalkSpeedInternal;
        private float SprintSpeedInternal;
        private float JumpPowerInternal;
        private float RecumbentingSpeedInternal;
        public delegate void StepHandler(int matIndex, StepSoundData.TypeOfMovement type);
        public event StepHandler PlayerStepEvent;

        [System.Serializable]
        public sealed class CrouchModifiers
        {
            public bool toggleCrouch = false;
            public float crouchWalkSpeedMultiplier = 0.5f;
            public float crouchJumpPowerMultiplier = 0f;
        }

        public sealed class RecumbentModifiers
        {
            public float RecumbenthWalkSpeedMultiplier { get; set; } = 0.25f;
            public float RecumbentJumpPowerMultiplier { get; set; } = 0f;
        }
        public CrouchModifiers MCrouchModifiers { get; set; } = new CrouchModifiers();
        public RecumbentModifiers MRecumbentModifiers { get; set; } = new RecumbentModifiers();
        private StepFpc stepPlayer;
        private bool PossibleJump;

        internal Camera GetCamera() => PlayerCamera;

        private StepSoundData stepSoundData;

        [System.Serializable]
        public sealed class AdvancedSettings
        {
            public float GravityMultiplier { get; set; } = 2f;

            public PhysicMaterial ZeroFrictionMaterial { get; set; }

            public PhysicMaterial HighFrictionMaterial { get; set; }

            public float MaxSlopeAngle { get; set; } = 90f;
            internal bool IsTouchingWalkable { get; set; }
            internal bool IsTouchingUpright { get; set; }
            internal bool IsTouchingFlat { get; set; }
            public float MaxWallShear { get; set; } = 89f;
            public float MaxStepHeight { get; set; } = 0.2f;
            internal bool stairMiniHop = false;
            public Vector3 CurntGroundNormal { get; set; }
            public float LastKnownSlopeAngle { get; set; }
            public float FOVKickAmount { get; set; } = 2.5f;
            public float fovRef;

            public float ColliderRadius { get; set; }
            public float ColliderHeight { get; set; }
            public float ChangeTime = 1;
        }

        internal void SetPossibleJump(bool v) => PossibleJump = v;

        public AdvancedSettings Advanced { get; set; } = new AdvancedSettings();
        private CapsuleCollider playerCollider;
        private bool IsGrounded = true;
        private Vector2 inputXY;
        private bool isCrouching = false;
        private bool isRecumbenting = false;
        private float yVelocity;

        private Rigidbody fpcRigidbody;
        public CapsuleCollider GetCollider() => playerCollider;
        private Vector3 oldPos = Vector3.zero;
        private int CurrentPhysicMaterialIndex;
        private bool wasGrounded = false;
        public bool StepEventIsEnabled { get; set; } = true;
        #endregion

        #endregion

        private void Awake()
        {
            #region Movement Settings - Awake        
            PlayerCamera = Camera.main;
            PlayerCameraTr = PlayerCamera.transform;
            JumpPowerInternal = JumpPower;
            playerCollider = GetComponent<CapsuleCollider>();
            fpcRigidbody = GetComponent<Rigidbody>();
            fpcRigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
            fpcRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            followAngles = targetAngles = transform.localEulerAngles;
            #endregion
        }
        private void OnEnable()
        {
            SprintSpeed = WalkSpeed * 1.5f;
            WalkSpeedInternal = WalkSpeed;
            SprintSpeedInternal = SprintSpeed;
            RecumbentingSpeed = WalkSpeed * 0.25f;
            RecumbentingSpeedInternal = RecumbentingSpeed;
        }

        private void Start()
        {
            #region Look Settings - Start
            VerticalRotationRange = 2 * HeadMaxY + Mathf.Clamp(0, HeadMinY, 0);
            #endregion

            #region Movement Settings - Start  
            playerCollider.radius = playerCollider.height / 4;
            Advanced.ColliderHeight = playerCollider.height;
            Advanced.ColliderRadius = playerCollider.radius;
            Advanced.ZeroFrictionMaterial = new PhysicMaterial("Zero_Friction")
            {
                dynamicFriction = 0,
                staticFriction = 0,
                frictionCombine = PhysicMaterialCombine.Minimum,
                bounceCombine = PhysicMaterialCombine.Minimum
            };

            Advanced.HighFrictionMaterial = new PhysicMaterial("Max_Friction")
            {
                dynamicFriction = 1,
                staticFriction = 1,
                frictionCombine = PhysicMaterialCombine.Maximum,
                bounceCombine = PhysicMaterialCombine.Average
            };
            playerSoundsCalculator = FindObjectOfType<PlayerSoundsCalculator>();
            #endregion
            stepSoundData = FindObjectOfType<StepSoundData>();
            stepPlayer = new StepFpc(this, stepSoundData);
        }
        private void Update()
        {
            #region Look Settings - Update

            if (!ScreensManager.HasActiveScreen())
            {
                float mouseYInput = Input.GetAxis("Mouse Y");
                float mouseXInput = Input.GetAxis("Mouse X") + AdditionalXMouse;

                if (targetAngles.y > 180) { targetAngles.y -= 360; followAngles.y -= 360; } else if (targetAngles.y < -180) { targetAngles.y += 360; followAngles.y += 360; }
                if (targetAngles.x > 180) { targetAngles.x -= 360; followAngles.x -= 360; } else if (targetAngles.x < -180) { targetAngles.x += 360; followAngles.x += 360; }

                targetAngles.y += mouseXInput * (float)GameSettings.GetMouseSensivity() * SensivityM;//rotate camera

                targetAngles.x += mouseYInput * (float)GameSettings.GetMouseSensivity() * SensivityM;

                targetAngles.x = Mathf.Clamp(targetAngles.x, -0.5f * VerticalRotationRange, 0.5f * VerticalRotationRange);
                followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, CameraSmoothing / 100);

                PlayerCameraTr.localRotation = Quaternion.Euler(-followAngles.x, 0, ZSlant);
                transform.localRotation = Quaternion.Euler(0, followAngles.y, 0);
            }
            #endregion

            #region Input Settings - Update
            if (PossibleJump && Input.GetKeyDown(GameSettings.GetJumpKeyCode()) && !ScreensManager.HasActiveScreen())
                Jump = true;
            else if (Input.GetKeyUp(GameSettings.GetJumpKeyCode()))
                Jump = false;


            if (!ScreensManager.HasActiveScreen())
            {
                isCrouching = Input.GetKey(GameSettings.GetCrouchKeyCode()) && !isRecumbenting;

                if (Input.GetKeyDown(GameSettings.GetProneKeyCode()) && !isCrouching)
                {
                    isRecumbenting = !isRecumbenting;
                }
            }

            Sprint = (Input.GetKey(GameSettings.GetSprintKeyCode())) && (!ScreensManager.HasActiveScreen()) && (CanSprinting);
            BasicNeeds.Instance.EnableFoodAndWaterMultiply(Sprint);

            #endregion
            SetPossibleJump(true);
        }

        private void FixedUpdate()
        {
            #region Movement Settings - FixedUpdate        

            Vector3 MoveDirection = Vector3.zero;
            speed = Sprint ? isCrouching ? WalkSpeedInternal : SprintSpeedInternal : WalkSpeedInternal;
            if (isRecumbenting)
                speed = RecumbentingSpeedInternal;
            speed *= additionalBraking;

            if (Advanced.MaxSlopeAngle > 0)
            {
                if (Advanced.IsTouchingUpright && Advanced.IsTouchingWalkable)
                {
                    MoveDirection = transform.forward * inputXY.y * speed + transform.right * inputXY.x * WalkSpeedInternal;
                    if (!didJump)
                        fpcRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                }
                else if (Advanced.IsTouchingUpright && !Advanced.IsTouchingWalkable)
                {
                    fpcRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                }

                else
                {
                    fpcRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                    MoveDirection = (transform.forward * inputXY.y * speed + transform.right * inputXY.x * WalkSpeedInternal) * (fpcRigidbody.velocity.y > 0.01f ? SlopeCheck() : 0.8f);
                }
            }
            else
                MoveDirection = transform.forward * inputXY.y * speed + transform.right * inputXY.x * WalkSpeedInternal;

            #region step logic

            if (Advanced.MaxStepHeight > 0 && Physics.Raycast(transform.position - new Vector3(0, ((playerCollider.height / 2) * transform.localScale.y) - 0.01f, 0), MoveDirection, out RaycastHit WT, playerCollider.radius - 3, Physics.AllLayers, QueryTriggerInteraction.Ignore) && Vector3.Angle(WT.normal, Vector3.up) > 88)
            {
                if (!Physics.Raycast(transform.position - new Vector3(0, (playerCollider.height / 2 * transform.localScale.y) - Advanced.MaxStepHeight, 0), MoveDirection, out _, playerCollider.radius + 0.25f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    Advanced.stairMiniHop = true;

                    fpcRigidbody.position += new Vector3(0, Advanced.MaxStepHeight * 1.2f, 0);
                }
            }
            #endregion
            float m = ScreensManager.HasActiveScreen() ? 0 : 1;


            float horizontalInput = 0;
            float verticalInput = 0;

            if (Input.GetKey(GameSettings.GetMoveFrontKeyCode()))
                verticalInput = 1;
            if (Input.GetKey(GameSettings.GetMoveBackKeyCode()))
                verticalInput = -1;
            if (Input.GetKey(GameSettings.GetMoveLeftKeyCode()))
                horizontalInput = -1;
            if (Input.GetKey(GameSettings.GetMoveRightKeyCode()))
                horizontalInput = 1;

            //Важная хрень, сделана, чтобы можно было менять клавиши ходьбы и оставалось сглаживание как при GetAxis
            var smoothedInput = SmoothInput(horizontalInput, verticalInput);

            horizontalInput = smoothedInput.x;
            verticalInput = smoothedInput.y;

            inputXY = new Vector2(horizontalInput, verticalInput) * m * additionalBraking;
            if (inputXY.magnitude > 1)
                inputXY.Normalize();

            #region Jump
            yVelocity = fpcRigidbody.velocity.y;

            if (IsGrounded && Jump && JumpPowerInternal > 0 && !didJump)
            {
                if (Advanced.MaxSlopeAngle > 0)
                {
                    if (Advanced.IsTouchingFlat || Advanced.IsTouchingWalkable)
                    {
                        didJump = true;
                        Jump = false;
                        yVelocity += fpcRigidbody.velocity.y < 0.01f ? JumpPowerInternal : JumpPowerInternal / 3;
                        Advanced.IsTouchingWalkable = false;
                        Advanced.IsTouchingFlat = false;
                        Advanced.IsTouchingUpright = false;
                        fpcRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                    }

                }
                else
                {
                    didJump = true;
                    Jump = false;
                    yVelocity += JumpPowerInternal;
                }

            }

            if (Advanced.MaxSlopeAngle > 0)
            {
                if (!didJump && Advanced.LastKnownSlopeAngle > 5 && Advanced.IsTouchingWalkable)
                {
                    yVelocity *= SlopeCheck() / 4;
                }
            }

            #endregion

            Vector3 newVel = (MoveDirection + (Vector3.up * yVelocity));
            if (newVel.y > 0)
                newVel *= additionalBraking;
            fpcRigidbody.velocity = newVel;
            playerCollider.sharedMaterial = inputXY.magnitude > 0 || !IsGrounded ? Advanced.ZeroFrictionMaterial : Advanced.HighFrictionMaterial;

            fpcRigidbody.AddForce(Physics.gravity * (Advanced.GravityMultiplier - 1) * fpcRigidbody.mass);

            float capsuleHeightFollowing, capsuleRadiusFollowing;
            if (isCrouching)
            {
                capsuleHeightFollowing = Advanced.ColliderHeight / 1.5f;
                capsuleRadiusFollowing = Advanced.ColliderRadius;

                WalkSpeedInternal = WalkSpeed * MCrouchModifiers.crouchWalkSpeedMultiplier;
                JumpPowerInternal = JumpPower * MCrouchModifiers.crouchJumpPowerMultiplier;
            }
            else if (isRecumbenting)
            {
                capsuleHeightFollowing = Advanced.ColliderHeight / 5;
                capsuleRadiusFollowing = Advanced.ColliderRadius / 5;
                WalkSpeedInternal = WalkSpeed * MRecumbentModifiers.RecumbenthWalkSpeedMultiplier;
                JumpPowerInternal = JumpPower * MRecumbentModifiers.RecumbentJumpPowerMultiplier;
            }
            else
            {
                capsuleHeightFollowing = Advanced.ColliderHeight;
                capsuleRadiusFollowing = Advanced.ColliderRadius;

                WalkSpeedInternal = WalkSpeed;
                SprintSpeedInternal = SprintSpeed;
                JumpPowerInternal = JumpPower;
            }
            playerCollider.height = Mathf.MoveTowards(playerCollider.height, capsuleHeightFollowing, 5 * Time.deltaTime * additionalBraking);
            playerCollider.radius = Mathf.MoveTowards(playerCollider.radius, capsuleRadiusFollowing, 5 * Time.deltaTime * additionalBraking);
            #endregion
            #region  Reset Checks

            if (Advanced.MaxSlopeAngle > 0)
            {
                if (Advanced.IsTouchingFlat || Advanced.IsTouchingWalkable || Advanced.IsTouchingUpright)
                    didJump = false;
                Advanced.IsTouchingWalkable = false;
                Advanced.IsTouchingUpright = false;
                Advanced.IsTouchingFlat = false;
            }
            #endregion
            SetPhysMaterial();
            playerSoundsCalculator.SetPlayerSpeed(Mathf.Abs(Vector3.Distance(transform.position, oldPos)) * 100);
            CallStepEvent();
            wasGrounded = IsGrounded;
            IsGrounded = false;
        }

        private Vector2 SmoothInput(float targetH, float targetV)
        {
            float sensitivity = 3;
            float deadZone = 0.001f;

            slidingH = Mathf.MoveTowards(slidingH,
                          targetH, sensitivity * Time.deltaTime);


            slidingV = Mathf.MoveTowards(slidingV,
                          targetV, sensitivity * Time.deltaTime);

            return new Vector2(
                   (Mathf.Abs(slidingH) < deadZone) ? 0f : slidingH,
                   (Mathf.Abs(slidingV) < deadZone) ? 0f : slidingV);
        }
        private void SetPhysMaterial()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit))
                CurrentPhysicMaterialIndex = stepSoundData.GetIndexFromRayCast(hit, transform.position);
            else
                CurrentPhysicMaterialIndex = 0;
        }
        private void CallStepEvent()
        {
            Vector2 to = new Vector2(transform.position.x, transform.position.z);
            Vector2 from = new Vector2(oldPos.x, oldPos.z);
            StepSoundData.TypeOfMovement type = StepSoundData.TypeOfMovement.None;
            if (!wasGrounded && IsGrounded)
                type = StepSoundData.TypeOfMovement.JumpLand;
            else if (Mathf.Abs(Vector2.Distance(to, from)) > Time.fixedDeltaTime * 2)
            {
                if (Sprint) type = StepSoundData.TypeOfMovement.Run;
                else type = StepSoundData.TypeOfMovement.Walk;
            }
            PlayerStepEvent?.Invoke(CurrentPhysicMaterialIndex, type);
            oldPos = transform.position;
        }

        private float SlopeCheck()
        {
            Advanced.LastKnownSlopeAngle = Mathf.MoveTowards(Advanced.LastKnownSlopeAngle, Vector3.Angle(Advanced.CurntGroundNormal, Vector3.up), 5f);

            return new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(Advanced.MaxSlopeAngle + 15, 0f), new Keyframe(Advanced.MaxWallShear, 0.0f), new Keyframe(Advanced.MaxWallShear + 0.1f, 1.0f), new Keyframe(90, 1.0f)) { preWrapMode = WrapMode.Clamp, postWrapMode = WrapMode.ClampForever }.Evaluate(Advanced.LastKnownSlopeAngle);
        }

        private void OnCollisionEnter(Collision CollisionData)
        {
            for (int i = 0; i < CollisionData.contactCount; i++)
            {
                ContactPoint currentCp = CollisionData.GetContact(i);
                float a = Vector3.Angle(currentCp.normal, Vector3.up);

                if (currentCp.point.y < transform.position.y - ((playerCollider.height / 2) - playerCollider.radius * 0.95f))
                {
                    if (!IsGrounded)
                    {
                        IsGrounded = true;
                        Advanced.stairMiniHop = false;
                        if (didJump && a <= 70)
                            didJump = false;
                    }

                    if (Advanced.MaxSlopeAngle > 0)
                    {
                        if (a < 5.1f)
                        {
                            Advanced.IsTouchingFlat = true;
                            Advanced.IsTouchingWalkable = true;
                        }
                        else if (a < Advanced.MaxSlopeAngle + 0.1f)
                            Advanced.IsTouchingWalkable = true;
                        else if (a < 90)
                            Advanced.IsTouchingUpright = true;

                        Advanced.CurntGroundNormal = currentCp.normal;
                    }
                }
            }
        }
        private void OnCollisionStay(Collision CollisionData)
        {
            for (int i = 0; i < CollisionData.contactCount; i++)
            {
                ContactPoint currentCp = CollisionData.GetContact(i);
                float a = Vector3.Angle(currentCp.normal, Vector3.up);

                if (currentCp.point.y < transform.position.y - ((playerCollider.height / 2) - playerCollider.radius * 0.95f))
                {
                    if (!IsGrounded)
                    {
                        IsGrounded = true;
                        Advanced.stairMiniHop = false;
                    }

                    if (Advanced.MaxSlopeAngle > 0)
                    {
                        if (a < 5.1f)
                        {
                            Advanced.IsTouchingFlat = true;
                            Advanced.IsTouchingWalkable = true;
                        }
                        else if (a < Advanced.MaxSlopeAngle + 0.1f)
                            Advanced.IsTouchingWalkable = true;
                        else if (a < 90)
                            Advanced.IsTouchingUpright = true;

                        Advanced.CurntGroundNormal = currentCp.normal;
                    }
                }
            }
        }
        private void OnCollisionExit(Collision CollisionData)
        {
            IsGrounded = false;
            if (Advanced.MaxSlopeAngle > 0)
            {
                Advanced.CurntGroundNormal = Vector3.up;
                Advanced.LastKnownSlopeAngle = 0;
                Advanced.IsTouchingWalkable = false;
                Advanced.IsTouchingUpright = false;
            }
        }

        public void SetPosAndRot(Transform point)
        {
            transform.position = point.position;
            transform.rotation = point.rotation;

            followAngles = targetAngles = point.eulerAngles;
        }

        public void Rocking(Vector3 v)
        {
            followAngles += v;
            targetAngles += v;
        }
        private float ZSlant { get; set; }

        public void SetZSlant(int n)
        {
            ZSlant = n;
            float rPos = ZSlant > 0 ? -0.375f : (ZSlant < 0 ? 0.375f : 0);

            PlayerCameraTr.localPosition = Vector3.MoveTowards(PlayerCameraTr.localPosition, new Vector3(rPos, 0, 0), Time.fixedDeltaTime * 2);
        }

        private float additionalBraking = 1;//добавляемая скорость при перегрузе
        public void SetBraking(float b) => additionalBraking = b;
        public class StepFpc : StepPlayer
        {
            private readonly FirstPersonController fpc;
            private readonly IMovableController fpcController;
            private readonly float sourceVolume = 0.25f;
            public StepFpc(IMovableController firstPersonController, StepSoundData ssd)
            {
                fpcController = firstPersonController;
                stepSoundData = ssd;
                fpc = (FirstPersonController)firstPersonController;
                fpc.PlayerStepEvent += OnStep;

                stepPlayerSource = fpc.gameObject.AddComponent<AudioSource>();
                stepPlayerSource.priority = 126;

                GameSettings.UpdateSettingsEvent += OnSettingsUpdate;
            }

            public override void OnStep(int physicMaterialIndex, StepSoundData.TypeOfMovement movementType)
            {
                if (!fpc.IsGrounded)
                    return;

                if (!fpcController.StepEventIsEnabled)
                    return;

                base.OnStep(physicMaterialIndex, movementType);
            }

            private void OnSettingsUpdate()
            {
                stepPlayerSource.volume = (float)(sourceVolume * Settings.GameSettings.GetGeneralVolume());
            }

            public void OnDestroy()
            {
                fpc.PlayerStepEvent -= OnStep;
                GameSettings.UpdateSettingsEvent -= OnSettingsUpdate;
            }
        }
        public float GetPlayerColliderHeight() => playerCollider.height;
        internal Transform GetCameraHost() => transform.GetChild(0);
        internal void ResetRbVelocity()
        {
            fpcRigidbody.velocity = Vector3.zero;
            fpcRigidbody.angularVelocity = Vector3.zero;
            oldPos = transform.position;
        }
        internal void SetPosition(Vector3 position)
        {
            fpcRigidbody.position = position;
            oldPos = position;
        }
        private void OnDestroy() => stepPlayer.OnDestroy();
    }
}
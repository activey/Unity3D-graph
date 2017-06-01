using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Elenesski.Camera.Utilities {

    public class GenericMoveCamera : MonoBehaviour {

        private Movement _Forward;
        private Movement _PanX;
        private Movement _RotateX;
        private Movement _PanY;
        private Movement _RotateY;
        private float _Resolution = 1f;

        [Header("Operational")]
        public bool Operational = true;

        [Header("Input Method")]
        public GenericMoveCameraInputs GetInputs = null;

        [Header("Camera")]
        public bool LevelCamera = true;
        public bool ForwardMovementLockEnabled = true;

        [Header("Movement Speed")]
        public float MovementSpeedMagnification = 1f;
        public float WheelMouseMagnification = 5f;
        public float ShiftKeyMagnification = 2f;
        public float ControlKeyMagnification = 0.25f;
        public float RotationMagnification = 1f;

        [Header("Pan Speed Modifications")]
        public float PanLeftRightSensitivity = 1f;
        public float PanUpDownSensitivity = 1f;

        [Header("Mouse Rotation Sensitivity")]
        public float MouseRotationSensitivity = 0.5f;

        [Header("Dampening")]
        public float ForwardDampenRate = 0.99f;
        public float PanningDampenRate = 0.95f;
        public float RotateDampenRate = 0.99f;

        [Header("Look At")]
        public GameObject LookAtTarget = null;
        public float MinimumZoom = 20f;
        public float MaximumZoom = 80f;

        [Header("Movement Limits - X")]
        public bool LockX = false;
        public bool UseXRange;
        public float XRangeMin;
        public float XRangeMax;

        [Header("Movement Limits - Y")]
        public bool LockY = false;
        public bool UseYRange;
        public float YRangeMin;
        public float YRangeMax;

        [Header("Movement Limits - Z")]
        public bool LockZ = false;
        public bool UseZRange;
        public float ZRangeMin;
        public float ZRangeMax;

        // Rotation when in Awake(), to prevent weird rotations later
        private Vector3 _DefaultRotation;

        private class Movement {
            private readonly Action<float> _Action;
            private readonly Func<float> _DampenRate;
            private float _Velocity;
            private float _Dampen;

            public Movement(Action<float> aAction, Func<float> aDampenRate) {
                _Action = aAction;
                _DampenRate = aDampenRate;
                _Velocity = 0f;
                _Dampen = 0;
            }

            public void ChangeVelocity(float aAmount) {
                _Velocity += aAmount;
                _Dampen = _DampenRate();
            }

            public void SetVelocity(float aAmount) {
                _Velocity = aAmount;
                _Dampen = _DampenRate();
            }

            public void Update(bool aDampen = true) {
                if (_Dampen > 0)
                    if (_Velocity >= -0.001f && _Velocity <= 0.001f) {
                        _Dampen = 0;
                        _Velocity = 0;
                    } else {
                        if (aDampen)
                            _Velocity *= _Dampen;

                        _Action(_Velocity);
                    }
            }
        }

        public void SetResolution(float aResolution) {
            _Resolution = aResolution;
        }

        public void Awake() {
            if ( GetInputs == null )
                GetInputs = new GenericMoveCameraInputs();

            _DefaultRotation = gameObject.transform.localRotation.eulerAngles;

            GetInputs.Initialize();
        }

        public void Start() {
            if (LookAtTarget == null) {
                _Forward = new Movement(aAmount => gameObject.transform.Translate(Vector3.forward*aAmount), () => ForwardDampenRate);
            } else {
                _Forward = new Movement(aAmount => gameObject.GetComponent<UnityEngine.Camera>().fieldOfView += aAmount, () => ForwardDampenRate);
            }

            _PanX = new Movement(aAmount => gameObject.transform.Translate(Vector3.left*aAmount), () => PanningDampenRate);
            _PanY = new Movement(aAmount => gameObject.transform.Translate(Vector3.up*aAmount), () => PanningDampenRate);

            _RotateX = new Movement(aAmount => gameObject.transform.Rotate(Vector3.up*aAmount), () => RotateDampenRate);
            _RotateY = new Movement(aAmount => gameObject.transform.Rotate(Vector3.left*aAmount), () => RotateDampenRate);

        }

        public void Update() {

            if (!Operational)
                return;

            GetInputs.QueryInputSystem();

            Vector3 START_POSITION = gameObject.transform.position;

            if (GetInputs.ResetMovement) {
                ResetMovement();
            } else {

                float MAG = (GetInputs.isSlowModifier ? ControlKeyMagnification : 1f)*(GetInputs.isFastModifier ? ShiftKeyMagnification : 1f);

                if (GetInputs.isPanLeft) {
                    _PanX.ChangeVelocity(0.01f*MAG*_Resolution*PanLeftRightSensitivity);
                } else if (GetInputs.isPanRight) {
                    _PanX.ChangeVelocity(-0.01f*MAG*_Resolution*PanLeftRightSensitivity);
                }

                if ( _PanX != null )
                    _PanX.Update();

                if (GetInputs.isMoveForward ) {
                    _Forward.ChangeVelocity(0.005f*MAG*_Resolution*MovementSpeedMagnification);
                } else if (GetInputs.isMoveBackward ) {
                    _Forward.ChangeVelocity(-0.005f*MAG*_Resolution*MovementSpeedMagnification);
                }

                if (GetInputs.isMoveForwardAlt) {
                    _Forward.ChangeVelocity(0.005f*MAG*_Resolution*MovementSpeedMagnification*WheelMouseMagnification);
                } else if (GetInputs.isMoveBackwardAlt) {
                    _Forward.ChangeVelocity(-0.005f*MAG*_Resolution*MovementSpeedMagnification*WheelMouseMagnification);
                }

                if (GetInputs.isPanUp) {
                    _PanY.ChangeVelocity(0.005f*MAG*_Resolution*PanUpDownSensitivity);
                } else if (GetInputs.isPanDown) {
                    _PanY.ChangeVelocity(-0.005f*MAG*_Resolution*PanUpDownSensitivity);
                }

                bool FORWARD_LOCK = GetInputs.isLockForwardMovement && ForwardMovementLockEnabled;
                _Forward.Update(!FORWARD_LOCK);

                _PanY.Update();

                // Pan
                if (GetInputs.isRotateAction) {

                    float X = (Input.mousePosition.x - GetInputs.RotateActionStart.x)/Screen.width*MouseRotationSensitivity;
                    float Y = (Input.mousePosition.y - GetInputs.RotateActionStart.y)/Screen.height*MouseRotationSensitivity;

                    _RotateX.SetVelocity(X*MAG*RotationMagnification*_Resolution);
                    _RotateY.SetVelocity(Y*MAG*RotationMagnification*_Resolution);

                }

                _RotateX.Update();
                _RotateY.Update();
            }


            // Lock at object
            if (LookAtTarget != null ) {
                transform.LookAt(LookAtTarget.transform);
                if (gameObject.GetComponent<UnityEngine.Camera>().fieldOfView < MinimumZoom) {
                    ResetMovement();
                    gameObject.GetComponent<UnityEngine.Camera>().fieldOfView = MinimumZoom;
                } else if (gameObject.GetComponent<UnityEngine.Camera>().fieldOfView > MaximumZoom) {
                    ResetMovement();
                    gameObject.GetComponent<UnityEngine.Camera>().fieldOfView = MaximumZoom;
                }
            }

            // Set ranges
            Vector3 END_POSITION = transform.position;

            if (LockX)
                END_POSITION.x = START_POSITION.x;
            if (LockY)
                END_POSITION.y = START_POSITION.y;
            if (LockZ)
                END_POSITION.z = START_POSITION.z;

            if (UseXRange && gameObject.transform.position.x < XRangeMin) END_POSITION.x = XRangeMin;
            if (UseXRange && gameObject.transform.position.x > XRangeMax) END_POSITION.x = XRangeMax;

            if (UseYRange && gameObject.transform.position.y < YRangeMin) END_POSITION.y = YRangeMin;
            if (UseYRange && gameObject.transform.position.y > YRangeMax) END_POSITION.y = YRangeMax;

            if (UseZRange && gameObject.transform.position.z < ZRangeMin) END_POSITION.z = ZRangeMin;
            if (UseZRange && gameObject.transform.position.z > ZRangeMax) END_POSITION.z = ZRangeMax;

            transform.position = END_POSITION;

            // Level Camera
            if (LevelCamera)
                LevelTheCamera();

        }

        public void ResetMovement() {
            _PanX.SetVelocity(0);
            _PanY.SetVelocity(0);
            _Forward.SetVelocity(0);
            _RotateX.SetVelocity(0);
            _RotateY.SetVelocity(0);

            _PanX.Update();
            _PanY.Update();
            _Forward.Update();
            _RotateX.Update();
            _RotateY.Update();
        }

        public void OnCollisionEnter(Collision collision) {
            ResetMovement();
        }

        public void PanY( float aMagnitude ) {
            _PanY.ChangeVelocity(0.005f*aMagnitude*_Resolution*PanUpDownSensitivity);
        }

        public void PanX(float aMagnitude) {
            _PanX.ChangeVelocity(-0.01f*aMagnitude*_Resolution*PanLeftRightSensitivity);
        }

        public void ForwardBack( float aMagnitude ) {
            _Forward.ChangeVelocity(-0.005f*aMagnitude*_Resolution*MovementSpeedMagnification);
        }

        public void LevelTheCamera() {
            transform.rotation = Quaternion.LookRotation(transform.forward.normalized, Vector3.up);
        }

    }

}
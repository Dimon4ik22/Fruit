//using System;
//using HyperCasualPack.ScriptableObjects;
//using HyperCasualPack.ScriptableObjects.Channels;
//using UnityEngine;

//namespace HyperCasualPack
//{
//    public enum ArcadeIdleMoverType { PLAYER, EMPLOYEE }
//    public class ArcadeIdleMover : MonoBehaviour
//    {
//        [SerializeField] Rigidbody _rbd;
//        [SerializeField] MovementDataSO _movementDataSO;
//        [SerializeField] InputChannelSO _inputChannelSo;
//        public ArcadeIdleMoverType ArcadeIdleMoverType;
//        Vector3 _currentInputVector;

//        public bool IsStopped => _currentInputVector.sqrMagnitude < 0.3f;

//        void FixedUpdate()
//        {
//            switch (ArcadeIdleMoverType)
//            {
//                case ArcadeIdleMoverType.PLAYER:
//                    _rbd.velocity = _currentInputVector;
//                    break;
//                case ArcadeIdleMoverType.EMPLOYEE:
//                    break;
//            }
//        }

//        void OnEnable()
//        {
//            _inputChannelSo.JoystickUpdate += OnJoystickUpdate;
//        }

//        void OnDisable()
//        {
//            _inputChannelSo.JoystickUpdate -= OnJoystickUpdate;
//        }

//        void OnJoystickUpdate(Vector2 newMoveDirection)
//        {
//            switch (ArcadeIdleMoverType)
//            {
//                case ArcadeIdleMoverType.PLAYER:
//                    if (newMoveDirection.magnitude >= 1f)
//                    {
//                        newMoveDirection.Normalize();
//                    }

//                    _currentInputVector = new Vector3(newMoveDirection.x * _movementDataSO.SideMoveSpeed, _rbd.velocity.y, newMoveDirection.y * _movementDataSO.ForwardMoveSpeed);

//                    Vector3 lookDir = new Vector3(_currentInputVector.x, 0f, _currentInputVector.z);
//                    if (lookDir != Vector3.zero)
//                    {
//                        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
//                    }
//                    break;
//                case ArcadeIdleMoverType.EMPLOYEE:
//                    break;
//            }

//        }
//    }
//}

using HyperCasualPack.ScriptableObjects.Channels;
using HyperCasualPack.ScriptableObjects;
using System;
using UnityEngine;

namespace HyperCasualPack
{
    public enum ArcadeIdleMoverType { PLAYER, EMPLOYEE }

    public class ArcadeIdleMover : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rbd;
        [SerializeField] private MovementDataSO _movementDataSO;
        [SerializeField] private InputChannelSO _inputChannelSo;

        public ArcadeIdleMoverType ArcadeIdleMoverType;

        private Vector3 _currentInputVector;
        private bool _isJoystickActive = false;

        public bool IsStopped => _currentInputVector.sqrMagnitude < 0.3f;

        void FixedUpdate()
        {
            if (ArcadeIdleMoverType == ArcadeIdleMoverType.PLAYER && !_isJoystickActive)
            {
                _rbd.velocity = _currentInputVector;
            }
        }

        void OnEnable()
        {
            _inputChannelSo.JoystickUpdate += OnJoystickUpdate;
        }

        void OnDisable()
        {
            _inputChannelSo.JoystickUpdate -= OnJoystickUpdate;
        }

        void OnJoystickUpdate(Vector2 newMoveDirection)
        {
            _isJoystickActive = true; // Активируем флаг при использовании джойстика

            if (ArcadeIdleMoverType == ArcadeIdleMoverType.PLAYER)
            {
                if (newMoveDirection.magnitude >= 1f)
                {
                    newMoveDirection.Normalize();
                }

                Vector3 joystickMovement = new Vector3(newMoveDirection.x * _movementDataSO.SideMoveSpeed, 0, newMoveDirection.y * _movementDataSO.ForwardMoveSpeed);

                if (joystickMovement != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(joystickMovement, Vector3.up);
                    _currentInputVector = new Vector3(joystickMovement.x, _rbd.velocity.y, joystickMovement.z);
                    _rbd.velocity = _currentInputVector; // Применяем движение немедленно
                }
            }
        }

        void Update()
        {
            if (ArcadeIdleMoverType == ArcadeIdleMoverType.PLAYER && !_isJoystickActive) // Обработка ввода с клавиатуры, если джойстик не активен
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                Vector3 keyboardMovement = new Vector3(horizontal, 0f, vertical);

                if (keyboardMovement.magnitude > 1f)
                {
                    keyboardMovement.Normalize();
                }

                keyboardMovement = new Vector3(keyboardMovement.x * _movementDataSO.SideMoveSpeed, 0f, keyboardMovement.z * _movementDataSO.ForwardMoveSpeed);

                if (keyboardMovement != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(keyboardMovement, Vector3.up);
                    _currentInputVector = new Vector3(keyboardMovement.x, _rbd.velocity.y, keyboardMovement.z);
                }
                else
                {
                    _currentInputVector = Vector3.zero;
                }
            }

            // Сбрасываем флаг активности джойстика в конце кадра, чтобы избежать конфликтов ввода
            if (_isJoystickActive)
            {
                _isJoystickActive = false;
            }
        }
    }
}


//using System;
//using HyperCasualPack.ScriptableObjects;
//using UnityEngine;

//namespace HyperCasualPack
//{
//    public enum ArcadeIdleMoverType { PLAYER, EMPLOYEE }

//    public class ArcadeIdleMover : MonoBehaviour
//    {
//        [SerializeField] private Rigidbody _rbd;
//        [SerializeField] private MovementDataSO _movementDataSO;
//        // Removed InputChannelSO since it's no longer needed for WASD input
//        public ArcadeIdleMoverType ArcadeIdleMoverType;
//        private Vector3 _currentInputVector;

//        public bool IsStopped => _currentInputVector.sqrMagnitude < 0.3f;

//        void FixedUpdate()
//        {
//            if (ArcadeIdleMoverType == ArcadeIdleMoverType.PLAYER)
//            {
//                // Handle WASD input
//                float moveHorizontal = Input.GetAxis("Horizontal"); // A/D keys
//                float moveVertical = Input.GetAxis("Vertical"); // W/S keys

//                _currentInputVector = new Vector3(moveHorizontal * _movementDataSO.SideMoveSpeed, _rbd.velocity.y, moveVertical * _movementDataSO.ForwardMoveSpeed);
//                _rbd.velocity = _currentInputVector;

//                Vector3 lookDir = new Vector3(_currentInputVector.x, 0f, _currentInputVector.z);
//                if (lookDir != Vector3.zero)
//                {
//                    transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
//                }
//            }
//            // No changes for EMPLOYEE case as there's no implementation provided
//        }

//        // Removed OnEnable and OnDisable methods as they are no longer needed for WASD input
//    }
//}

//using System;
//using HyperCasualPack.ScriptableObjects;
//using HyperCasualPack.ScriptableObjects.Channels;
//using UnityEngine;

//namespace HyperCasualPack
//{
//    public enum ArcadeIdleMoverType { PLAYER, EMPLOYEE }

//    public class ArcadeIdleMover : MonoBehaviour
//    {
//        [SerializeField] private Rigidbody _rbd;
//        [SerializeField] private MovementDataSO _movementDataSO;
//        [SerializeField] private InputChannelSO _inputChannelSo;
//        public ArcadeIdleMoverType ArcadeIdleMoverType;
//        private Vector3 _currentInputVector;

//        public bool IsStopped => _currentInputVector.sqrMagnitude < 0.3f;

//        private void OnEnable()
//        {
//            _inputChannelSo.JoystickUpdate += OnJoystickUpdate;
//        }

//        private void OnDisable()
//        {
//            _inputChannelSo.JoystickUpdate -= OnJoystickUpdate;
//        }

//        private void FixedUpdate()
//        {
//            if (ArcadeIdleMoverType == ArcadeIdleMoverType.PLAYER)
//            {
//                // Если нет ввода от джойстика, используем клавиатуру
//                if (_currentInputVector == Vector3.zero)
//                {
//                    float moveHorizontal = Input.GetAxis("Horizontal"); // A/D keys
//                    float moveVertical = Input.GetAxis("Vertical"); // W/S keys

//                    Vector3 inputVector = new Vector3(moveHorizontal, 0, moveVertical);

//                    // Нормализация вектора движения, чтобы длина была всегда 1 (или 0, если нет движения)
//                    if (inputVector.magnitude > 1)
//                    {
//                        inputVector.Normalize();
//                    }

//                    _currentInputVector = new Vector3(inputVector.x * _movementDataSO.SideMoveSpeed, _rbd.velocity.y, inputVector.z * _movementDataSO.ForwardMoveSpeed);
//                }

//                _rbd.velocity = _currentInputVector;

//                Vector3 lookDir = new Vector3(_currentInputVector.x, 0f, _currentInputVector.z);
//                if (lookDir != Vector3.zero)
//                {
//                    transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
//                }

//                // Сбрасываем вектор ввода после обработки, чтобы избежать постоянного движения
//                _currentInputVector = Vector3.zero;
//            }
//        }


//        private void OnJoystickUpdate(Vector2 newMoveDirection)
//        {
//            if (ArcadeIdleMoverType == ArcadeIdleMoverType.PLAYER)
//            {
//                if (newMoveDirection.magnitude >= 1f)
//                {
//                    newMoveDirection.Normalize();
//                }

//                _currentInputVector = new Vector3(newMoveDirection.x * _movementDataSO.SideMoveSpeed, _rbd.velocity.y, newMoveDirection.y * _movementDataSO.ForwardMoveSpeed);

//                Vector3 lookDir = new Vector3(_currentInputVector.x, 0f, _currentInputVector.z);
//                if (lookDir != Vector3.zero)
//                {
//                    transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
//                }
//            }
//        }
//    }
//}

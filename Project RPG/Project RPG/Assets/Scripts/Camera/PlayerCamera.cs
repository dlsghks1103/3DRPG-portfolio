using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.PlayerCameraSystem
{
	public class PlayerCamera : MonoBehaviour
	{
		#region Variables
		public float ControlRotationSensitivity = 0.0f;
		public Transform Rig; // The root transform of the camera rig
		public Transform Pivot; // The point at which the camera pivots around
		public Transform Target; // The point at which the camera pivots around
		public Camera Camera;

		public bool isUIBtnDown;

		private bool isOnUI = false;

		private int pointerID = 0;

		private Vector3 _cameraVelocity;
		Vector2 controllRotation;
		#endregion Variables

		#region Unity Methods
		private void Awake()
		{
			var obj = FindObjectsOfType<PlayerCamera>();

			if (obj.Length == 1)
			{
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}

        private void Start()
        {
#if UNITY_ANDROID
			pointerID = 0;
#endif
		}

		private void Update()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			else
			{
				isOnUI = EventSystem.current.IsPointerOverGameObject(pointerID);
			}

			SetPosition(Target.transform.position);
			
			if (Input.GetMouseButton(0) && !isUIBtnDown && !isOnUI) 
			{
				Vector2 CameraInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

				UpdateControlRotation();

				// Adjust the pitch angle (X Rotation)
				float pitchAngle = controllRotation.x;
				pitchAngle -= CameraInput.y * ControlRotationSensitivity;

				// Adjust the yaw angle (Y Rotation)
				float yawAngle = controllRotation.y;
				yawAngle += CameraInput.x * ControlRotationSensitivity;

				controllRotation = new Vector2(pitchAngle, yawAngle);
				SetControlRotation(controllRotation);
			}
		}
		#endregion Unity Methods

		#region Methods
		public void SetPosition(Vector3 position)
		{
			Rig.position = position;
		}

		public void SetControlRotation(Vector2 controlRotation)
		{
			// Y Rotation (Yaw Rotation)
			Quaternion rigTargetLocalRotation = Quaternion.Euler(0.0f, controlRotation.y, 0.0f);

			// X Rotation (Pitch Rotation)
			Quaternion pivotTargetLocalRotation = Quaternion.Euler(controlRotation.x, 0.0f, 0.0f);

			Rig.localRotation = rigTargetLocalRotation;
			Pivot.localRotation = pivotTargetLocalRotation;
		}

		public void UpdateControlRotation()
		{
			// Adjust the pitch angle (X Rotation)
			float pitchAngle = controllRotation.x;
			pitchAngle %= 360.0f;
			pitchAngle = Mathf.Clamp(pitchAngle, 5, 60);

			// Adjust the yaw angle (Y Rotation)
			float yawAngle = controllRotation.y;
			yawAngle %= 360.0f;

			controllRotation = new Vector2(pitchAngle, yawAngle);
		}

		public void PointerDown()
		{
			isUIBtnDown = true;
		}

		public void PointerUp()
		{
			isUIBtnDown = false;
		}
		#endregion Methods
	}
}

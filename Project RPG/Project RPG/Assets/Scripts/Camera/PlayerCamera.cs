using UnityEngine;

namespace RPG.PlayerCamera
{
	public class PlayerCamera : MonoBehaviour
	{
		public float ControlRotationSensitivity = 0.0f;
		public Transform Rig; // The root transform of the camera rig
		public Transform Pivot; // The point at which the camera pivots around
		public Transform Target; // The point at which the camera pivots around
		public Camera Camera;

		private Vector3 _cameraVelocity;
		Vector2 controllRotation;

		private void Update()
		{
			SetPosition(Target.transform.position);

			if (Input.multiTouchEnabled) 
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
			pitchAngle = Mathf.Clamp(pitchAngle, -45, 75);

			// Adjust the yaw angle (Y Rotation)
			float yawAngle = controllRotation.y;
			yawAngle %= 360.0f;

			controllRotation = new Vector2(pitchAngle, yawAngle);
		}
	}
}

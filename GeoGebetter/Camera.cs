using System;
using OpenTK.Mathematics;

namespace GeoGebetter
{
	public class Camera
	{
		public Vector3 Position;

		private Vector3 _front;
		private Vector3 _right;
		private Vector3 _up;
		
		public Vector3 Front
		{
			get => _front;
		}
		public Vector3 Right
		{
			get => _right;
		}
		public Vector3 Up
		{
			get => _up;
		}

		private float _pitch = 0f;
		private float _yaw = 0f;
		private float _roll = 0f;

		public float AspectRatio;

		private float _fov;
		
		public Camera(Vector3 position, float aspectRatio)
		{
			Position = position;
			AspectRatio = aspectRatio;
		}
		
		public float Pitch
		{
			get => MathHelper.RadiansToDegrees(_pitch);
			set
			{
				// We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
				// of weird "bugs" when you are using euler angles for rotation.
				// If you want to read more about this you can try researching a topic called gimbal lock
				var angle = MathHelper.Clamp(value, -89f, 89f);
				_pitch = MathHelper.DegreesToRadians(angle);
				UpdateVectors();
			}
		}
		
		public float Yaw
		{
			get => MathHelper.RadiansToDegrees(_yaw);
			set
			{
				_yaw = MathHelper.DegreesToRadians(value);
				UpdateVectors();
			}
		}
		
		public float Fov
		{
			get => MathHelper.RadiansToDegrees(_fov);
			set
			{
				float angle = MathHelper.Clamp(value, 1f, 90f);
				_fov = MathHelper.DegreesToRadians(angle);
			}
		}
		

		public Matrix4 GetViewMatrix()
		{
			Matrix4 view = Matrix4.LookAt(Position, Position + _front, _up);
			return view;
		}

		public Matrix4 GetProjectionMatrix()
		{
			return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
		}

		public void UpdateVectors()
		{
			_front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
			_front.Y = MathF.Sin(_pitch);
			_front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

			_front = Vector3.Normalize(_front);

			_right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
			_up = Vector3.Normalize(Vector3.Cross(_right, _front));
		}
	}
}
using System;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GeoGebetter
{
	public class InputManager
	{
		private Game mainWindow;
		
		private Camera camera;
		private KeyboardState keyInput;
		private MouseState mouseInput;
		private bool _firstMove = true;
		private Vector2 _lastPos;

		private Vector2 deltaMouse;
		
		private float rightAscension;

		private float declination;

		private float zoom = 1f;

		public InputType InputType = InputType.TurnTable;
		
		public InputManager(Camera _camera, KeyboardState _keyboardState, MouseState _mouseState, Game _mainWindow)
		{
			camera = _camera;
			keyInput = _keyboardState;
			mouseInput = _mouseState;
			mainWindow = _mainWindow;
			
			
		}

		private Vector2 CalculateDeltaMouse(FrameEventArgs e)
		{
			Vector2 dp = new Vector2(mouseInput.X, mouseInput.Y) - _lastPos;
			
			if (_firstMove) // This bool variable is initially set to true.
			{
				_lastPos = new Vector2(mouseInput.X, mouseInput.Y);
				_firstMove = false;
				return Vector2.Zero;
			}
			else
			{
				_lastPos = new Vector2(mouseInput.X, mouseInput.Y);
			}
			return dp;
		}

		public void TakeInput(FrameEventArgs e)
		{
			var io = ImGui.GetIO();
			
			deltaMouse = CalculateDeltaMouse(e);
			if (!io.WantCaptureKeyboard && ImGui.IsMouseDown(ImGuiMouseButton.Left))
			{
				mainWindow.CursorState = CursorState.Grabbed;
				
				switch (InputType)
				{
					case (InputType.Fly):
					{
						TakeInputsFly(e);
						break;
					}
					case InputType.TurnTable:
					{
						TakeInputTurntable(e);
						break;
					}
				}
			}
			else
			{
				mainWindow.CursorState = CursorState.Normal;
			}
		}
		
		public void TakeInputTurntable(FrameEventArgs e)
		{
			const float zoomSpeed = 1.5f;
			const float sensitivity = 0.2f;
			
			if (keyInput.IsKeyDown(Keys.Up))
			{
				zoom += zoomSpeed * (float)e.Time;
			}
			
			if (keyInput.IsKeyDown(Keys.Down))
			{
				zoom -= zoomSpeed * (float)e.Time;
				zoom = zoom > 0 ? zoom : 0;
			}

			declination = Math.Clamp(declination + deltaMouse.Y * sensitivity, 91f, 269f);
			rightAscension += deltaMouse.X * sensitivity;

			float ophyp = MathF.Abs(MathF.Cos(declination * 0.01745329251f) * zoom);
			
			camera.Position.X = MathF.Cos(rightAscension * 0.01745329251f) * ophyp;
			camera.Position.Z = MathF.Sin(rightAscension * 0.01745329251f) * ophyp;
			camera.Position.Y = MathF.Sin(declination * 0.01745329251f) * zoom;

			camera.Yaw = rightAscension - 180f;
			camera.Pitch = declination - 180f;
		}
		
		public void TakeInputsFly(FrameEventArgs e)
		{
			const float cameraSpeed = 1.5f;
			const float sensitivity = 0.2f;

			if (keyInput.IsKeyDown(Keys.W))
			{
				camera.Position += camera.Front * cameraSpeed * (float)e.Time; // Forward
			}

			if (keyInput.IsKeyDown(Keys.S))
			{
				camera.Position -= camera.Front * cameraSpeed * (float)e.Time; // Backwards
			}

			if (keyInput.IsKeyDown(Keys.A))
			{
				camera.Position -= camera.Right * cameraSpeed * (float)e.Time; // Left
			}

			if (keyInput.IsKeyDown(Keys.D))
			{
				camera.Position += camera.Right * cameraSpeed * (float)e.Time; // Right
			}

			if (keyInput.IsKeyDown(Keys.Space))
			{
				camera.Position += camera.Up * cameraSpeed * (float)e.Time; // Up
			}

			if (keyInput.IsKeyDown(Keys.LeftShift))
			{
				camera.Position -= camera.Up * cameraSpeed * (float)e.Time; // Down
			}

			if (_firstMove) // This bool variable is initially set to true.
			{
				_lastPos = new Vector2(mouseInput.X, mouseInput.Y);
				_firstMove = false;
			}
			else
			{
				// Calculate the offset of the mouse position
				var deltaX = mouseInput.X - _lastPos.X;
				var deltaY = mouseInput.Y - _lastPos.Y;
				_lastPos = new Vector2(mouseInput.X, mouseInput.Y);

				// Apply the camera pitch and yaw (we clamp the pitch in the camera class)
				camera.Yaw += deltaX * sensitivity;
				camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
			}
		}
	}

	public enum InputType
	{
		Fly,
		TurnTable
	}
}
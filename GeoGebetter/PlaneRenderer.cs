using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GeoGebetter
{
	public class PlaneRenderer : RenderObject
	{
		private Vector3 planeEquation;
		
		public Vector3 PlaneEquation
		{
			get => planeEquation;
			set
			{
				const float radToDeg = 180 / MathF.PI;
				
				if (value == planeEquation)
				{
					return;
				}
				
				planeEquation = value;
				
				Rotation = new Vector3(MathF.Atan(value.Z / value.Y) * radToDeg, 0f, MathF.Atan(value.X / value.Y)* radToDeg);
			}
		}

		public PlaneRenderer(Shader shader, Vector3 _planeEq) : base(shader, Primitives.Plane)
		{
			PlaneEquation = _planeEq;
			
			OnLoad();
		}
		
		public override void OnRenderFrame(FrameEventArgs e, Matrix4 _view, Matrix4 _projection)
		{
			base.OnRenderFrame(e,_view,_projection);
		}
	}
}
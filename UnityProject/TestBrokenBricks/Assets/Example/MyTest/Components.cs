using ECS;
using UnityEngine;

namespace MyTest.Components
{
	public class PositionComponent : IComponent
	{
		public Vector2 position;
	}

	public class MovementComponent : IComponent
	{
//		public Vector2 acceleration;
		public Vector2 velocity;
		public float speed;
//		public float maxSpeed;
	}

	public class ViewComponent : IComponent
	{
		public GameObject viewPrefab;
		public GameObject view;
	}

	public class ControllerComponent : IComponent
	{
		public Vector2 movement;
	}

	public class InputComponent : IComponent
	{
		public string horizontalAxisName;
		public string verticalAxisName;
	}
}
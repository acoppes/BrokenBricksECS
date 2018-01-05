using ECS;
using UnityEngine;

namespace MyTest.Components
{
	public class PositionComponent : IComponent
	{
		public Vector3 position;
		public Vector2 lookingDirection;
	}

	public class MovementComponent : IComponent
	{
//		public Vector2 acceleration;
		public Vector2 velocity;
		public float speed;
//		public float maxSpeed;
	}

	public class JumpComponent : IComponent
	{
		public float jumpSpeed;
		public float maxJumpHeight;

		public bool isJumping = false;
		public bool isFalling = false;
	}

	public class ViewComponent : IComponent
	{
		public GameObject viewPrefab;
		public GameObject view;
		public Animator animator;
		public SpriteRenderer sprite;
	}

	public class ControllerComponent : IComponent
	{
		public Vector2 movement;
		public bool isJumpPressed;
	}

	public class InputComponent : IComponent
	{
		public string horizontalAxisName;
		public string verticalAxisName;
		public string jumpActionName;
	}
}
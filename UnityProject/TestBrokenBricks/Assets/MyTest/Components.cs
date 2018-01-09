using ECS;
using UnityEngine;
using System;

namespace MyTest.Components
{
	[Serializable]
	public class DelegatePhysicsComponent : IComponent {

		[NonSerialized]
		public Vector3 force;

		public Vector3 velocity;

		public float maxForce;

		public Vector3 position;

		public float gravityMultiplier = 1.0f;

		public float frictionMultiplier = 1.0f;

		public Vector3 AddForce(Vector3 force)
		{
			this.force += force;
			return this.force;
		}

		public void StopAtHeight(float height)
		{
			position.z = height;
			velocity.z = 0.0f;
		}

		public bool IsOnFloor()
		{
			return Mathf.Abs (position.z - 0.0f) < Mathf.Epsilon;
		}
	}

	[Serializable]
	public class LimitVelocityComponent : IComponent
	{
		public float maxSpeedHorizontal;
	}

	[Serializable]
	public class PositionComponent : IComponent
	{
		public Vector3 position;

		[NonSerialized]
		public Vector2 lookingDirection;
	}

	[Serializable]
	public class MovementComponent : IComponent
	{
		[NonSerialized]
		public Vector2 direction;

		// it is the movement force (acceleration)
		public float speed;

		public float maxSpeedHorizontal;
	}

	[Serializable]
	public class JumpComponent : IComponent
	{
		public float jumpForce;

		[NonSerialized]
		public float currentJumpForce;

		public float jumpStopFactor;

		[NonSerialized]
		public bool isJumping = false;

		[NonSerialized]
		public bool isFalling = false;
	}

	[Serializable]
	public class ViewComponent : IComponent
	{
		public GameObject viewPrefab;
		[NonSerialized]
		public GameObject view;
		[NonSerialized]
		public Animator animator;
		[NonSerialized]
		public SpriteRenderer sprite;
	}

	[Serializable]
	public class ControllerComponent : IComponent
	{
		[NonSerialized]
		public Vector2 movement;

		[NonSerialized]
		public bool isJumpPressed;
	}

	[Serializable]
	public class InputComponent : IComponent
	{
		public string horizontalAxisName;
		public string verticalAxisName;
		public string jumpActionName;
	}
}
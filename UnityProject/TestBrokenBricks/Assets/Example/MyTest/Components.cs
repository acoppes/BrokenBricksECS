using ECS;
using UnityEngine;

namespace MyTest.Components
{
	public class PhysicsParticleComponent : IComponent {

		public Vector3 force;
		public Vector3 velocity;

		public float maxSpeed;
		public float maxForce;

		public Vector3 position;

		public float gravityMultiplier = 1.0f;

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
	}

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
//		public float jumpSpeed;
//		public float maxJumpHeight;
//		public float minJumpHeight;

		public float jumpForce;
		public float currentJumpForce;
		public float jumpStopFactor;

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
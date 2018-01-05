using ECS;
using MyTest.Components;
using UnityEngine;

namespace MyTest.Systems
{
	public class ControllerSystem : ComponentSystem
	{
		ComponentArray<ControllerComponent> _controllers;
		ComponentArray<MovementComponent> _movements;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();
			var group = _entityManager.GetComponentGroup (typeof(ControllerComponent), typeof(MovementComponent));
			_controllers = group.GetComponent<ControllerComponent> ();
			_movements = group.GetComponent<MovementComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			for (int i = 0; i < _controllers.Length; i++) {
				_movements [i].velocity = _controllers [i].movement.normalized * _movements[i].speed;
			}
		}

	}

	public class JumpSystem : ComponentSystem
	{
		ComponentArray<ControllerComponent> _controllers;
		ComponentArray<JumpComponent> _jumps;
		ComponentArray<PositionComponent> _positions;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();
			var group = _entityManager.GetComponentGroup (typeof(ControllerComponent), typeof(JumpComponent), typeof(PositionComponent));
			_controllers = group.GetComponent<ControllerComponent> ();
			_jumps = group.GetComponent<JumpComponent> ();
			_positions = group.GetComponent<PositionComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			for (int i = 0; i < _controllers.Length; i++) {
				var jump = _jumps [i];

				if (!jump.isFalling && !jump.isJumping) {
					jump.isJumping = _controllers [i].isJumpPressed;
				}

				// TODO: we could use our custom GlobalTime

				if (jump.isJumping) {

					var p = _positions [i].position;
					p.z += jump.jumpSpeed * Time.deltaTime;
					_positions [i].position = p;

					if (p.z > jump.maxJumpHeight || !_controllers[i].isJumpPressed) {
						p.z = jump.maxJumpHeight;
						jump.isFalling = true;
						jump.isJumping = false;
					}
				}

				if (jump.isFalling) {
					var p = _positions [i].position;
					p.z -= 9.8f * Time.deltaTime;

					if (p.z < 0) {
						p.z = 0;
						jump.isFalling = false;
					}

					_positions [i].position = p;
				}
			}
		}

	}
}
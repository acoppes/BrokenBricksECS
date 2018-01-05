using ECS;
using MyTest.Components;
using UnityEngine;

namespace MyTest.Systems
{
	public class JumpSystem : ComponentSystem
	{
		ComponentArray<ControllerComponent> _controllers;
		ComponentArray<JumpComponent> _jumps;
		ComponentArray<PositionComponent> _positions;
		ComponentArray<PhysicsParticleComponent> _physicsParticles;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();
			var group = _entityManager.GetComponentGroup (typeof(ControllerComponent), 
				typeof(JumpComponent), 
				typeof(PositionComponent), 
				typeof(PhysicsParticleComponent));
			
			_controllers = group.GetComponent<ControllerComponent> ();
			_jumps = group.GetComponent<JumpComponent> ();
			_positions = group.GetComponent<PositionComponent> ();
			_physicsParticles = group.GetComponent<PhysicsParticleComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			for (int i = 0; i < _controllers.Length; i++) {
				var jump = _jumps [i];
				var physicsParticle = _physicsParticles [i];

				if (!jump.isFalling && !jump.isJumping && Mathf.Abs(physicsParticle.position.z) < Mathf.Epsilon) {
					jump.isJumping = _controllers [i].isJumpPressed;
					if (jump.isJumping) {
						jump.currentJumpForce = jump.jumpForce;
					}
				}

				// TODO: we could use our custom GlobalTime

				var p = _positions [i].position;
				p.z = _physicsParticles [i].position.z;
			
				if (jump.isJumping) {

					_physicsParticles [i].AddForce (new Vector3 (0, 0, 1) * jump.currentJumpForce);
					jump.currentJumpForce -= jump.jumpStopFactor * Time.deltaTime;

					if (jump.currentJumpForce <= 0 || !_controllers [i].isJumpPressed) {
						jump.currentJumpForce = 0;

						if (physicsParticle.velocity.z <= 0) {
							jump.isJumping = false;
							jump.isFalling = true;					
						}
					}
						
				} else if (!jump.isFalling && p.z > 0) {
					jump.isFalling = true;
				}

				if (jump.isFalling) {

					if (Mathf.Abs(p.z - 0.0f) < Mathf.Epsilon) {
						p.z = 0;
						jump.isFalling = false;
					}
				}

				_positions [i].position = p;
			}
		}

	}
}
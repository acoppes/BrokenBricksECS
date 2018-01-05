using ECS;
using MyTest.Components;
using UnityEngine;

namespace MyTest.Systems
{
	public class DelegatePhysicsSystem : ComponentSystem
	{
		ComponentArray<DelegatePhysicsComponent> _physicsParticles;

		[InjectDependency]
		protected EntityManager _entityManager;

		readonly Vector3 gravity = new Vector3(0, 0, -9.8f);

		public override void OnStart ()
		{
			base.OnStart ();

			var group = _entityManager.GetComponentGroup (
				typeof(DelegatePhysicsComponent)
			);

			_physicsParticles = group.GetComponent<DelegatePhysicsComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			var dt = Time.deltaTime;

			for (int i = 0; i < _physicsParticles.Length; i++) {
				var physics = _physicsParticles [i];

				if (!physics.IsOnFloor())
					physics.AddForce (gravity * physics.gravityMultiplier);

				physics.force = Vector3.ClampMagnitude(physics.force, physics.maxForce);

				if (physics.force.sqrMagnitude > 0.0001f) {
					Vector3 deltaV = physics.force * dt;
					physics.velocity += deltaV;
				}

				var horizontalVelocity = new Vector2 (physics.velocity.x, physics.velocity.y);
				horizontalVelocity = Vector2.ClampMagnitude (horizontalVelocity, physics.maxSpeedHorizontal);

				var verticalVelocity = new Vector3 (0, 0, physics.velocity.z);
				verticalVelocity = Vector3.ClampMagnitude (verticalVelocity, physics.maxSpeedVertical);

				physics.velocity = new Vector3(horizontalVelocity.x, horizontalVelocity.y, verticalVelocity.z);

//				physics.velocity = Vector3.ClampMagnitude(physics.velocity, physics.maxSpeed);

				if (physics.velocity.sqrMagnitude < 0.0001f)
					physics.velocity.Set(0, 0, 0);

				physics.position += physics.velocity * dt;

				physics.force = Vector3.zero;

				// colliison with floor
				if (physics.position.z < 0.0f) {
					physics.StopAtHeight (0.0f);
				}
			}
		}

	}
	
}
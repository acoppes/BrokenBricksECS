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
				var physicsParticle = _physicsParticles [i];

				physicsParticle.AddForce (gravity * physicsParticle.gravityMultiplier);

				physicsParticle.force = Vector3.ClampMagnitude(physicsParticle.force, physicsParticle.maxForce);

				Vector3 deltaV = physicsParticle.force * dt;
				physicsParticle.velocity += deltaV;
				physicsParticle.velocity = Vector3.ClampMagnitude(physicsParticle.velocity, physicsParticle.maxSpeed);

				physicsParticle.position += physicsParticle.velocity * dt;

				physicsParticle.force = Vector3.zero;

				// colliison with floor
				if (physicsParticle.position.z < 0.0f) {
					physicsParticle.StopAtHeight (0.0f);
				}
			}
		}

	}
	
}
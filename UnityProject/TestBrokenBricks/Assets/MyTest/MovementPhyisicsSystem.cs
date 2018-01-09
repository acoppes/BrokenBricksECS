using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class MovementPhyisicsSystem : ComponentSystem
	{
		ComponentGroup _group;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			_group = _entityManager.GetComponentGroup (
				typeof(MovementPhysicsComponent), 
				typeof(PositionComponent),
				typeof(DelegatePhysicsComponent)
			);
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			var dt = Time.deltaTime;

			var movementArray = _group.GetComponent<MovementPhysicsComponent> ();
			var positionArray = _group.GetComponent<PositionComponent> ();
			var physicsArray = _group.GetComponent<DelegatePhysicsComponent> ();

			Vector3 horizontalForce = new Vector3();

			for (int i = 0; i < movementArray.Length; i++) {
				var movement = movementArray [i];
				var position = positionArray [i];
				var physics = physicsArray [i];

				var v = physics.velocity;
				v.z = 0.0f;

				if (movement.direction.sqrMagnitude > 0) {

					var moveForce = (Vector3) movement.direction.normalized * movement.force;

					var maxSpeedHorizontal = movement.maxSpeedHorizontal;

					if (maxSpeedHorizontal > 0) {
						// this does some "estimation" of physics behaviour.. not sure if here is the right
						// place but want to do this logic only for movement...
						var vh = physics.velocity + moveForce * dt;
						vh.Set (vh.x, vh.y, 0);

						if (vh.sqrMagnitude > (maxSpeedHorizontal * maxSpeedHorizontal) && dt > 0) {
							var limitForce = (vh - (vh.normalized * maxSpeedHorizontal)) / dt;
							moveForce += (limitForce * -1);
						} 
					}

					physics.AddForce (moveForce);

				} else {
					if (physics.IsOnFloor ()) {
						physics.AddForce (physics.velocity * physics.frictionMultiplier * -1.0f);
					}
				}

				var p = physics.position;
				// managed by jump for now...
				p.z = position.position.z;

//				_positions[i].position = _positions[i].position + (Vector3)(movement.velocity * Time.deltaTime);

				if (Mathf.Abs (physics.velocity.x) > 0) { 
					position.lookingDirection = physics.velocity.normalized;
				}

				position.position = p;
			}
		}

	}

}
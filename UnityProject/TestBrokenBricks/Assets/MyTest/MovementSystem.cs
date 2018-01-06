using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class MovementSystem : ComponentSystem
	{
		ComponentArray<PositionComponent> _positions;
		ComponentArray<MovementComponent> _movements;
		ComponentArray<DelegatePhysicsComponent> _physics;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			var group = _entityManager.GetComponentGroup (
				typeof(MovementComponent), 
				typeof(PositionComponent),
				typeof(DelegatePhysicsComponent)
			);

			_movements = group.GetComponent<MovementComponent> ();
			_positions = group.GetComponent<PositionComponent> ();
			_physics = group.GetComponent<DelegatePhysicsComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			for (int i = 0; i < _movements.Length; i++) {

				var v = _physics [i].velocity;
				v.z = 0.0f;
		
				// physics.setMaxSpeedInPlaneXY()

				if (_movements [i].direction.sqrMagnitude > 0)
					_physics [i].AddForce (_movements [i].direction.normalized * _movements [i].speed);
				else {
					if (_physics [i].IsOnFloor ()) {
						_physics [i].AddForce (_physics [i].velocity * _physics[i].frictionMultiplier * -1.0f);
					}
				}

				// movement friction??

				var p = _physics [i].position;
				// managed by jump for now...
				p.z = _positions [i].position.z;

//				_positions[i].position = _positions[i].position + (Vector3)(movement.velocity * Time.deltaTime);

				if (Mathf.Abs (_physics[i].velocity.x) > 0) { 
					_positions [i].lookingDirection = _physics[i].velocity.normalized;
				}

				_positions [i].position = p;
			}
		}

	}

}
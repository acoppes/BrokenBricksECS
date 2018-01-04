using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class MovementSystem : ComponentSystem
	{
		ComponentArray<PositionComponent> _positions;
		ComponentArray<MovementComponent> _movements;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();
			var group = _entityManager.GetComponentGroup (typeof(MovementComponent), typeof(PositionComponent));
			_movements = group.GetComponent<MovementComponent> ();
			_positions = group.GetComponent<PositionComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			for (int i = 0; i < _movements.Length; i++) {
				var movement = _movements [i];
				_positions[i].position = _positions[i].position + movement.velocity * Time.deltaTime;
			}
		}

	}

}
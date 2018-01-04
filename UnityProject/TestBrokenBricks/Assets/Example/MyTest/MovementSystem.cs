using ECS;
using UnityEngine;

namespace MyTest.Systems
{

	public class MovementSystem : ComponentSystem
	{
		[InjectTuple]
		ComponentArray<MovementComponent> _movements;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();
			var group = _entityManager.GetComponentGroup (typeof(MovementComponent));
			_movements = group.GetComponent<MovementComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			for (int i = 0; i < _movements.Length; i++) {
				var movement = _movements [i];
				movement.position += movement.velocity * Time.deltaTime;
			}
		}

	}
	
}
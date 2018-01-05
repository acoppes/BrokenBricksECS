using ECS;
using MyTest.Components;

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
				_movements [i].direction = _controllers [i].movement.normalized;
			}
		}

	}

}
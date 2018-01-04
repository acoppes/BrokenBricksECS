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

	public class ViewSystem : ComponentSystem
	{
		[InjectTuple]
		ComponentArray<ViewComponent> _views;

		[InjectTuple]
		ComponentArray<MovementComponent> _movements;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			var group = _entityManager.GetComponentGroup (typeof(MovementComponent), typeof(ViewComponent));
			_movements = group.GetComponent<MovementComponent> ();
			_views = group.GetComponent<ViewComponent> ();

			// we probably have to use this one...
			group.SubscribeOnEntityAdded (this);

//			_entityManager.SubscribeOnEntityAdded (this);
		}

		public override void OnEntityAdded (object sender, Entity entity)
		{
			// base.OnEntityAdded (sender, entity);

			if (!_entityManager.HasComponent<ViewComponent> (entity))
				return;

			var view = _entityManager.GetComponent<ViewComponent> (entity);

			//			var view = _views.GetComponent (entity);

			if (view.viewPrefab != null) {
				view.view = GameObject.Instantiate (view.viewPrefab);
			}

			_entityManager.SetComponent (entity, view);
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();
			for (int i = 0; i < _views.Length; i++) {
				var view = _views [i];

				if (view.viewPrefab == null || view.view == null)
					continue;
//
//				if (view.view == null) {
//					view.view = GameObject.Instantiate (view.viewPrefab);
//				}

				_views [i].view.transform.position = _movements [i].position;
			}
		}

	}
}
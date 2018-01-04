using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class ViewSystem : ComponentSystem
	{
		[InjectTuple]
		ComponentArray<ViewComponent> _views;

		[InjectTuple]
		ComponentArray<PositionComponent> _positions;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			var group = _entityManager.GetComponentGroup (typeof(PositionComponent), typeof(ViewComponent));
			_positions = group.GetComponent<PositionComponent> ();
			_views = group.GetComponent<ViewComponent> ();

			// we probably have to use this one...
			group.SubscribeOnEntityAdded (this);

//			_entityManager.SubscribeOnEntityAdded (this);
		}

		public override void OnEntityAdded (object sender, Entity entity)
		{
			base.OnEntityAdded (sender, entity);

			if (!_entityManager.HasComponent<ViewComponent> (entity))
				return;

			var view = _entityManager.GetComponent<ViewComponent> (entity);

			if (view.viewPrefab != null && view.view == null) {
				view.view = GameObject.Instantiate (view.viewPrefab);
			}

		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();
			for (int i = 0; i < _views.Length; i++) {
				var view = _views [i];

				if (view.view == null)
					continue;

				_views [i].view.transform.position = _positions[i].position;
			}
		}

	}
}
using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class ViewSystem : ComponentSystem, IEntityAddedEventListener, IEntityRemovedEventListener
	{
		ComponentArray<ViewComponent> _views;
		ComponentArray<PositionComponent> _positions;
		ComponentArray<MovementPhysicsComponent> _movements;
		ComponentArray<JumpComponent> _jumps;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			var group = _entityManager.GetComponentGroup (typeof(PositionComponent), typeof(ViewComponent), typeof(MovementPhysicsComponent), typeof(JumpComponent));
			_positions = group.GetComponent<PositionComponent> ();
			_views = group.GetComponent<ViewComponent> ();
			_movements = group.GetComponent<MovementPhysicsComponent> ();
			_jumps = group.GetComponent<JumpComponent> ();

			// we probably have to use this one...
			 group.SubscribeOnEntityAdded (this);
		}

		public void OnEntityAdded (object sender, Entity entity)
		{
			if (!_entityManager.HasComponent<ViewComponent> (entity))
				return;

			var view = _entityManager.GetComponent<ViewComponent> (entity);

			if (view.viewPrefab != null && view.view == null) {
				view.view = GameObject.Instantiate (view.viewPrefab);
				view.animator = view.view.GetComponentInChildren<Animator> ();
				view.sprite = view.view.GetComponentInChildren<SpriteRenderer> ();
			}

		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();
			for (int i = 0; i < _views.Length; i++) {
				var view = _views [i];

				if (view.view == null)
					continue;

				var pos = _positions [i].position;

				view.view.transform.position = new Vector3 (pos.x, 0, pos.y);
				view.sprite.transform.localPosition = new Vector3(0, pos.z, 0);

				if (view.animator != null) {
					view.animator.SetBool ("Walking", _movements [i].direction.sqrMagnitude > 0);
					view.sprite.flipX = _positions[i].lookingDirection.x < 0;

					view.animator.SetBool("Jumping", _jumps[i].isJumping);
					view.animator.SetBool("Falling", _jumps[i].isFalling);
				}
			}
		}

	}
}
using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class ViewSystem : ComponentSystem
	{
		ComponentArray<ViewComponent> _views;
		ComponentArray<PositionComponent> _positions;
		ComponentArray<MovementComponent> _movements;
		ComponentArray<JumpComponent> _jumps;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			var group = _entityManager.GetComponentGroup (typeof(PositionComponent), typeof(ViewComponent), typeof(MovementComponent), typeof(JumpComponent));
			_positions = group.GetComponent<PositionComponent> ();
			_views = group.GetComponent<ViewComponent> ();
			_movements = group.GetComponent<MovementComponent> ();
			_jumps = group.GetComponent<JumpComponent> ();

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

				view.view.transform.position = new Vector3 (pos.x, pos.y, 0);
				view.sprite.transform.localPosition = new Vector3(0, pos.z, 0);

				if (view.animator != null) {
					view.animator.SetBool ("Walking", _movements [i].velocity.sqrMagnitude > 0);
					view.sprite.flipX = _positions[i].lookingDirection.x < 0;

					view.animator.SetBool("Jumping", _jumps[i].isJumping);
					view.animator.SetBool("Falling", _jumps[i].isFalling);
				}
			}
		}

	}
}
using ECS;
using MyTest.Components;
using UnityEngine;

namespace MyTest.Systems
{
	public class LimitVelocitySystem : ComponentSystem
	{
		ComponentGroup _group;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			_group = _entityManager.GetComponentGroup (
				typeof(DelegatePhysicsComponent),
				typeof(LimitVelocityComponent)
			);
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			var dt = Time.deltaTime;

			var physicsArray = _group.GetComponent<DelegatePhysicsComponent> ();
			var limitVelocity = _group.GetComponent<LimitVelocityComponent> ();

			Vector3 horizontalForce = new Vector3();

			// this will limit also an explosion impulse for example

			for (int i = 0; i < physicsArray.Length; i++) {
				var physics = physicsArray[i];

				var force = physics.force;

				horizontalForce.Set(force.x, force.y, 0);
				var vh = physics.velocity + horizontalForce * dt;
				vh.Set (vh.x, vh.y, 0);

				var maxSpeedHorizontal = limitVelocity [i].maxSpeedHorizontal;

				if (maxSpeedHorizontal > 0) {
					if (vh.sqrMagnitude > (maxSpeedHorizontal * maxSpeedHorizontal) && dt > 0) {
						var limitForce = (vh - (vh.normalized * maxSpeedHorizontal)) / dt;
						physics.AddForce (limitForce * -1);
					} 
				}
					
			}
		}

	}
}
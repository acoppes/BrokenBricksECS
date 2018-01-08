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

			var physics = _group.GetComponent<DelegatePhysicsComponent> ();
			var limitVelocity = _group.GetComponent<LimitVelocityComponent> ();

			Vector3 horizontalForce = new Vector3();
			Vector3 verticalForce = new Vector3();

			for (int i = 0; i < physics.Length; i++) {
				var force = physics [i].force;

				horizontalForce.Set(force.x, force.y, 0);
				verticalForce.Set (0, 0, verticalForce.z);

				var vh = physics [i].velocity + horizontalForce * dt;
				var vv = physics [i].velocity + verticalForce * dt;

				vh.Set (vh.x, vh.y, 0);
				vv.Set (0, 0, vv.z);

				var maxSpeedHorizontal = limitVelocity [i].maxSpeedHorizontal;

				if (maxSpeedHorizontal > 0) {
					if (vh.sqrMagnitude > (maxSpeedHorizontal * maxSpeedHorizontal) && dt > 0) {
						var limitForce = (vh - (vh.normalized * maxSpeedHorizontal)) / dt;
						physics [i].AddForce (limitForce * -1);
					} 
				}

				var maxSpeedVertical = limitVelocity [i].maxSpeedVertical;

				if (maxSpeedVertical > 0) {
					if (vv.sqrMagnitude > (maxSpeedVertical * maxSpeedVertical) && dt > 0) {
						var limitForce = (vv - (vv.normalized * maxSpeedVertical)) / dt;
						physics [i].AddForce (limitForce * -1);
					} 
				}
			}
		}

	}
}
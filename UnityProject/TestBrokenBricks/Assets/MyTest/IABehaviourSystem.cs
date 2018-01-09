using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class IABehaviourSystem : ComponentSystem
	{
		ComponentGroup _group;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();

			_group = _entityManager.GetComponentGroup (
				typeof(IABehaviourComponent), 
				typeof(ControllerComponent),
				typeof(PositionComponent)
			);
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			var dt = Time.deltaTime;

			var behaviourArray = _group.GetComponent<IABehaviourComponent> ();
			var controllerArray = _group.GetComponent<ControllerComponent> ();
			var positionArray = _group.GetComponent<PositionComponent> ();

			for (int i = 0; i < behaviourArray.Length; i++) {
			
				var behaviour = behaviourArray [i];
				var controller = controllerArray [i];
				var position = positionArray [i];

				controller.movement = Vector2.zero;

				if (behaviour.waitingForAction) {
					behaviour.actionTime += dt;

					if (behaviour.actionTime > behaviour.waitForActionTime) {
						// decide next action...

						var nextAction = UnityEngine.Random.Range (0, 2);
						if (nextAction == 0) {
							behaviour.waitingForAction = true;
							behaviour.walking = false;
							behaviour.actionTime = 0;
						} else if (nextAction == 1) {
							behaviour.walking = true;
							behaviour.waitingForAction = false;
							behaviour.destination = (Vector3) UnityEngine.Random.insideUnitCircle * behaviour.maxRandomDistance;
						}
					}
				} else if (behaviour.walking) {
				
					// walk to destination

					controller.movement = (behaviour.destination - position.position).normalized;

					if (Vector3.Distance (position.position, behaviour.destination) < 0.1f) {

						var nextAction = UnityEngine.Random.Range (0, 2);
						if (nextAction == 0) {
							behaviour.waitingForAction = true;
							behaviour.walking = false;
							behaviour.actionTime = 0;
						} else if (nextAction == 1) {
							behaviour.walking = true;
							behaviour.waitingForAction = false;
							behaviour.destination = (Vector3) UnityEngine.Random.insideUnitCircle * behaviour.maxRandomDistance;
						}
							
					}

				}


			}
		}
	}

}
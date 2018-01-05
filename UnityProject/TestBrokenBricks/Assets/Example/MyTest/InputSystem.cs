using ECS;
using UnityEngine;
using MyTest.Components;

namespace MyTest.Systems
{
	public class InputSystem : ComponentSystem
	{
		ComponentArray<InputComponent> _inputs;
		ComponentArray<ControllerComponent> _controllers;

		[InjectDependency]
		protected EntityManager _entityManager;

		public override void OnStart ()
		{
			base.OnStart ();
			var group = _entityManager.GetComponentGroup (typeof(InputComponent), typeof(ControllerComponent));
			_inputs = group.GetComponent<InputComponent> ();
			_controllers = group.GetComponent<ControllerComponent> ();
		}

		public override void OnFixedUpdate ()
		{
			base.OnFixedUpdate ();

			for (int i = 0; i < _inputs.Length; i++) {

				_controllers [i].movement = new Vector2 () { 
					x = Input.GetAxis(_inputs[i].horizontalAxisName),
					y = Input.GetAxis(_inputs[i].verticalAxisName),
				};

				_controllers [i].isJumpPressed = Input.GetButton (_inputs [i].jumpActionName);
			}
		}

	}
	
}
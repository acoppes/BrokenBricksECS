using ECS;
using UnityEngine;
using MyTest.Components;

public class LoadLevelSystem : ComponentSystem
{
	[InjectDependency]
	protected EntityManager _entityManager;

	public override void OnStart ()
	{
		base.OnStart ();

		var e1 = CreateTestObject ();
		var e2 = CreateTestObject ();

		_entityManager.AddComponent (e1, new InputComponent () {
			horizontalAxisName = "Horizontal",
			verticalAxisName = "Vertical",
			jumpActionName = "Jump2"
		});

		_entityManager.AddComponent (e2, new InputComponent () {
			horizontalAxisName = "Horizontal2",
			verticalAxisName = "Vertical2",
			jumpActionName = "Jump"
		});

		_entityManager.GetComponent<PositionComponent> (e1).position = new Vector2(0, 0);
		_entityManager.GetComponent<PositionComponent> (e2).position = new Vector2(0, 2);

		var e3 = CreateTestObjectWithoutMovement ();
		_entityManager.GetComponent<PositionComponent> (e3).position = new Vector2(-2, 3);
	}

	Entity CreateTestObject()
	{
		var e = _entityManager.CreateEntity();

		_entityManager.AddComponent (e, new PositionComponent () { 
			position = new Vector3(0, 0, 0),
			lookingDirection = new Vector2(1, 0)
		});

		_entityManager.AddComponent (e, new MovementComponent () {
			velocity = new Vector2(0, 0),
			speed = 3
		});

		var viewPrefab = GameObject.FindObjectOfType<MyTestSceneController> ().viewPrefab;

		_entityManager.AddComponent (e, new ViewComponent () { 
			viewPrefab = viewPrefab
		});

		_entityManager.AddComponent (e, new JumpComponent () { 
			jumpForce = 100.0f,
			jumpStopFactor = 800.0f
		});

		_entityManager.AddComponent (e, new PhysicsParticleComponent () { 
			maxSpeed = 15,
			maxForce = 20000
		});
				
		_entityManager.AddComponent (e, new ControllerComponent ());

		return e;
	}

	Entity CreateTestObjectWithoutMovement()
	{
		var e = _entityManager.CreateEntity();

		_entityManager.AddComponent (e, new PositionComponent () { 
			position = new Vector3(0, 0, 0),
			lookingDirection = new Vector2(1, 0)
		});

		_entityManager.AddComponent (e, new MovementComponent () {
			velocity = new Vector2(0, 0),
			speed = 0
		});

		var viewPrefab = GameObject.FindObjectOfType<MyTestSceneController> ().viewPrefab;

		_entityManager.AddComponent (e, new ViewComponent () { 
			viewPrefab = viewPrefab
		});

		return e;
	}
}

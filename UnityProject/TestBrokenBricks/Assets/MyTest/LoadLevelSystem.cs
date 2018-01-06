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

		var sceneEntities = GameObject.FindObjectsOfType<EntityTemplateBehaviour> ();

		foreach (var sceneEntity in sceneEntities) {
			var e = _entityManager.CreateEntity ();
			sceneEntity.Apply (e);
			sceneEntity.gameObject.SetActive (false);
		}
	}

	Entity CreateTestObject()
	{
		var e = _entityManager.CreateEntity();

		_entityManager.AddComponent (e, new PositionComponent () { 
			position = new Vector3(0, 0, 0),
			lookingDirection = new Vector2(1, 0)
		});

		_entityManager.AddComponent (e, new MovementComponent () {
			direction = new Vector2(0, 0),
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

		_entityManager.AddComponent (e, new DelegatePhysicsComponent () { 
//			maxSpeed = 15,
			maxForce = 20000
		});

//		GameObject.FindObjectOfType<PhysicsParticleTemplate> ().Apply (e);
				
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
			direction = new Vector2(0, 0),
			speed = 0
		});

		var viewPrefab = GameObject.FindObjectOfType<MyTestSceneController> ().viewPrefab;

		_entityManager.AddComponent (e, new ViewComponent () { 
			viewPrefab = viewPrefab
		});

		return e;
	}
}

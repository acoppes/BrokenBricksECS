using ECS;
using MyTest.Systems;
using UnityEngine;

public class MyTestSceneController : ECSController<UnityStandardSystemRoot, UnityEntityManager> {

	public GameObject viewPrefab;

	protected override void Initialize() {

		#if UNITY_EDITOR
		AddSystem<DebugEntitiesSystem> ();
		#endif

		AddSystem<MovementSystem>();
		AddSystem<ViewSystem>();
		AddSystem<InputSystem> ();
		AddSystem<ControllerSystem> ();
		AddSystem<JumpSystem> ();
		AddSystem<DelegatePhysicsSystem> ();

		AddSystem<LoadLevelSystem> ();
	}
}


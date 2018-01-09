using ECS;
using MyTest.Systems;
using UnityEngine;

public class MyTestSceneController : ECSController<UnityStandardSystemRoot, UnityEntityManager> {

	public GameObject viewPrefab;

	protected override void Initialize() {

		#if UNITY_EDITOR
		AddSystem<DebugEntitiesSystem> ();
		#endif

		AddSystem<MovementPhyisicsSystem>();
		AddSystem<ViewSystem>();

		AddSystem<InputSystem> ();
		AddSystem<IABehaviourSystem> ();

		AddSystem<ControllerSystem> ();
		AddSystem<JumpSystem> ();

		AddSystem<LimitVelocitySystem> ();
		AddSystem<DelegatePhysicsSystem> ();

		AddSystem<LoadLevelSystem> ();
	}
}


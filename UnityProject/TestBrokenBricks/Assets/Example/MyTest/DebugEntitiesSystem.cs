using ECS;
using UnityEngine;
using MyTest.Components;

public class DebugEntitiesSystem : ComponentSystem
{
	class DebugBehaviour : ScriptBehaviour
	{
		[InjectDependency]
		EntityManager _entityManager;

		public int entityId;

		public Entity entity;

		public DelegatePhysicsComponent physics = new DelegatePhysicsComponent();

		void Update()
		{
			if (_entityManager.HasComponent<DelegatePhysicsComponent> (entity)) {
				var physicsComponent = _entityManager.GetComponent<DelegatePhysicsComponent> (entity);
				JsonUtility.FromJsonOverwrite (JsonUtility.ToJson (physicsComponent), physics);
			}			
		}

		public void OnDrawGizmos()
		{
			if (_entityManager.HasComponent<DelegatePhysicsComponent> (entity)) {
				var physicsComponent = _entityManager.GetComponent<DelegatePhysicsComponent> (entity);
				#if UNITY_EDITOR

				var unityPosition = new Vector3(physicsComponent.position.x, physicsComponent.position.z, physicsComponent.position.y);
				var unityVelocity = new Vector3(physicsComponent.velocity.x, physicsComponent.velocity.z, physicsComponent.velocity.y);

				UnityEngine.Gizmos.DrawLine(unityPosition, unityPosition + unityVelocity);

				#endif
			}	
		}
	}

	class DebugComponent : IComponent
	{
		public DebugBehaviour debug;
	}

	[InjectDependency]
	EntityManager _entityManager;

	Transform _entitiesParent;

	public override void OnStart ()
	{
		base.OnStart ();
		_entitiesParent = new GameObject ("~Entities").transform;
	
		_entityManager.SubscribeOnEntityAdded (this);
	}

	public override void OnEntityAdded (object sender, Entity entity)
	{
		// base.OnEntityAdded (sender, entity);

		var entityObject = new GameObject ("Entity-" + entity.Id);
		var debug = entityObject.AddComponent<DebugBehaviour> ();

		debug.entity = entity;
		debug.entityId = entity.Id;

		entityObject.transform.SetParent (_entitiesParent);

		_entityManager.AddComponent (entity, new DebugComponent () {
			debug = debug
		});
	}

	public override void OnEntityRemoved (object sender, Entity entity)
	{
		base.OnEntityRemoved (sender, entity);
		var debugComponent = _entityManager.GetComponent<DebugComponent> (entity);

		if (debugComponent.debug != null) {
			GameObject.Destroy (debugComponent.debug.gameObject);
			debugComponent.debug = null;
		}
	}
}
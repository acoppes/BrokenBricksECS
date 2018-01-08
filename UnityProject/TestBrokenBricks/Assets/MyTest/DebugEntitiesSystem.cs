using ECS;
using UnityEngine;
using MyTest.Components;

public class DebugEntitiesSystem : ComponentSystem, IEntityAddedEventListener, IEntityRemovedEventListener
{
	class DebugBehaviour<T> : ScriptBehaviour where T : IComponent, new()
	{
		[InjectDependency]
		EntityManager _entityManager;

		public Entity entity;

		public T debugComponent = new T();

		bool _serializedOnce;

		protected bool HasComponent()
		{
			return _entityManager.HasComponent<T> (entity);
		}

		protected T GetComponent()
		{
			return _entityManager.GetComponent<T> (entity);
		}

		void SerializeFromEntity()
		{
			if (HasComponent()) {
				var component = GetComponent();
				JsonUtility.FromJsonOverwrite (JsonUtility.ToJson (component), debugComponent);
			}			
		}

		void SerializeToEntity()
		{
			if (HasComponent()) {
				var component = GetComponent ();
				JsonUtility.FromJsonOverwrite (JsonUtility.ToJson (debugComponent), component);
			}			
		}

		void FixedUpdate()
		{
			SerializeFromEntity();		
		}

		void OnValidate()
		{
			// serialize back	
			if (!_serializedOnce) {
				SerializeFromEntity ();
				_serializedOnce = true;
			}
			SerializeToEntity();		
		}
	}
	
	class DelegatePhysicsBehaviour : DebugBehaviour<DelegatePhysicsComponent> 
	{
		#if UNITY_EDITOR

		void OnDrawGizmos()
		{
			if (HasComponent()) {
				var physicsComponent = GetComponent();

				var unityPosition = new Vector3(physicsComponent.position.x, physicsComponent.position.z, physicsComponent.position.y);
				var unityVelocity = new Vector3(physicsComponent.velocity.x, physicsComponent.velocity.z, physicsComponent.velocity.y);

				UnityEngine.Gizmos.DrawLine(unityPosition, unityPosition + unityVelocity);

			}	
		}

		#endif
	}

	class DelegateLimitVelocityBehaviour : DebugBehaviour<LimitVelocityComponent>
	{
		
	}

	class DebugComponent : IComponent
	{
		public DebugBehaviour<DelegatePhysicsComponent> debug;
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

	public void OnEntityAdded (object sender, Entity entity)
	{
		// base.OnEntityAdded (sender, entity);

		var entityObject = new GameObject ("Entity-" + entity.Id);
//		var debug = entityObject.AddComponent<DelegatePhysicsComponent> ();
		var debug = entityObject.AddComponent<DelegatePhysicsBehaviour> ();
		entityObject.AddComponent<DelegateLimitVelocityBehaviour> ().entity = entity;

		debug.entity = entity;

		entityObject.transform.SetParent (_entitiesParent);

		_entityManager.AddComponent (entity, new DebugComponent () {
			debug = debug
		});
	}

	public void OnEntityRemoved (object sender, Entity entity)
	{
		var debugComponent = _entityManager.GetComponent<DebugComponent> (entity);

		if (debugComponent.debug != null) {
			GameObject.Destroy (debugComponent.debug.gameObject);
			debugComponent.debug = null;
		}
	}
}
using ECS;
using UnityEngine;

public class DebugEntitiesSystem : ComponentSystem
{
	class DebugBehaviour : MonoBehaviour
	{
		public int entityId;
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
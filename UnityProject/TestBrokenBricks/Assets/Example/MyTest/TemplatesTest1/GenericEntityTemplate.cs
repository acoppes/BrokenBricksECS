using ECS;
using UnityEngine;
using UnityEngine.Serialization;

public class GenericEntityTemplate<T> : ComponentTemplateBehaviour where T : IComponent
{
	[FormerlySerializedAsAttribute("t")]
	public T component;

	public override void Apply (Entity e)
	{
		var p = JsonUtility.ToJson(component);
		_entityManager.AddComponent(e, JsonUtility.FromJson<T>(p));
	}

}

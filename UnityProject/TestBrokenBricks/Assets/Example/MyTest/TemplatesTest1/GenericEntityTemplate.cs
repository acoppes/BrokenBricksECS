using ECS;
using UnityEngine;

public class GenericEntityTemplate<T> : ComponentTemplateBehaviour where T : IComponent
{
	public T t;

	public override void Apply (Entity e)
	{
		var p = JsonUtility.ToJson(t);
		_entityManager.AddComponent(e, JsonUtility.FromJson<T>(p));
	}

}

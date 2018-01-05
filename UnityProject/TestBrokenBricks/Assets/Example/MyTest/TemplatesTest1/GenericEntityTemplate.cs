using ECS;
using MyTest.Components;
using UnityEngine;

public class GenericEntityTemplate<T> : EntityTemplateBehaviour where T : IComponent
{
	public T t;

	public override void Apply (Entity e)
	{
		var p = JsonUtility.ToJson(t);
		_entityManager.AddComponent(e, JsonUtility.FromJson<T>(p));
	}

}

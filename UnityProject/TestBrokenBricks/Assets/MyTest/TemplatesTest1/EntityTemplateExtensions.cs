using ECS;
using UnityEngine;

public static class EntityTemplateExtensions 
{
	public static void Apply(Entity e, GameObject go)
	{
		var templates = go.GetComponentsInChildren<EntityTemplate> ();
		foreach (var template in templates) {
			template.Apply (e);
		}
	}
}

using ECS;
using UnityEngine;

public class EntityTemplateBehaviour : MonoBehaviour, EntityTemplate
{
	#region EntityTemplate implementation
	public void Apply (Entity e) 
	{
		var componentTemplates = gameObject.GetComponentsInChildren<ComponentTemplateBehaviour> ();
		foreach (var c in componentTemplates) {
			c.Apply (e);
		}
	}
	#endregion

}

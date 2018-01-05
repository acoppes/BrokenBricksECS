using MyTest.Components;
using UnityEngine;

[ExecuteInEditMode]
public class PositionComponentTemplate : GenericEntityTemplate<PositionComponent> {

	#if UNITY_EDITOR
	void Update()
	{
		component.position = new Vector3(transform.position.x, transform.position.z, 0);	
		UnityEditor.EditorUtility.SetDirty (this);
		UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
	}
	#endif

}

using MyTest.Components;
using UnityEngine;

[ExecuteInEditMode]
public class PositionComponentTemplate : GenericEntityTemplate<PositionComponent> {

	#if UNITY_EDITOR
	void Update()
	{
		if (Application.isPlaying)
			return;

		if (component.position != new Vector3 (transform.position.x, transform.position.z, 0)) {
			component.position = new Vector3 (transform.position.x, transform.position.z, 0);	

			UnityEditor.EditorUtility.SetDirty (this);
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty (UnityEngine.SceneManagement.SceneManager.GetActiveScene ());
		}
	}
	#endif

}

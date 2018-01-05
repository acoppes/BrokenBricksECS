using MyTest.Components;
using UnityEngine;

[ExecuteInEditMode]
public class PhysicsParticleTemplate : GenericEntityTemplate<DelegatePhysicsComponent> {

	#if UNITY_EDITOR
	void Update()
	{
		component.position = new UnityEngine.Vector3(transform.position.x, transform.position.z, transform.position.y);	
		UnityEditor.EditorUtility.SetDirty (this);
		UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
	}
	#endif

}

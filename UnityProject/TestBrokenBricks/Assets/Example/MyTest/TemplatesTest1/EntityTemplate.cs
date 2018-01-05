using ECS;
using MyTest.Systems;
using UnityEngine;

public interface EntityTemplate 
{
	void Apply (Entity e);
}

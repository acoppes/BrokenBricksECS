using NUnit.Framework;
using ECS;

public class TestGroupCallbacks {

	class Component1 : IComponent
	{
		
	}

	class Component2 : IComponent
	{
		
	}

	class Component3 : IComponent
	{

	}

	class ListenerMock : IEntityAddedEventListener
	{
		public int timesCalled;

		#region IEntityAddedEventListener implementation
		public void OnEntityAdded (object sender, Entity entity)
		{
			timesCalled++;
		}
		#endregion
		
	}

	[Test]
	public void EntityAddedShouldBeCalledOnceWhenAllComponentsFromGroup() {
		// Use the Assert class to test conditions.

		var entityManager = new EntityManager ();
		var group = entityManager.GetComponentGroup (typeof(Component1), typeof(Component2));

		var listenerMock = new ListenerMock ();
		group.SubscribeOnEntityAdded (listenerMock);

		Assert.That (listenerMock.timesCalled, Is.EqualTo (0));

		var e = entityManager.CreateEntity ();
		entityManager.AddComponent (e, new Component1 ());

		Assert.That (listenerMock.timesCalled, Is.EqualTo (0));

		entityManager.AddComponent (e, new Component2 ());
		Assert.That (listenerMock.timesCalled, Is.EqualTo (1));
	}

	[Test]
	public void EntityAddedToGroupShouldNotBeCalledIfDifferentComponent() {
		// Use the Assert class to test conditions.

		var entityManager = new EntityManager ();
		var group = entityManager.GetComponentGroup (typeof(Component1), typeof(Component2));

		var listenerMock = new ListenerMock ();
		group.SubscribeOnEntityAdded (listenerMock);

		var e = entityManager.CreateEntity ();
		Assert.That (listenerMock.timesCalled, Is.EqualTo (0));
		entityManager.AddComponent (e, new Component1 ());
		Assert.That (listenerMock.timesCalled, Is.EqualTo (0));
		entityManager.AddComponent (e, new Component2 ());
		Assert.That (listenerMock.timesCalled, Is.EqualTo (1));
		entityManager.AddComponent (e, new Component3 ());
		Assert.That (listenerMock.timesCalled, Is.EqualTo (1));
	}


}

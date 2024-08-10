using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using SUL.Adapters;
using NSubstitute;
using System.Linq;

namespace SUL_Tests.Adapters
{
    [TestFixture]
    [Category("Adapters")]
    public class GameObjectAdapterEditorTests
    {
        private const string TEST_OBJ_NAME = "TestObject";
        private const string NEW_OBJ_NAME = "NewObject";
        private IGameObjectAdapter adapter;
        private GameObject testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new GameObject(TEST_OBJ_NAME);
            adapter = new GameObjectAdapter(testObj);
        }

        [TearDown]
        public void TearDown()
        {
            if (testObj != null)
                UnityEngine.Object.DestroyImmediate(testObj);
            testObj = null;
            adapter = null;

            var scene = SceneManager.GetActiveScene();
            EditorSceneManager.CloseScene(scene, true);
        }

        [Test]
        public void G_GameObjectAdapter_W_GetBasicProperties_T_ReturnsCorrectValues()
        {
            // Assert
            Assert.AreEqual(TEST_OBJ_NAME, adapter.name);
            Assert.AreEqual(testObj.activeInHierarchy, adapter.activeInHierachy);
            Assert.AreEqual(testObj.activeSelf, adapter.activeSelf);
            Assert.AreEqual(testObj.isStatic, adapter.isStatic);
            Assert.AreEqual(testObj.layer, adapter.layer);
            Assert.AreEqual(testObj.scene, adapter.scene);
            Assert.AreEqual(testObj.sceneCullingMask, adapter.sceneCullingMask);
            Assert.AreEqual(testObj.tag, adapter.tag);
            Assert.AreEqual(testObj.transform, adapter.transform);
        }

        [Test]
        public void G_GameObjectAdapter_W_SetBasicProperties_T_PropertiesAreSet()
        {
            // Act
            adapter.name = NEW_OBJ_NAME;
            adapter.isStatic = true;
            adapter.layer = 5;
            adapter.tag = "Player";

            // Assert
            Assert.AreEqual(NEW_OBJ_NAME, testObj.name);
            Assert.IsTrue(testObj.isStatic);
            Assert.AreEqual(5, testObj.layer);
            Assert.AreEqual("Player", testObj.tag);
        }

        [Test]
        public void G_GameObjectAdapter_W_AddComponent_T_ComponentIsAdded()
        {
            // Act
            var component = adapter.AddComponent(typeof(BoxCollider));

            // Assert
            Assert.IsNotNull(component);
            Assert.IsInstanceOf<BoxCollider>(component);
            Assert.IsNotNull(testObj.GetComponent<BoxCollider>());
        }

        [Test]
        public void G_GameObjectAdapter_W_GetComponent_T_ReturnsCorrectComponent()
        {
            // Arrange
            testObj.AddComponent<BoxCollider>();

            // Act
            var component1 = adapter.GetComponent<BoxCollider>();
            var component2 = adapter.GetComponent(typeof(BoxCollider));
            var component3 = adapter.GetComponent("BoxCollider");

            // Assert
            Assert.IsNotNull(component1);
            Assert.IsNotNull(component2);
            Assert.IsNotNull(component3);
            Assert.IsInstanceOf<BoxCollider>(component1);
            Assert.IsInstanceOf<BoxCollider>(component2);
            Assert.IsInstanceOf<BoxCollider>(component3);
        }

        [Test]
        public void G_GameObjectAdapter_W_GetComponentInChildren_T_ReturnsCorrectComponent()
        {
            // Arrange
            var childObj = new GameObject("Child");
            childObj.transform.SetParent(testObj.transform);
            childObj.AddComponent<BoxCollider>();

            // Act
            var component1 = adapter.GetComponentInChildren<BoxCollider>();
            var component2 = adapter.GetComponentInChildren(typeof(BoxCollider));
            var component3 = adapter.GetComponentInChildren(typeof(BoxCollider), true);

            // Assert
            Assert.IsNotNull(component1);
            Assert.IsNotNull(component2);
            Assert.IsNotNull(component3);
            Assert.IsInstanceOf<BoxCollider>(component1);
            Assert.IsInstanceOf<BoxCollider>(component2);
            Assert.IsInstanceOf<BoxCollider>(component3);
        }

        [Test]
        public void G_GameObjectAdapter_W_GetComponentInParent_T_ReturnsCorrectComponent()
        {
            // Arrange
            var parentObj = new GameObject("Parent");
            testObj.transform.SetParent(parentObj.transform);
            parentObj.AddComponent<BoxCollider>();
            var childAdapter = new GameObjectAdapter(testObj);

            // Act
            var component1 = childAdapter.GetComponentInParent<BoxCollider>();
            var component2 = childAdapter.GetComponentInParent(typeof(BoxCollider));
            var component3 = childAdapter.GetComponentInParent(typeof(BoxCollider), true);

            // Assert
            Assert.IsNotNull(component1);
            Assert.IsNotNull(component2);
            Assert.IsNotNull(component3);
            Assert.IsInstanceOf<BoxCollider>(component1);
            Assert.IsInstanceOf<BoxCollider>(component2);
            Assert.IsInstanceOf<BoxCollider>(component3);
        }

        [Test]
        public void G_GameObjectAdapter_W_GetComponents_T_ReturnsCorrectComponents()
        {
            // Arrange
            testObj.AddComponent<BoxCollider>();
            testObj.AddComponent<SphereCollider>();

            // Act
            var components1 = adapter.GetComponents<Collider>();
            var components2 = adapter.GetComponents(typeof(Collider));
            var componentsList = new List<Collider>();
            adapter.GetComponents(componentsList);

            // Assert
            Assert.AreEqual(2, components1.Length);
            Assert.AreEqual(2, components2.Length);
            Assert.AreEqual(2, componentsList.Count);
            Assert.IsTrue(components1.Any(c => c is BoxCollider));
            Assert.IsTrue(components1.Any(c => c is SphereCollider));
        }

        [Test]
        public void G_GameObjectAdapter_W_GetComponentsInChildren_T_ReturnsCorrectComponents()
        {
            // Arrange
            var childObj = new GameObject("Child");
            childObj.transform.SetParent(testObj.transform);
            testObj.AddComponent<BoxCollider>();
            childObj.AddComponent<SphereCollider>();

            // Act
            var components1 = adapter.GetComponentsInChildren<Collider>();
            var components2 = adapter.GetComponentsInChildren<Collider>(true);
            var components3 = adapter.GetComponentsInChildren(typeof(Collider), true);
            var componentsList = new List<Collider>();
            adapter.GetComponentsInChildren(componentsList);
            var componentsListInactive = new List<Collider>();
            adapter.GetComponentsInChildren(true, componentsListInactive);

            // Assert
            Assert.AreEqual(2, components1.Length);
            Assert.AreEqual(2, components2.Length);
            Assert.AreEqual(2, components3.Length);
            Assert.AreEqual(2, componentsList.Count);
            Assert.AreEqual(2, componentsListInactive.Count);
        }

        [Test]
        public void G_GameObjectAdapter_W_GetComponentsInParent_T_ReturnsCorrectComponents()
        {
            // Arrange
            var parentObj = new GameObject("Parent");
            testObj.transform.SetParent(parentObj.transform);
            parentObj.AddComponent<BoxCollider>();
            testObj.AddComponent<SphereCollider>();
            var childAdapter = new GameObjectAdapter(testObj);

            // Act
            var components1 = childAdapter.GetComponentsInParent<Collider>();
            var components2 = childAdapter.GetComponentsInParent<Collider>(true);
            var components3 = childAdapter.GetComponentsInParent(typeof(Collider), true);
            var componentsList = new List<Collider>();
            childAdapter.GetComponentsInParent(true, componentsList);

            // Assert
            Assert.AreEqual(2, components1.Length);
            Assert.AreEqual(2, components2.Length);
            Assert.AreEqual(2, components3.Length);
            Assert.AreEqual(2, componentsList.Count);
        }

        [Test]
        public void G_GameObjectAdapter_W_TryGetComponent_T_ReturnsCorrectComponent()
        {
            // Arrange
            testObj.AddComponent<BoxCollider>();

            // Act
            bool success1 = adapter.TryGetComponent<BoxCollider>(out var component1);
            bool success2 = adapter.TryGetComponent(typeof(BoxCollider), out var component2);

            // Assert
            Assert.IsTrue(success1);
            Assert.IsTrue(success2);
            Assert.IsNotNull(component1);
            Assert.IsNotNull(component2);
            Assert.IsInstanceOf<BoxCollider>(component1);
            Assert.IsInstanceOf<BoxCollider>(component2);
        }

        [Test]
        public void G_GameObjectAdapter_W_CompareTag_T_ReturnsCorrectResult()
        {
            // Arrange
            testObj.tag = "Player";

            // Act
            bool result = adapter.CompareTag("Player");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void G_GameObjectAdapter_W_SetActive_T_GameObjectActiveStateChanges()
        {
            // Act
            adapter.SetActive(false);

            // Assert
            Assert.IsFalse(testObj.activeSelf);
        }

        [Test]
        public void G_GameObjectAdapter_W_CreatePrimitive_T_ReturnsCorrectGameObjectAdapter()
        {
            // Act
            var primitiveAdapter = adapter.CreatePrimitive(PrimitiveType.Cube);

            // Assert
            Assert.IsNotNull(primitiveAdapter);
            Assert.IsInstanceOf<IGameObjectAdapter>(primitiveAdapter);
            Assert.IsNotNull(primitiveAdapter.GetComponent<MeshFilter>());
            Assert.IsNotNull(primitiveAdapter.GetComponent<BoxCollider>());
        }

        [Test]
        public void G_GameObjectAdapter_W_Find_T_ReturnsCorrectGameObjectAdapter()
        {
            // Act
            var foundAdapter = adapter.Find(TEST_OBJ_NAME);

            // Assert
            Assert.IsNotNull(foundAdapter);
            Assert.IsInstanceOf<IGameObjectAdapter>(foundAdapter);
            Assert.AreEqual(TEST_OBJ_NAME, foundAdapter.name);
        }

        [Test]
        public void G_GameObjectAdapter_W_FindGameObjectsWithTag_T_ReturnsCorrectGameObjectAdapters()
        {
            // Arrange
            testObj.tag = "TestTag";
            var anotherObj = new GameObject("AnotherObject");
            anotherObj.tag = "TestTag";

            // Act
            var foundAdapters = adapter.FindGameObjectsWithTag("TestTag");

            // Assert
            Assert.IsNotNull(foundAdapters);
            Assert.AreEqual(2, foundAdapters.Length);
            Assert.IsTrue(foundAdapters.All(a => a is IGameObjectAdapter));
            Assert.IsTrue(foundAdapters.Any(a => a.name == TEST_OBJ_NAME));
            Assert.IsTrue(foundAdapters.Any(a => a.name == "AnotherObject"));

            // Clean up
            UnityEngine.Object.DestroyImmediate(anotherObj);
        }

        [Test]
        public void G_GameObjectAdapter_W_FindWithTag_T_ReturnsCorrectGameObjectAdapter()
        {
            // Arrange
            testObj.tag = "TestTag";

            // Act
            var foundAdapter = adapter.FindWithTag("TestTag");

            // Assert
            Assert.IsNotNull(foundAdapter);
            Assert.IsInstanceOf<IGameObjectAdapter>(foundAdapter);
            Assert.AreEqual(TEST_OBJ_NAME, foundAdapter.name);
        }

        [Test]
        public void G_GameObjectAdapter_W_GetScene_T_ReturnsCorrectScene()
        {
            // Arrange
            int instanceID = testObj.GetInstanceID();

            // Act
            var scene = adapter.GetScene(instanceID);

            // Assert
            Assert.AreEqual(testObj.scene, scene);
        }

        [Test]
        public void G_GameObjectAdapter_W_SendMessage_T_MessageIsSent()
        {
            // Arrange
            var mockReceiver = Substitute.For<MonoBehaviour>();
            testObj.AddComponent(mockReceiver.GetType());

            // Act
            adapter.SendMessage("TestMethod", "TestParameter", SendMessageOptions.RequireReceiver);

            // Assert
            mockReceiver.Received().SendMessage("TestMethod", "TestParameter", SendMessageOptions.RequireReceiver);
        }

        [Test]
        public void G_GameObjectAdapter_W_SendMessageUpwards_T_MessageIsSentUpwards()
        {
            // Arrange
            var parentObj = new GameObject("Parent");
            testObj.transform.SetParent(parentObj.transform);
            var mockReceiver = Substitute.For<MonoBehaviour>();
            parentObj.AddComponent(mockReceiver.GetType());

            // Act
            adapter.SendMessageUpwards("TestMethod", "TestParameter", SendMessageOptions.RequireReceiver);

            // Assert
            mockReceiver.Received().SendMessageUpwards("TestMethod", "TestParameter", SendMessageOptions.RequireReceiver);

            // Clean up
            UnityEngine.Object.DestroyImmediate(parentObj);
        }

        [Test]
        public void G_GameObjectAdapter_W_BroadcastMessage_T_MessageIsBroadcasted()
        {
            // Arrange
            var childObj = new GameObject("Child");
            childObj.transform.SetParent(testObj.transform);
            var mockReceiver = Substitute.For<MonoBehaviour>();
            childObj.AddComponent(mockReceiver.GetType());

            // Act
            adapter.BroadCastMessage("TestMethod", "TestParameter", SendMessageOptions.RequireReceiver);

       // Assert
            mockReceiver.Received().BroadcastMessage("TestMethod", "TestParameter", SendMessageOptions.RequireReceiver);

            // Clean up
            UnityEngine.Object.DestroyImmediate(childObj);
        }

        [Test]
        public void G_GameObjectAdapter_W_InstantiateGameObjects_T_GameObjectsAreInstantiated()
        {
            // Arrange
            int sourceInstanceID = testObj.GetInstanceID();
            int count = 3;
            var newInstanceIDs = new Unity.Collections.NativeArray<int>(count, Unity.Collections.Allocator.Temp);
            var newTransformInstanceIDs = new Unity.Collections.NativeArray<int>(count, Unity.Collections.Allocator.Temp);
            int initialObjectCount = UnityEngine.Object.FindObjectsOfType<GameObject>().Length;

            // Act
            adapter.InstantiateGameObjects(sourceInstanceID, count, newInstanceIDs, newTransformInstanceIDs);

            // Assert
            int newObjectCount = UnityEngine.Object.FindObjectsOfType<GameObject>().Length;
            Assert.AreEqual(initialObjectCount + count, newObjectCount, "Expected number of new GameObjects were not created");

            // Verify that the instance IDs in the NativeArrays are valid
            for (int i = 0; i < count; i++)
            {
                Assert.AreNotEqual(0, newInstanceIDs[i], $"Invalid GameObject instance ID at index {i}");
                Assert.AreNotEqual(0, newTransformInstanceIDs[i], $"Invalid Transform instance ID at index {i}");
            }

            // Clean up
            newInstanceIDs.Dispose();
            newTransformInstanceIDs.Dispose();

            // Destroy the newly created objects
            var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            for (int i = initialObjectCount; i < allObjects.Length; i++)
            {
                UnityEngine.Object.DestroyImmediate(allObjects[i]);
            }
        }

        [Test]
        public void G_GameObjectAdapter_W_SetGameObjectsActive_T_GameObjectsActiveStateChanges()
        {
            // Arrange
            var obj1 = new GameObject("Obj1");
            var obj2 = new GameObject("Obj2");
            var instanceIDs = new int[] { obj1.GetInstanceID(), obj2.GetInstanceID() };

            // Act
            adapter.SetGameObjectsActive(instanceIDs, false);

            // Assert
            Assert.IsFalse(obj1.activeSelf);
            Assert.IsFalse(obj2.activeSelf);

            // Clean up
            UnityEngine.Object.DestroyImmediate(obj1);
            UnityEngine.Object.DestroyImmediate(obj2);
        }

        [Test]
        public void G_GameObjectAdapter_W_SetGameObjectsActiveWithNativeArray_T_GameObjectsActiveStateChanges()
        {
            // Arrange
            var obj1 = new GameObject("Obj1");
            var obj2 = new GameObject("Obj2");
            var instanceIDs = new Unity.Collections.NativeArray<int>(new int[] { obj1.GetInstanceID(), obj2.GetInstanceID() }, Unity.Collections.Allocator.Temp);

            // Act
            adapter.SetGameObjectsActive(instanceIDs, false);

            // Assert
            Assert.IsFalse(obj1.activeSelf);
            Assert.IsFalse(obj2.activeSelf);

            // Clean up
            instanceIDs.Dispose();
            UnityEngine.Object.DestroyImmediate(obj1);
            UnityEngine.Object.DestroyImmediate(obj2);
        }
    }
}

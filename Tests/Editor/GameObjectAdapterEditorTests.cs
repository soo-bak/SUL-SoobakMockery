using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Linq;

namespace SUL.Adapters {

[TestFixture]
[Category("Adapters")]
public class GameObjectAdapterEditorTests {
  private const string TEST_GO_NAME = "TestObject";
  private const string NEW_GO_NAME = "NewObject";
  private const string PARENT_PREFIX = "(Parent)";
  private const string CHILD_PREFIX = "(Child)";

  private IGameObjectAdapter goAdapter;
  private GameObject testGo;

  [SetUp]
  public void SetUp() {
    EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

    testGo = new GameObject(TEST_GO_NAME);
    goAdapter = new GameObjectAdapter(testGo);
  }

  [TearDown]
  public void TearDown() {
    if (testGo != null) Object.DestroyImmediate(testGo);
    testGo = null;
    goAdapter = null;

    EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
  }

  [Test]
  public void G_GameObjectAdapter_W_GetBasicProperties_T_ReturnsCorrectValues() {
    // Assert
    Assert.AreEqual(TEST_GO_NAME, goAdapter.name);
    Assert.AreEqual(testGo.activeInHierarchy, goAdapter.activeInHierachy);
    Assert.AreEqual(testGo.activeSelf, goAdapter.activeSelf);
    Assert.AreEqual(testGo.isStatic, goAdapter.isStatic);
    Assert.AreEqual(testGo.layer, goAdapter.layer);
    Assert.AreEqual(testGo.scene, goAdapter.scene);
    Assert.AreEqual(testGo.sceneCullingMask, goAdapter.sceneCullingMask);
    Assert.AreEqual(testGo.tag, goAdapter.tag);
    Assert.AreEqual(testGo.transform, goAdapter.transform);
  }

  [Test]
  public void G_GameObjectAdapter_W_SetBasicProperties_T_PropertiesAreSet() {
    // Act
    goAdapter.name = NEW_GO_NAME;
    goAdapter.isStatic = true;
    goAdapter.layer = 5;
    goAdapter.tag = "Player";

    // Assert
    Assert.AreEqual(NEW_GO_NAME, testGo.name);
    Assert.IsTrue(testGo.isStatic);
    Assert.AreEqual(5, testGo.layer);
    Assert.AreEqual("Player", testGo.tag);
  }

  [Test]
  public void G_GameObjectAdapter_W_AddComponent_T_ComponentIsAdded() {
    // Act
    var component = goAdapter.AddComponent(typeof(BoxCollider));

    // Assert
    Assert.IsNotNull(component);
    Assert.IsInstanceOf<BoxCollider>(component);
    Assert.IsNotNull(testGo.GetComponent<BoxCollider>());
  }

  [Test]
  public void G_GameObjectAdapter_W_GetComponent_T_ReturnsCorrectComponent() {
    // Arrange
    testGo.AddComponent<BoxCollider>();

    // Act
    var component1 = goAdapter.GetComponent<BoxCollider>();
    var component2 = goAdapter.GetComponent(typeof(BoxCollider));
    var component3 = goAdapter.GetComponent("BoxCollider");

    // Assert
    Assert.IsNotNull(component1);
    Assert.IsNotNull(component2);
    Assert.IsNotNull(component3);
    Assert.IsInstanceOf<BoxCollider>(component1);
    Assert.IsInstanceOf<BoxCollider>(component2);
    Assert.IsInstanceOf<BoxCollider>(component3);
  }

  [Test]
  public void G_GameObjectAdapter_W_GetComponentInChildren_T_ReturnsCorrectComponent() {
    // Arrange
    var childObj = new GameObject(CHILD_PREFIX);
    childObj.transform.SetParent(testGo.transform);
    childObj.AddComponent<BoxCollider>();

    // Act
    var component1 = goAdapter.GetComponentInChildren<BoxCollider>();
    var component2 = goAdapter.GetComponentInChildren(typeof(BoxCollider));
    var component3 = goAdapter.GetComponentInChildren(typeof(BoxCollider), true);

    // Assert
    Assert.IsNotNull(component1);
    Assert.IsNotNull(component2);
    Assert.IsNotNull(component3);
    Assert.IsInstanceOf<BoxCollider>(component1);
    Assert.IsInstanceOf<BoxCollider>(component2);
    Assert.IsInstanceOf<BoxCollider>(component3);
  }

  [Test]
  public void G_GameObjectAdapter_W_GetComponentInParent_T_ReturnsCorrectComponent() {
    // Arrange
    var parentObj = new GameObject(PARENT_PREFIX);
    testGo.transform.SetParent(parentObj.transform);
    parentObj.AddComponent<BoxCollider>();
    var childAdapter = new GameObjectAdapter(testGo);

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
  public void G_GameObjectAdapter_W_GetComponents_T_ReturnsCorrectComponents() {
    // Arrange
    testGo.AddComponent<BoxCollider>();
    testGo.AddComponent<SphereCollider>();

    // Act
    var components1 = goAdapter.GetComponents<Collider>();
    var components2 = goAdapter.GetComponents(typeof(Collider));
    var componentsList = new List<Collider>();
    goAdapter.GetComponents(componentsList);

    // Assert
    Assert.AreEqual(2, components1.Length);
    Assert.AreEqual(2, components2.Length);
    Assert.AreEqual(2, componentsList.Count);
    Assert.IsTrue(components1.Any(c => c is BoxCollider));
    Assert.IsTrue(components1.Any(c => c is SphereCollider));
  }

  [Test]
  public void G_GameObjectAdapter_W_GetComponentsInChildren_T_ReturnsCorrectComponents() {
    // Arrange
    var childObj = new GameObject(CHILD_PREFIX);
    childObj.transform.SetParent(testGo.transform);
    testGo.AddComponent<BoxCollider>();
    childObj.AddComponent<SphereCollider>();

    // Act
    var components1 = goAdapter.GetComponentsInChildren<Collider>();
    var components2 = goAdapter.GetComponentsInChildren<Collider>(true);
    var components3 = goAdapter.GetComponentsInChildren(typeof(Collider), true);
    var componentsList = new List<Collider>();
    goAdapter.GetComponentsInChildren(componentsList);
    var componentsListInactive = new List<Collider>();
    goAdapter.GetComponentsInChildren(true, componentsListInactive);

    // Assert
    Assert.AreEqual(2, components1.Length);
    Assert.AreEqual(2, components2.Length);
    Assert.AreEqual(2, components3.Length);
    Assert.AreEqual(2, componentsList.Count);
    Assert.AreEqual(2, componentsListInactive.Count);
  }

  [Test]
  public void G_GameObjectAdapter_W_GetComponentsInParent_T_ReturnsCorrectComponents() {
    // Arrange
    var parentObj = new GameObject(PARENT_PREFIX);
    testGo.transform.SetParent(parentObj.transform);
    parentObj.AddComponent<BoxCollider>();
    testGo.AddComponent<SphereCollider>();
    var childAdapter = new GameObjectAdapter(testGo);

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
  public void G_GameObjectAdapter_W_TryGetComponent_T_ReturnsCorrectComponent() {
    // Arrange
    testGo.AddComponent<BoxCollider>();

    // Act
    bool success1 = goAdapter.TryGetComponent<BoxCollider>(out var component1);
    bool success2 = goAdapter.TryGetComponent(typeof(BoxCollider), out var component2);

    // Assert
    Assert.IsTrue(success1);
    Assert.IsTrue(success2);
    Assert.IsNotNull(component1);
    Assert.IsNotNull(component2);
    Assert.IsInstanceOf<BoxCollider>(component1);
    Assert.IsInstanceOf<BoxCollider>(component2);
  }

  [Test]
  public void G_GameObjectAdapter_W_CompareTag_T_ReturnsCorrectResult() {
    // Arrange
    testGo.tag = "Player";

    // Act
    bool result = goAdapter.CompareTag("Player");

    // Assert
    Assert.IsTrue(result);
  }

  [Test]
  public void G_GameObjectAdapter_W_SetActive_T_GameObjectActiveStateChanges() {
    // Act
    goAdapter.SetActive(false);

    // Assert
    Assert.IsFalse(testGo.activeSelf);
  }

  [Test]
  public void G_GameObjectAdapter_W_CreatePrimitive_T_ReturnsCorrectGameObjectAdapter() {
    // Act
    var primitiveAdapter = goAdapter.CreatePrimitive(PrimitiveType.Cube);

    // Assert
    Assert.IsNotNull(primitiveAdapter);
    Assert.IsInstanceOf<IGameObjectAdapter>(primitiveAdapter);
    Assert.IsNotNull(primitiveAdapter.GetComponent<MeshFilter>());
    Assert.IsNotNull(primitiveAdapter.GetComponent<BoxCollider>());
  }

  [Test]
  public void G_GameObjectAdapter_W_Find_T_ReturnsCorrectGameObjectAdapter() {
    // Act
    var foundAdapter = goAdapter.Find(TEST_GO_NAME);

    // Assert
    Assert.IsNotNull(foundAdapter);
    Assert.IsInstanceOf<IGameObjectAdapter>(foundAdapter);
    Assert.AreEqual(TEST_GO_NAME, foundAdapter.name);
  }

  [Test]
  public void G_GameObjectAdapter_W_GetScene_T_ReturnsCorrectScene() {
    // Arrange
    int instanceID = testGo.GetInstanceID();

    // Act
    var scene = goAdapter.GetScene(instanceID);

    // Assert
    Assert.AreEqual(testGo.scene, scene);
  }

  [Test]
  public void G_GameObjectAdapter_W_InstantiateGameObjects_T_GameObjectsAreInstantiated() {
    // Arrange
    int sourceInstanceID = testGo.GetInstanceID();
    int count = 3;
    var newInstanceIDs = new Unity.Collections.NativeArray<int>(count, Unity.Collections.Allocator.Temp);
    var newTransformInstanceIDs = new Unity.Collections.NativeArray<int>(count, Unity.Collections.Allocator.Temp);
    int initialObjectCount = Object.FindObjectsOfType<GameObject>().Length;

    // Act
    goAdapter.InstantiateGameObjects(sourceInstanceID, count, newInstanceIDs, newTransformInstanceIDs);

    // Assert
    int newObjectCount = Object.FindObjectsOfType<GameObject>().Length;
    Assert.AreEqual(initialObjectCount + count, newObjectCount, "Expected number of new GameObjects were not created");

    for (int i = 0; i < count; i++) {
      Assert.AreNotEqual(0, newInstanceIDs[i], $"Invalid GameObject instance ID at index {i}");
      Assert.AreNotEqual(0, newTransformInstanceIDs[i], $"Invalid Transform instance ID at index {i}");
    }

    // Clean up
    newInstanceIDs.Dispose();
    newTransformInstanceIDs.Dispose();
    
    var allObjects = Object.FindObjectsOfType<GameObject>();
    for (int i = initialObjectCount; i < allObjects.Length; i++)
        Object.DestroyImmediate(allObjects[i]);
  }

  [Test]
  public void G_GameObjectAdapter_W_SetGameObjectsActive_T_GameObjectsActiveStateChanges() {
    // Arrange
    var obj1 = new GameObject("Obj1");
    var obj2 = new GameObject("Obj2");
    var instanceIDs = new int[] { obj1.GetInstanceID(), obj2.GetInstanceID() };

    // Act
    goAdapter.SetGameObjectsActive(instanceIDs, false);

    // Assert
    Assert.IsFalse(obj1.activeSelf);
    Assert.IsFalse(obj2.activeSelf);

    // Clean up
    Object.DestroyImmediate(obj1);
    Object.DestroyImmediate(obj2);
  }

  [Test]
  public void G_GameObjectAdapter_W_SetGameObjectsActiveWithNativeArray_T_GameObjectsActiveStateChanges() {
    // Arrange
    var obj1 = new GameObject("Obj1");
    var obj2 = new GameObject("Obj2");
    var instanceIDs = new Unity.Collections.NativeArray<int>(new int[] { obj1.GetInstanceID(), obj2.GetInstanceID() }, Unity.Collections.Allocator.Temp);

    // Act
    goAdapter.SetGameObjectsActive(instanceIDs, false);

    // Assert
    Assert.IsFalse(obj1.activeSelf);
    Assert.IsFalse(obj2.activeSelf);

    // Clean up
    instanceIDs.Dispose();
    Object.DestroyImmediate(obj1);
    Object.DestroyImmediate(obj2);
  }
}

}  // namespace
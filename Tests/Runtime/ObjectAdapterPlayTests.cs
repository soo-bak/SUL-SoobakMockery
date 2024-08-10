using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Collections;
using System.Linq;
using SUL.TestUtilities;

namespace SUL.Adapters {

[TestFixture]
[Category("Adapters")]
public class ObjectAdapterPlayTests {
  const string INACTIVE_GOBJ_NAME = "InactiveObject";
  const string ACTIVE_GOBJ_NAME = "ActiveObject";
  const string TEST_OBJ_NAME = "TestObject";
  const string PARENT_OBJ_NAME = "Parent";

  private class TestComponent : MonoBehaviour { }

  Vector3 INST_POS = new (1, 2, 3);
  Quaternion INST_ROT = Quaternion.Euler(30, 60, 90);

  IObjectAdapter adapter = null;
  GameObject testObj = null;

  [UnitySetUp]
  public IEnumerator SetUp() {
    testObj = new GameObject(TEST_OBJ_NAME);
    adapter = new ObjectAdapter(testObj);
    yield return null;
  }

  [UnityTearDown]
  public IEnumerator TearDown() {
    if (testObj != null)
      UnityEngine.Object.Destroy(testObj);
    testObj = null;

    if (adapter != null)
      UnityEngine.Object.DestroyImmediate(adapter as UnityEngine.Object);
    adapter = null;

    var allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    foreach (var obj in allObjects)
      UnityEngine.Object.DestroyImmediate(obj);

    yield return null;
  }

  [UnityTest]
  public IEnumerator G_ObjectExists_W_DestroyCalledWithDelay_T_ObjectIsDestroyedAfterDelay() {
    // Arrange
    const float delaySec = 0.1f;
    const float afterDelaySec = 0.2f;

    // Act
    adapter.Destroy(adapter, delaySec);

    // Assert
    Assert.IsFalse(testObj == null);
    yield return new WaitForSeconds(afterDelaySec);

    Assert.IsTrue(testObj == null);
  }

  [UnityTest]
  public IEnumerator G_ObjectExist_W_DontDestroyOnLoadCalledAndSceneTransitions_T_ObjectPersists() {
    // Act
    adapter.DontDestroyOnLoad(adapter);
    yield return TestHelper.SimulateSceneTransition();

    // Assert
    Assert.IsFalse(testObj == null);
  }

  [UnityTest]
  public IEnumerator G_InactiveObjectWithComponent_W_FindObjectOfTypeCalledWithIncludeInactive_T_ReturnsCorrectComponent() {
    // Arrange
    var inactiveObj = new GameObject(INACTIVE_GOBJ_NAME);
    inactiveObj.SetActive(false);
    var testCmp = inactiveObj.AddComponent<TestComponent>();

    // Act
    var foundCmp = adapter.FindObjectOfType<TestComponent>(true);
    yield return null;

    // Assert
    Assert.IsNotNull(foundCmp);
    Assert.AreEqual(testCmp, foundCmp);

    // CleanUp
    UnityEngine.Object.Destroy(inactiveObj);
  }

  [UnityTest]
  public IEnumerator G_InactiveObjectWithComponent_W_FindObjectOfTypeCalledWithTypeAndIncludeInactive_T_ReturnsCorrectObjectAdapter() {
    // Arrange
    var inactiveObj = new GameObject(INACTIVE_GOBJ_NAME);
    inactiveObj.SetActive(false);
    inactiveObj.AddComponent<TestComponent>();
    yield return null;

    // Act
    var foundObj = adapter.FindObjectOfType(typeof(TestComponent), true);
    yield return null;

    // Assert
    Assert.IsNotNull(foundObj);
    Assert.IsInstanceOf<IObjectAdapter>(foundObj);
    Assert.AreEqual(inactiveObj.name, foundObj.name);

    // CleanUp
    UnityEngine.Object.Destroy(inactiveObj);
  }

  [UnityTest]
  public IEnumerator G_ActiveAndInactiveObjectsWithComponent_W_FindObjectsOfTypeCalledWithIncludeInactive_T_ReturnsAllComponents() {
    // Arrange
    var activeObj = new GameObject(ACTIVE_GOBJ_NAME);
    var inactiveObj = new GameObject(INACTIVE_GOBJ_NAME);
    inactiveObj.SetActive(false);

    var testCmp_1 = activeObj.AddComponent<TestComponent>();
    var testCmp_2 = inactiveObj.AddComponent<TestComponent>();

    // Act
    var foundCmps = adapter.FindObjectsOfType<TestComponent>(true);
    yield return null;

    // Assert
    Assert.IsNotNull(foundCmps);
    Assert.AreEqual(2, foundCmps.Length);
    Assert.IsTrue(foundCmps.Contains(testCmp_1));
    Assert.IsTrue(foundCmps.Contains(testCmp_2));

    // CleanUp
    UnityEngine.Object.Destroy(activeObj);
    UnityEngine.Object.Destroy(inactiveObj);
  }

  [UnityTest]
  public IEnumerator G_ActiveAndInactiveObjectsWithComponent_W_FindObjectsOfTypeCalledWithTypeAndIncludeInactive_T_ReturnsAllObjectAdapters() {
    // Arrange
    var activeObj = new GameObject(ACTIVE_GOBJ_NAME);
    var inactiveObj = new GameObject(INACTIVE_GOBJ_NAME);
    inactiveObj.SetActive(false);

    activeObj.AddComponent<TestComponent>();
    inactiveObj.AddComponent<TestComponent>();

    // Act
    var foundObjs = adapter.FindObjectsOfType(typeof(TestComponent), true);
    yield return null;

    // Assert
    Assert.IsNotNull(foundObjs);
    Assert.AreEqual(2, foundObjs.Length);
    Assert.IsTrue(foundObjs.All(obj => obj is IObjectAdapter));
    Assert.IsTrue(foundObjs.Any(obj => obj.name == ACTIVE_GOBJ_NAME));
    Assert.IsTrue(foundObjs.Any(obj => obj.name == INACTIVE_GOBJ_NAME));

    // CleanUp
    UnityEngine.Object.Destroy(activeObj);
    UnityEngine.Object.Destroy(inactiveObj);
  }

  [UnityTest]
  public IEnumerator G_ObjectAdapter_W_InstantiateCalled_T_ReturnsNewObjectAdapter() {
    // Act
    var newAdapter = adapter.Instantiate(adapter);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    Assert.IsInstanceOf<IObjectAdapter>(newAdapter);
    Assert.AreNotEqual(adapter.GetInstanceID(), newAdapter.GetInstanceID());

    // CleanUp
    adapter.Destroy(newAdapter);
  }

  [UnityTest]
  public IEnumerator G_ObjectAdapter_W_InstantiateCalledWithParent_T_ReturnsNewObjectAdapterWithParent() {
    // Arrange
    var parentObj = new GameObject(PARENT_OBJ_NAME);

    // Act
    var newAdapter = adapter.Instantiate(adapter, parentObj.transform);
    yield return null;

    // Assert
    var newGameObject = GameObject.Find(newAdapter.name);
    Assert.IsNotNull(newAdapter);
    Assert.AreEqual(parentObj.transform, newGameObject.transform.parent);

    // CleanUp
    UnityEngine.Object.Destroy(parentObj);
    adapter.Destroy(newAdapter);
    GameObject.DestroyImmediate(newGameObject);
  }

  [UnityTest]
  public IEnumerator G_ObjectAdapter_W_InstantiateWithParentAndWorldSpace_T_ReturnsNewObjectAdapterWithCorrectParent() {
    // Arrange
    var parentObj = new GameObject(PARENT_OBJ_NAME);
    var newAdapter = adapter.Instantiate(adapter, parentObj.transform, false);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    var newGameObject = GameObject.Find(newAdapter.name);
    Assert.IsNotNull(newGameObject);
    Assert.AreEqual(parentObj.transform, newGameObject.transform.parent);

    // CleanUp
    UnityEngine.Object.Destroy(parentObj);
    adapter.Destroy(newAdapter);
    GameObject.DestroyImmediate(newGameObject);
  }

  [UnityTest]
  public IEnumerator G_ObjectAdapter_W_InstantiateWithPositionAndRotation_T_ReturnsNewObjectAdapterWithCorrectTransform() {
    // Act
    var newAdapter = adapter.Instantiate(adapter, INST_POS, INST_ROT);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    var newGameObject = GameObject.Find(newAdapter.name);
    Assert.IsNotNull(newGameObject);
    Assert.AreEqual(INST_POS, newGameObject.transform.position);
    Assert.AreEqual(INST_ROT.ToString(), newGameObject.transform.rotation.ToString());

    // CleanUp
    adapter.Destroy(newAdapter);
    GameObject.DestroyImmediate(newGameObject);
  }

  [UnityTest]
  public IEnumerator G_ObjectAdapter_W_InstantiateWithPositionRotationAndParent_T_ReturnsNewObjectAdapterWithCorrectTransformAndParent() {
    // Arrange
    var parentObj = new GameObject(PARENT_OBJ_NAME);
    
    // Act
    var newAdapter = adapter.Instantiate(adapter, INST_POS, INST_ROT, parentObj.transform);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    var newGameObject = GameObject.Find(newAdapter.name);
    Assert.IsNotNull(newGameObject);
    Assert.AreEqual(INST_POS, newGameObject.transform.localPosition);
    Assert.AreEqual(INST_ROT.ToString(), newGameObject.transform.localRotation.ToString());
    Assert.AreEqual(parentObj.transform, newGameObject.transform.parent);

    // CleanUp
    UnityEngine.Object.Destroy(parentObj);
    adapter.Destroy(newAdapter);
    GameObject.DestroyImmediate(newGameObject);
  }
}

}
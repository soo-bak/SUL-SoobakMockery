using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Linq;

namespace SUL.Adapters {

[TestFixture]
[Category("Adapters")]
public class ObjectAdapterEditorTests {
  const string TEST_OBJ_NAME = "TestObject";
  const string NEW_OBJ_NAME = "NewObject";

  IObjectAdapter adapter = null;
  GameObject testObj = null;

  [SetUp]
  public void SetUp() {
    EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

    testObj = new GameObject(TEST_OBJ_NAME);
    adapter = new ObjectAdapter(testObj);
  }

  [TearDown]
  public void TearDown() {
    if (testObj != null)
      Object.DestroyImmediate(testObj);
    testObj = null;

    if (adapter != null)
      Object.DestroyImmediate(adapter as Object);
    adapter = null;

    EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
  }

  [Test]
  public void G_ObjectAdapter_W_GetNameAndHideFlags_T_ReturnsCorrectValues() {
    // Act
    adapter.name = NEW_OBJ_NAME;
    adapter.hideFlags = HideFlags.HideInHierarchy;

    // Assert
    Assert.AreEqual(NEW_OBJ_NAME, adapter.name);
    Assert.AreEqual(HideFlags.HideInHierarchy, adapter.hideFlags);
  }

  [Test]
  public void G_ObjectAdapter_W_GetInstanceIDAndToString_T_ReturnsCorrectValues() {
    // Assert
    Assert.AreEqual(testObj.GetInstanceID(), adapter.GetInstanceID());
    Assert.AreEqual(testObj.ToString(), adapter.ToString());
  }

  [Test]
  public void G_ObjectExists_W_DestroyImmediateCalled_T_ObjectIsDestroyedImmediately() {
    // Act
    adapter.DestroyImmediate(adapter);

    // Assert
    Assert.IsTrue(testObj == null);
  }

  [Test]
  public void G_ObjectAdapter_W_FindObjectOfTypeGeneric_T_ReturnsCorrectType() {
    // Act
    var foundObj = adapter.FindObjectOfType<GameObject>();

    // Assert
    Assert.IsNotNull(foundObj);
    Assert.IsInstanceOf<GameObject>(foundObj);
  }

  [Test]
  public void G_ObjectAdapter_W_FindObjectOfTypeWithType_T_ReturnsCorrectObjectAdapter() {
    // Act
    var foundObj = adapter.FindObjectOfType(typeof(GameObject));

    // Assert
    Assert.IsNotNull(foundObj);
    Assert.IsInstanceOf<IObjectAdapter>(foundObj);
  }

  [Test]
  public void G_ObjectAdapter_W_FindObjectsOfTypeWithType_T_ReturnsArrayOfObjectAdapters() {
    // Act
    var foundObjs = adapter.FindObjectsOfType(typeof(GameObject));

    // Assert
    Assert.IsNotNull(foundObjs);
    Assert.IsTrue(foundObjs.Length > 0);
    Assert.IsTrue(foundObjs.All(obj => obj is IObjectAdapter));
  }

  [Test]
  public void G_ObjectAdapter_W_FindObjectsOfTypeGeneric_T_ReturnsArrayOfCorrectType() {
    // Act
    var foundObjs = adapter.FindObjectsOfType<GameObject>();

    // Assert
    Assert.IsNotNull(foundObjs);
    Assert.IsTrue(foundObjs.Length > 0);
    Assert.IsTrue(foundObjs.All(obj => obj is GameObject));
  }

  [Test]
  public void G_NonObjectAdapter_W_InstantiateMethods_T_ReturnsNull() {
    // Arrange
    var nonAdapter = new object() as IObjectAdapter;

    // Act & Assert
    Assert.IsNull(adapter.Instantiate(nonAdapter));
    Assert.IsNull(adapter.Instantiate(nonAdapter, new GameObject().transform));
    Assert.IsNull(adapter.Instantiate(nonAdapter, new GameObject().transform, false));
    Assert.IsNull(adapter.Instantiate(nonAdapter, Vector3.zero, Quaternion.identity));
    Assert.IsNull(adapter.Instantiate(nonAdapter, Vector3.zero, Quaternion.identity, new GameObject().transform));
  }
}

} // namespace

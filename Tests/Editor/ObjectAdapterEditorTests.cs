using NSubstitute;
using NUnit.Framework;
using SUL.Adapters;
using System.Collections;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SUL_Tests.Adapters {

[TestFixture]
[Category("Adapters")]
public class ObjectAdapterEditorTests {
  private IObjectAdapter adapter;
  private Object testObj;

  [SetUp]
  public void SetUp() {
    testObj = new Object();
    adapter = new ObjectAdapter(testObj);
  }

  [TearDown]
  public void TearDown() {
    if (testObj != null) Object.Destroy(testObj);
    testObj = null;

    if (adapter != null) Object.DestroyImmediate(adapter as Object);
    adapter = null;

    var curScene = SceneManager.GetActiveScene();
    EditorSceneManager.CloseScene(curScene, true);
  }

  [Test]
  public void GetInstanceID_ReturnsCorrectID() {
    // Arrange
    var expectedID = testObj.GetInstanceID();

    // Act
    var result = adapter.GetInstanceID();

    // Assert
    Assert.AreEqual(expectedID, result);
  }

  [Test]
  public void Name_GetSet_WorksCorrectly() {
    // Arrange
    var testName = "soobak";

    // Act
    adapter.Name = testName;

    // Assert
    Assert.AreEqual(testName, adapter.Name);
  }

  [Test]
  public void HideFlags_GetSet_WorksCorrectly() {
    // Act
    var testHidFlag = HideFlags.HideInHierarchy;
    adapter.HideFlags = testHidFlag;

    // Act & Assert
    Assert.AreEqual(testHidFlag, adapter.HideFlags);
  }

  [Test]
  public void ToString_ReturnsCorrectString() {
    // Arrange
    var testObjName = "soobakObj";
    testObj = new Object {
      name = testObjName
    };

    adapter = new ObjectAdapter(testObj);

    // Act && Assert
    Assert.AreEqual(testObjName, adapter.ToString());
  }

  [Test]
  public void DestroyImmediate_CallsDestroyImmediateMethod() {
    // Act
    adapter.DestroyImmediate(adapter, true);

    // Assert
    Assert.IsTrue(adapter == null);
  }

  [Test]
  public void FindObjectOfType_GenericMethod_ReturnsCorrectObject() {
    // Arrange
    var testCmp = new GameObject().AddComponent<TestComponent>();

    // Act
    var foundCmp = adapter.FindObjectOfType<TestComponent>();

    // Assert
    Assert.IsNotNull(foundCmp);
    Assert.AreEqual(testCmp, foundCmp);

    // Cleanup
    Object.DestroyImmediate(testCmp.gameObject);
  }

  [Test]
  public void FindObjectOfType_WithType_ReturnsCorrectObject() {
    // Arrange
    var testCmp = new GameObject().AddComponent<TestComponent>();

    // Act
    var foundObj = adapter.FindObjectOfType(typeof(TestComponent));

    // Assert
    Assert.IsFalse(foundObj == null);
    Assert.IsInstanceOf<IObjectAdapter>(foundObj);
    Assert.AreEqual(testCmp.name, foundObj.Name);

    // Cleanup
    Object.DestroyImmediate(testCmp.gameObject);
  }

  [Test]
  public void FindObjectsOfType_GenericMethod_ReturnsCorrectObjects() {
    // Arrange
    var testCmp1 = new GameObject().AddComponent<TestComponent>();
    var testCmp2 = new GameObject().AddComponent<TestComponent>();

    // Act
    var results = adapter.FindObjectsOfType<TestComponent>();

    // Assert
    Assert.IsNotNull(results);
    Assert.AreEqual(2, results.Length);
    Assert.IsTrue(results.Contains(testCmp1));
    Assert.IsTrue(results.Contains(testCmp2));

    // Cleanup
    Object.DestroyImmediate(testCmp1.gameObject);
    Object.DestroyImmediate(testCmp2.gameObject);
  }

  [Test]
  public void FindObjectsOfType_WithType_ReturnsCorrectObjects() {
    // Arrange
    var testCmp1 = new GameObject().AddComponent<TestComponent>();
    var testCmp2 = new GameObject().AddComponent<TestComponent>();

    // Act
    var results = adapter.FindObjectsOfType(typeof(TestComponent));

    // Assert
    Assert.IsNotNull(results);
    Assert.AreEqual(2, results.Length);
    Assert.IsTrue(results.All(r => r is IObjectAdapter));
    Assert.IsTrue(results.Any(r => r.Name == testCmp1.name));
    Assert.IsTrue(results.Any(r => r.Name == testCmp2.name));

    // Cleanup
    Object.DestroyImmediate(testCmp1.gameObject);
    Object.DestroyImmediate(testCmp2.gameObject);
  }

  [Test]
  public void Instantiate_CreatesNewObject() {
    // Arrange
    var mockAdapter = Substitute.For<IObjectAdapter>();

    // Act
    var result = adapter.Instantiate(mockAdapter);

    // Assert
    Assert.IsNotNull(result);
    Assert.IsInstanceOf<IObjectAdapter>(result);
    Assert.AreEqual(mockAdapter, result);
  }

  [Test]
  public void Instantiate_WithParent_CreatesNewObjectWithParent() {
    // Arrange
    var mockOriginal = Substitute.For<IObjectAdapter>();
    var mockParent = Substitute.For<Transform>();

    // Act
    var result = adapter.Instantiate(mockOriginal, mockParent);

    // Assert
    Assert.IsNotNull(result);
    Assert.IsInstanceOf<IObjectAdapter>(result);
    Assert.AreEqual(mockParent.GetComponentInChildren<IObjectAdapter>(), result);
  }

  [Test]
  public void Instantiate_WithParentAndWorldPositionStays_CreatesNewObjectCorrectly() {
      // Arrange
      var mockOriginal = Substitute.For<IObjectAdapter>();
    var mockInstantiated = Substitute.For<IObjectAdapter>();
    var mockParent = Substitute.For<Transform>();
    adapter.Instantiate(mockOriginal, mockParent, true).Returns(mockInstantiated);

    var result = adapter.Instantiate(mockOriginal, mockParent, true);

    Assert.IsNotNull(result);
    Assert.IsInstanceOf<IObjectAdapter>(result);
  }

  [Test]
  public void Instantiate_WithPosition_CreatesNewObjectAtPosition() {
    // Arrange
    var mockOriginal = Substitute.For<IObjectAdapter>();
    var mockInstantiated = Substitute.For<IObjectAdapter>();
    var position = new Vector3(1, 2, 3);
    var rotation = Quaternion.Euler(30, 60, 90);
    adapter.Instantiate(mockOriginal, position, rotation).Returns(mockInstantiated);

    var result = adapter.Instantiate(mockOriginal, position, rotation);

    Assert.IsNotNull(result);
    Assert.IsInstanceOf<IObjectAdapter>(result);
  }

  [Test]
  public void Instantiate_WithPositionAndParent_CreatesNewObjectCorrectly() {
    // Arrange
    var mockOriginal = Substitute.For<IObjectAdapter>();
    var mockInstantiated = Substitute.For<IObjectAdapter>();
    var position = new Vector3(1, 2, 3);
    var rotation = Quaternion.Euler(30, 60, 90);
    var mockParent = Substitute.For<Transform>();
    adapter.Instantiate(mockOriginal, position, rotation, mockParent).Returns(mockInstantiated);

    var result = adapter.Instantiate(mockOriginal, position, rotation, mockParent);

    Assert.IsNotNull(result);
    Assert.IsInstanceOf<IObjectAdapter>(result);
  }

  private class TestComponent : MonoBehaviour { }
}

} // namespace

using NSubstitute;
using NUnit.Framework;
using SUL.Adapters;
using System.Linq;
using UnityEngine;

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
    if (testObj != null)
      Object.Destroy(testObj);
    testObj = null;

    adapter?.DestroyImmediate(adapter);
    adapter = null;
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
}

} // namespace

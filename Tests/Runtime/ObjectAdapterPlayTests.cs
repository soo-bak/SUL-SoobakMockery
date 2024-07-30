using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using SUL.Adapters;
using System.Collections;
using System.Linq;

namespace SUL_Tests.Adapters {

[TestFixture]
[Category("Adapters")]
public class ObjectAdapterPlayTests {
  const string INACTIVE_GOBJ_NAME = "Inactive";
  const string ACTIVE_GOBJ_NAME = "Active";

  private IObjectAdapter adapter = null;
  private Object testObj = null;

  [UnitySetUp]
  public IEnumerator SetUp() {
    testObj = new Object();
    adapter = new ObjectAdapter(testObj);

    yield return null;
  }

  [UnityTearDown]
  public IEnumerator TearDown() {
    if (testObj != null) Object.Destroy(testObj);
    testObj = null;

    if (adapter != null) Object.Destroy(adapter as Object);
    adapter = null;

    yield return null;
  }

  [UnityTest]
  public IEnumerator Destroy_DestroysObjectAfterDelay() {
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
  public IEnumerator DontDestroyOnLoad_KeepsObjectAcrossSceneLoads() {
    // Act
    adapter.DontDestroyOnLoad(adapter);
    yield return SimulateSceneTransition();

    // Assert
    Assert.IsNotNull(adapter);
    Assert.IsFalse(adapter == null);
  }

  [UnityTest]
  public IEnumerator FindObjectOfType_GenericMethod_IncludeInactive_ReturnsCorrectObject() {
    // Arrange
    var inactiveObj = new GameObject(INACTIVE_GOBJ_NAME);
    inactiveObj.SetActive(false);

    var testCmp = inactiveObj.AddComponent<TestComponent>();

    // Act
    var foundCmp = adapter.FindObjectOfType<TestComponent>(true);

    // Assert
    Assert.IsNotNull(foundCmp);
    Assert.AreEqual(testCmp, foundCmp);

    // Cleanup
    Object.Destroy(inactiveObj);
    yield return null;
  }

  [UnityTest]
  public IEnumerator FindObjectOfType_WithType_IncludeInactive_ReturnsCorrectObject()
  {
    // Arrange
    var inactiveObj = new GameObject(INACTIVE_GOBJ_NAME);
    inactiveObj.SetActive(false);

    inactiveObj.AddComponent<TestComponent>();

    // Act
    var foundObj = adapter.FindObjectOfType(typeof(TestComponent), true);
    yield return null;

    // Assert
    Assert.IsNotNull(foundObj);
    Assert.IsInstanceOf<IObjectAdapter>(foundObj);
    Assert.AreEqual(inactiveObj, foundObj);

    // Cleanup
    Object.Destroy(inactiveObj);
    yield return null;
  }

  [UnityTest]
  public IEnumerator FindObjectsOfType_GenericMethod_IncludeInactive_ReturnsCorrectObjects()
  {
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

    // Cleanup
    Object.Destroy(activeObj);
    Object.Destroy(inactiveObj);
    yield return null;
  }

  [UnityTest]
  public IEnumerator FindObjectsOfType_WithType_IncludeInactive_ReturnsCorrectObjects()
  {
    // Arrange
    var activeObj = new GameObject(ACTIVE_GOBJ_NAME);
    var inactiveObj = new GameObject(INACTIVE_GOBJ_NAME);
    inactiveObj.SetActive(false);

    var testCmp_1 = activeObj.AddComponent<TestComponent>();
    var testCmp_2 = inactiveObj.AddComponent<TestComponent>();

    // Act
    var foundObjs = adapter.FindObjectsOfType(typeof(TestComponent), true);
    yield return null;

    // Assert
    Assert.IsNotNull(foundObjs);
    Assert.AreEqual(2, foundObjs.Length);
    Assert.IsTrue(foundObjs.All(r => r is IObjectAdapter));
    Assert.IsTrue(foundObjs.Any(r => r.Name == testCmp_1.name));
    Assert.IsTrue(foundObjs.Any(r => r.Name == testCmp_2.name));

    // Cleanup
    Object.Destroy(activeObj);
    Object.Destroy(inactiveObj);
    yield return null;
  }

  private class TestComponent : MonoBehaviour { }

    private IEnumerator SimulateSceneTransition() {
    var originScene = SceneManager.GetActiveScene();

    var tempSceneName = "TempScene";
    var tempScene = SceneManager.CreateScene(tempSceneName);

    yield return SceneManager.SetActiveScene(tempScene);

    yield return SceneManager.UnloadSceneAsync(tempScene);

    yield return SceneManager.SetActiveScene(originScene);
  }
}

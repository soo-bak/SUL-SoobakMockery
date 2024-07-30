using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using SUL.Adapters;
using System.Collections;

namespace SUL_Tests.Adapters {

[TestFixture]
[Category("Adapters")]
public class ObjectAdapterPlayTests {
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
    if (testObj != null)
      Object.Destroy(testObj);
    testObj = null;

    adapter?.Destroy(adapter);
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

  private IEnumerator SimulateSceneTransition() {
    var originScene = SceneManager.GetActiveScene();

    var tempSceneName = "TempScene";
    var tempScene = SceneManager.CreateScene(tempSceneName);

    yield return SceneManager.SetActiveScene(tempScene);

    yield return SceneManager.UnloadSceneAsync(tempScene);

    yield return SceneManager.SetActiveScene(originScene);
  }
}

}// namesapce

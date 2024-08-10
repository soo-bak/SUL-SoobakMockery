using System.Collections;
using UnityEngine.SceneManagement;

namespace SUL.TestUtilities {

public static class TestHelper {
  public static IEnumerator SimulateSceneTransition() {
    var originScene = SceneManager.GetActiveScene();
    var tmpSceneName = "TempScene";
    var tmpScene = SceneManager.CreateScene(tmpSceneName);
    yield return SceneManager.SetActiveScene(tmpScene);
    yield return SceneManager.UnloadSceneAsync(tmpScene);
    yield return SceneManager.SetActiveScene(originScene);
  }
}

} // namespace

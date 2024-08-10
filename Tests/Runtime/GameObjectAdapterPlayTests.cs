using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using SUL.Adapters;

namespace SUL_Tests.Adapters
{
    [TestFixture]
    [Category("Adapters")]
    public class GameObjectAdapterPlayTests
    {
        private const string TEST_OBJ_NAME = "TestObject";
        private IGameObjectAdapter adapter;
        private GameObject testObj;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            testObj = new GameObject(TEST_OBJ_NAME);
            adapter = new GameObjectAdapter(testObj);
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (testObj != null)
                Object.Destroy(testObj);
            testObj = null;
            adapter = null;

            yield return null;
        }

        [UnityTest]
        public IEnumerator G_GameObjectAdapter_W_DontDestroyOnLoad_T_GameObjectPersistsThroughSceneLoad()
        {
            // Arrange
            adapter.DontDestroyOnLoad(adapter);

            // Act
            yield return SimulateSceneTransition();

            // Assert
            Assert.IsNotNull(GameObject.Find(TEST_OBJ_NAME));
        }

        [UnityTest]
        public IEnumerator G_GameObjectAdapter_W_Instantiate_T_NewGameObjectIsCreated()
        {
            // Act
            var newAdapter = adapter.Instantiate(adapter);
            yield return null;

            // Assert
            Assert.IsNotNull(newAdapter);
            Assert.AreNotEqual(adapter.GetInstanceID(), newAdapter.GetInstanceID());
            Assert.AreEqual(TEST_OBJ_NAME + "(Clone)", newAdapter.name);

            // Clean up
            newAdapter.Destroy(newAdapter);
        }


        [UnityTest]
        public IEnumerator G_GameObjectAdapter_W_InstantiateWithParent_T_NewGameObjectIsCreatedWithParent()
        {
            // Arrange
            var parentObj = new GameObject("Parent");
            var parentAdapter = new GameObjectAdapter(parentObj);

            // Act
            var newAdapter = adapter.Instantiate(adapter, parentAdapter.transform);
            yield return null;

            // Assert
            Assert.IsNotNull(newAdapter);
            var newGameObject = GameObject.Find(TEST_OBJ_NAME + "(Clone)");
            Assert.IsNotNull(newGameObject, "New GameObject was not found in the scene");
            Assert.AreEqual(parentObj.transform, newGameObject.transform.parent);

            // Clean up
            parentAdapter.Destroy(parentAdapter);
            newAdapter.Destroy(newAdapter);
        }


        [UnityTest]
        public IEnumerator G_GameObjectAdapter_W_InstantiateWithPosition_T_NewGameObjectIsCreatedAtPosition()
        {
            // Arrange
            Vector3 position = new Vector3(1, 2, 3);
            Quaternion rotation = Quaternion.Euler(30, 60, 90);

            // Act
            var newAdapter = adapter.Instantiate(adapter, position, rotation);
            yield return null;

            // Assert
            Assert.IsNotNull(newAdapter);
            var newGameObject = GameObject.Find(TEST_OBJ_NAME + "(Clone)");
            Assert.IsNotNull(newGameObject, "New GameObject was not found in the scene");
            Assert.AreEqual(position, newGameObject.transform.position);
            Assert.AreEqual(rotation, newGameObject.transform.rotation);

            // Clean up
            newAdapter.Destroy(newAdapter);
        }

        [UnityTest]
        public IEnumerator G_GameObjectAdapter_W_InstantiateWithPositionAndParent_T_NewGameObjectIsCreatedAtPositionWithParent()
        {
            // Arrange
            var parentObj = new GameObject("Parent");
            var parentAdapter = new GameObjectAdapter(parentObj);
            Vector3 position = new Vector3(1, 2, 3);
            Quaternion rotation = Quaternion.Euler(30, 60, 90);

            // Act
            var newAdapter = adapter.Instantiate(adapter, position, rotation, parentAdapter.transform);
            yield return null;

            // Assert
            Assert.IsNotNull(newAdapter);
            var newGameObject = GameObject.Find(TEST_OBJ_NAME + "(Clone)");
            Assert.IsNotNull(newGameObject, "New GameObject was not found in the scene");
            Assert.AreEqual(parentObj.transform, newGameObject.transform.parent);
            Assert.AreEqual(position, newGameObject.transform.localPosition);
            Assert.AreEqual(rotation, newGameObject.transform.localRotation);

            // Clean up
            parentAdapter.Destroy(parentAdapter);
            newAdapter.Destroy(newAdapter);
        }

        private IEnumerator SimulateSceneTransition()
        {
            var originScene = SceneManager.GetActiveScene();
            var tmpSceneName = "TempScene";
            var tmpScene = SceneManager.CreateScene(tmpSceneName);
            yield return SceneManager.SetActiveScene(tmpScene);
            yield return SceneManager.UnloadSceneAsync(tmpScene);
            yield return SceneManager.SetActiveScene(originScene);
        }
    }
}

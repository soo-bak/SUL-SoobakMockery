using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SUL.TestUtilities;
using NUnit.Framework.Internal;
using System.Linq;
using UnityEditor;

namespace SUL.Adapters {

[TestFixture]
[Category("Adapters")]
public class GameObjectAdapterPlayTests {
  private const string TEST_GO_NAME = "TestObject";
  private const string TEST_OBJ_NAME = "TestObject";
  private const string TEST_TAG_NAME = "TestTag";
  private const string TEST_METHOD_NAME = "TestMethod";
  private const string TEST_PARAMETER_NAME = "TestParameter";
  private const string CLONE_SUFFIX = "(Clone)";
  private const string PARENT_PREFIX = "(Parent)";
  private const string CHILD_PREFIX = "(Child)";

  public class MonobehaviorSpy : MonoBehaviour {
    public bool MethodCalled { get; private set; }
    public string LastMethodName { get; private set; }
    public object LastParameter { get; private set; }

    public void TestMethod(object parameter = null) {
      MethodCalled = true;
      LastMethodName = nameof(TestMethod);
      LastParameter = parameter;
    }
  }

  private IGameObjectAdapter goAdapter = null;
  private GameObject testGo = null;

  [OneTimeSetUp]
  public void OneTimeSetup() { 
    EditorSettings.enterPlayModeOptionsEnabled = true;
    EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
  }

  [UnitySetUp]
  public IEnumerator SetUp() {
    yield return new EnterPlayMode();

    testGo = new GameObject(TEST_OBJ_NAME);
    goAdapter = new GameObjectAdapter(testGo);
  }

  [UnityTearDown]
  public IEnumerator TearDown() {
    if (testGo != null) Object.Destroy(testGo);
    testGo = null;
    goAdapter = null;

    yield return new ExitPlayMode();
  }

  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_DontDestroyOnLoad_T_GameObjectPersistsThroughSceneLoad() {
    // Arrange
    goAdapter.DontDestroyOnLoad(goAdapter);

    // Act
    yield return TestHelper.SimulateSceneTransition();

    // Assert
    Assert.IsNotNull(GameObject.Find(TEST_OBJ_NAME));
  }

  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_Instantiate_T_NewGameObjectIsCreated() {
    // Act
    var newAdapter = goAdapter.Instantiate(goAdapter);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    Assert.AreNotEqual(goAdapter.GetInstanceID(), newAdapter.GetInstanceID());
    Assert.AreEqual(TEST_OBJ_NAME + CLONE_SUFFIX, newAdapter.name);

    // Clean up
    newAdapter.Destroy(newAdapter);
  }


  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_InstantiateWithParent_T_NewGameObjectIsCreatedWithParent() {
    // Arrange
    var parentObj = new GameObject(PARENT_PREFIX);
    var parentAdapter = new GameObjectAdapter(parentObj);

    // Act
    var newAdapter = goAdapter.Instantiate(goAdapter, parentAdapter.transform);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    var newGameObject = GameObject.Find(TEST_OBJ_NAME + CLONE_SUFFIX);
    Assert.IsNotNull(newGameObject, "New GameObject was not found in the scene");
    Assert.IsTrue(parentObj.transform == newGameObject.transform.parent);

    // Clean up
    parentAdapter.Destroy(parentAdapter);
    newAdapter.Destroy(newAdapter);
  }

  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_InstantiateWithPosition_T_NewGameObjectIsCreatedAtPosition() {
    // Arrange
    var position = new Vector3(1, 2, 3);
    var rotation = Quaternion.Euler(30, 60, 90);

    // Act
    var newAdapter = goAdapter.Instantiate(goAdapter, position, rotation);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    var newGameObject = GameObject.Find(TEST_OBJ_NAME + CLONE_SUFFIX);
    Assert.IsNotNull(newGameObject, "New GameObject was not found in the scene");
    Assert.IsTrue(position == newGameObject.transform.position);
    Assert.IsTrue(rotation == newGameObject.transform.rotation);

    // Clean up
    newAdapter.Destroy(newAdapter);
  }

  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_InstantiateWithPositionAndParent_T_NewGameObjectIsCreatedAtPositionWithParent() {
    // Arrange
    var parentObj = new GameObject(PARENT_PREFIX);
    var parentAdapter = new GameObjectAdapter(parentObj);
    var position = new Vector3(1, 2, 3);
    var rotation = Quaternion.Euler(30, 60, 90);

    // Act
    var newAdapter = goAdapter.Instantiate(goAdapter, position, rotation, parentAdapter.transform);
    yield return null;

    // Assert
    Assert.IsNotNull(newAdapter);
    var newGameObject = GameObject.Find(TEST_OBJ_NAME + CLONE_SUFFIX);
    Assert.IsNotNull(newGameObject, "New GameObject was not found in the scene");
    Assert.IsTrue(parentObj.transform == newGameObject.transform.parent);
    Assert.IsTrue(position == newGameObject.transform.localPosition);
    Assert.IsTrue(rotation == newGameObject.transform.localRotation);

    // Clean up
    parentAdapter.Destroy(parentAdapter);
    newAdapter.Destroy(newAdapter);
  }

  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_SendMessage_T_MessageIsSent() {
    // Arrange
    var mockBehaviour = testGo.AddComponent<MonobehaviorSpy>();
            
    // Act
    goAdapter.SendMessage(TEST_METHOD_NAME, TEST_PARAMETER_NAME, SendMessageOptions.RequireReceiver);
    yield return null; // Wait for a frame to ensure the message is processed

    // Assert
    Assert.IsTrue(mockBehaviour.MethodCalled);
    Assert.AreEqual(TEST_METHOD_NAME, mockBehaviour.LastMethodName);
    Assert.AreEqual(TEST_PARAMETER_NAME, mockBehaviour.LastParameter);
  }

  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_SendMessageUpwards_T_MessageIsSentUpwards() {
    // Arrange
    var parentObj = new GameObject(PARENT_PREFIX);
    testGo.transform.SetParent(parentObj.transform);
    var mockBehaviour = parentObj.AddComponent<MonobehaviorSpy>();

    // Act
    goAdapter.SendMessageUpwards(TEST_METHOD_NAME, TEST_PARAMETER_NAME, SendMessageOptions.RequireReceiver);
    yield return null; // Wait for a frame to ensure the message is processed

    // Assert
    Assert.IsTrue(mockBehaviour.MethodCalled);
    Assert.AreEqual(TEST_METHOD_NAME, mockBehaviour.LastMethodName);
    Assert.AreEqual(TEST_PARAMETER_NAME, mockBehaviour.LastParameter);

    // Clean up
    Object.Destroy(parentObj);
  }

  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_BroadcastMessage_T_MessageIsBroadcasted() {
    // Arrange
    var childObj = new GameObject(CHILD_PREFIX);
    childObj.transform.SetParent(testGo.transform);
    var mockBehaviour = childObj.AddComponent<MonobehaviorSpy>();

    // Act
    goAdapter.BroadCastMessage(TEST_METHOD_NAME, TEST_PARAMETER_NAME, SendMessageOptions.RequireReceiver);
    yield return null; // Wait for a frame to ensure the message is processed

    // Assert
    Assert.IsTrue(mockBehaviour.MethodCalled);
    Assert.AreEqual(TEST_METHOD_NAME, mockBehaviour.LastMethodName);
    Assert.AreEqual(TEST_PARAMETER_NAME, mockBehaviour.LastParameter);

    // Clean up
    Object.Destroy(childObj);
  }
  
  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_FindWithTag_T_ReturnsCorrectGameObjectAdapter() {
    // Arrange
    InitTagConfig(TEST_TAG_NAME);
    testGo.tag = TEST_TAG_NAME;

    // Act
    var foundAdapter = goAdapter.FindWithTag(TEST_TAG_NAME);
    yield return null;

    // Assert
    Assert.IsNotNull(foundAdapter);
    Assert.IsInstanceOf<IGameObjectAdapter>(foundAdapter);
    Assert.AreEqual(TEST_GO_NAME, foundAdapter.name);
  }


  [UnityTest]
  public IEnumerator G_GameObjectAdapter_W_FindGameObjectsWithTag_T_ReturnsCorrectGameObjectAdapters() {
    // Arrange
    InitTagConfig(TEST_TAG_NAME);
    testGo.tag = TEST_TAG_NAME;
    var anotherObj = new GameObject("AnotherObject") {
      tag = TEST_TAG_NAME
    };

    // Act
    var foundAdapters = goAdapter.FindGameObjectsWithTag(TEST_TAG_NAME);
    yield return null;

    // Assert
    Assert.IsNotNull(foundAdapters);
    Assert.AreEqual(2, foundAdapters.Length);
    Assert.IsTrue(foundAdapters.All(a => a is IGameObjectAdapter));
    Assert.IsTrue(foundAdapters.Any(a => a.name == TEST_GO_NAME));
    Assert.IsTrue(foundAdapters.Any(a => a.name == "AnotherObject"));

    // Clean up
    Object.DestroyImmediate(anotherObj);
  }

  private void InitTagConfig(string tagName) {
    var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
    var targetPropName = "tags";
    var tagsProp = tagManager.FindProperty(targetPropName);
            
    bool isFound = false;
    for (int i = 0; i < tagsProp.arraySize; i++) {
      SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
      if (t.stringValue.Equals(tagName)) {
        isFound = true;
        break ;
      }
    }
            
    if (!isFound) {
      tagsProp.InsertArrayElementAtIndex(0);
      SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(0);
      newTag.stringValue = tagName;
      tagManager.ApplyModifiedProperties();
    }
  }
}

} // namespace
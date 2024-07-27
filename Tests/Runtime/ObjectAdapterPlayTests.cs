using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SUL.Adapters;
using System.Collections;

public class ObjectAdapterPlayTests
{
    private IObjectAdapter adapter;
    private UnityEngine.Object testObject;

    [SetUp]
    public void SetUp()
    {
        testObject = new GameObject();
        adapter = new ObjectAdapter(testObject);
    }

    [TearDown]
    public void TearDown()
    {
        if (testObject != null)
        {
            UnityEngine.Object.Destroy(testObject);
        }
    }

    [UnityTest]
    public IEnumerator Destroy_DestroysObjectAfterDelay()
    {
        adapter.Destroy(adapter, 0.1f);

        Assert.IsNotNull(testObject.GetInstanceID());

        yield return new WaitForSeconds(0.2f);

        Assert.IsTrue(testObject == null);
    }

    [UnityTest]
    public IEnumerator DontDestroyOnLoad_KeepsObjectAcrossSceneLoads()
    {
        adapter.DontDestroyOnLoad(adapter);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        yield return null;

        Assert.IsNotNull(testObject);
    }
}
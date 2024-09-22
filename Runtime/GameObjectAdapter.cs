using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SUL.Adapters {

public interface IGameObjectAdapter : IObjectAdapter {
  public bool activeInHierachy { get;}
  public bool activeSelf { get;}
  public bool isStatic { get; set; }
  public int layer { get; set; }
  public UnityEngine.SceneManagement.Scene scene { get; }
  public ulong sceneCullingMask { get; }
  public string tag { get; set; }
  public Transform transform { get; }

  public Component AddComponent(Type componentType);
  public void BroadCastMessage(string message, object parameter = null, SendMessageOptions options = SendMessageOptions.RequireReceiver);
  public bool CompareTag(string tag);
  public T GetComponent<T>();
  public Component GetComponent(Type type);
  public Component GetComponent(string type);
  public T GetComponentInChildren<T>(bool includeInactive = false);
  public Component GetComponentInChildren(Type type);
  public Component GetComponentInChildren(Type type, bool includeInactive);
  public T GetComponentInParent<T>(bool includeInactive= false);
  public Component GetComponentInParent(Type type);
  public Component GetComponentInParent(Type type, bool includeInactive);
  public T[] GetComponents<T>();
  public void GetComponents<T>(List<T> results);
  public Component[] GetComponents(Type type);
  public void GetComponents(Type type, List<Component> results);
  public T[] GetComponentsInChildren<T>();
  public T[] GetComponentsInChildren<T>(bool includeInactive);
  public void GetComponentsInChildren<T>(List<T> results);
  public void GetComponentsInChildren<T>(bool includeInactive, List<T> results);
  public Component[] GetComponentsInChildren (Type type, bool includeInactive= false);
  public T[] GetComponentsInParent<T>();
  public T[] GetComponentsInParent<T>(bool includeInactive);
  public void GetComponentsInParent<T>(bool includeInactive, List<T> results);
  public Component[] GetComponentsInParent(Type type, bool includeInactive= false);
  public void SendMessage(string methodName, object value= null, SendMessageOptions options= SendMessageOptions.RequireReceiver);
  public void SendMessageUpwards(string methodName, object value= null, SendMessageOptions options= SendMessageOptions.RequireReceiver);
  public void SetActive(bool value);
  public bool TryGetComponent<T>(out T component);
  public bool TryGetComponent(Type type, out Component component);

  public IGameObjectAdapter CreatePrimitive(PrimitiveType type);
  public IGameObjectAdapter Find(string name);
  public IGameObjectAdapter[] FindGameObjectsWithTag(string tag);
  public IGameObjectAdapter FindWithTag(string tag);
  public UnityEngine.SceneManagement.Scene GetScene(int instanceID);
  public void InstantiateGameObjects(int sourceInstanceID, int count, NativeArray<int> newInstanceIDs, NativeArray<int> newTransformInstanceIDs,
                                     Scene destinationScene = default(Scene));
  public void SetGameObjectsActive(NativeArray<int> instanceIDs, bool active);
  public void SetGameObjectsActive(ReadOnlySpan<int> instanceIDs, bool active);
}

public class GameObjectAdapter : IGameObjectAdapter {
  private GameObject adaptee;

  public GameObjectAdapter(GameObject adaptee)
  => this.adaptee = adaptee;

  public bool activeInHierachy => adaptee.activeInHierarchy;

  public bool activeSelf => adaptee.activeSelf;

  public bool isStatic { get => adaptee.isStatic; set => adaptee.isStatic = value; }
  public int layer { get => adaptee.layer; set => adaptee.layer = value; }

  public Scene scene => adaptee.scene;

  public ulong sceneCullingMask => adaptee.sceneCullingMask;

  public string tag { get => adaptee.tag; set => adaptee.tag = value; }

  public Transform transform => adaptee.transform;


  public GameObjectAdapter() => this.adaptee = new GameObject();

  public GameObjectAdapter(string name) => this.adaptee = new GameObject(name);

  public GameObjectAdapter(string name, params Type[] components) => this.adaptee = new GameObject(name, components);


  public string name { get => adaptee.name; set => adaptee.name = value; }
  public HideFlags hideFlags { get => adaptee.hideFlags; set => adaptee.hideFlags = value; }

  public void Destroy(IObjectAdapter objAdapter, float t = 0) {
    if (objAdapter is GameObjectAdapter gameObjAdapter)
      UnityEngine.Object.Destroy(gameObjAdapter.adaptee, t);
  }

  public void DestroyImmediate(IObjectAdapter objAdapter, bool allowDestroyingAssets = false) {
    if (objAdapter is GameObjectAdapter gameObjAdapter)
      UnityEngine.Object.DestroyImmediate(gameObjAdapter.adaptee, allowDestroyingAssets);
  }

  public void DontDestroyOnLoad(IObjectAdapter target) {
    if (target is GameObjectAdapter gameObjAdapter)
      UnityEngine.Object.DontDestroyOnLoad(gameObjAdapter.adaptee);
  }

  public T FindObjectOfType<T>() where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectOfType<T>();

  public T FindObjectOfType<T>(bool includeInactive) where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectOfType<T>(includeInactive);

  public IObjectAdapter FindObjectOfType(Type type) {
    var obj = UnityEngine.Object.FindObjectOfType(type);
    return obj is GameObject gameObj ? new GameObjectAdapter(gameObj) : null;
  }

  public IObjectAdapter FindObjectOfType(Type type, bool includeInactive) {
    var obj = UnityEngine.Object.FindObjectOfType(type, includeInactive);
    return obj is GameObject gmaeObj ? new GameObjectAdapter(gmaeObj) : null;
  }

  public IObjectAdapter[] FindObjectsOfType(Type type) {
    var objs = UnityEngine.Object.FindObjectsOfType(type);
    return ConvertToAdapters(objs);
  }

  public IObjectAdapter[] FindObjectsOfType(Type type, bool includeInactive) {
    var objs = UnityEngine.Object.FindObjectsOfType(type, includeInactive);
    return ConvertToAdapters(objs);
  }

  public T[] FindObjectsOfType<T>(bool includeInactive) where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectsOfType<T>(includeInactive);
  

  public T[] FindObjectsOfType<T>() where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectsOfType<T>();

  public int GetInstanceID() => adaptee.GetInstanceID();

  public IObjectAdapter Instantiate(IObjectAdapter original)
    => InstantiateInternal(original, null, false);

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent)
    => InstantiateInternal(original, parent, false);

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent, bool instantiateInWorldSpace)
    => InstantiateInternal(original, parent, instantiateInWorldSpace);

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation)
    => InstantiateInternal(original, null, false, position, rotation);

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation, Transform parent)
    => InstantiateInternal(original, parent, false, position, rotation);

  public Component AddComponent(Type componentType) => adaptee.AddComponent(componentType);

  public void BroadCastMessage(string methodName, object parameter = null,
                               SendMessageOptions options = SendMessageOptions.RequireReceiver)
     => adaptee.BroadcastMessage(methodName, parameter, options);

  public bool CompareTag(string tag) => adaptee.CompareTag(tag);

  public T GetComponent<T>() => adaptee.GetComponent<T>();

  public Component GetComponent(Type type) => adaptee.GetComponent(type);

  public Component GetComponent(string type) => adaptee.GetComponent(type);

  public T GetComponentInChildren<T>(bool includeInactive = false)
    => adaptee.GetComponentInChildren<T>(includeInactive);

  public Component GetComponentInChildren(Type type)
    => adaptee.GetComponentInChildren(type);

  public Component GetComponentInChildren(Type type, bool includeInactive)
    => adaptee.GetComponentInChildren(type, includeInactive);

  public T GetComponentInParent<T>(bool includeInactive = false)
    => adaptee.GetComponentInParent<T>(includeInactive);

  public Component GetComponentInParent(Type type)
    => adaptee.GetComponentInParent(type);

  public Component GetComponentInParent(Type type, bool includeInactive)
    => adaptee.GetComponentInParent(type, includeInactive);

  public T[] GetComponents<T>() => adaptee.GetComponents<T>();

  public void GetComponents<T>(List<T> results) => adaptee.GetComponents(results);

  public Component[] GetComponents(Type type) => adaptee.GetComponents(type);

  public void GetComponents(Type type, List<Component> results) => adaptee.GetComponents(type, results);

  public T[] GetComponentsInChildren<T>() => adaptee.GetComponentsInChildren<T>();

  public T[] GetComponentsInChildren<T>(bool includeInactive) => adaptee.GetComponentsInChildren<T>(includeInactive);

  public void GetComponentsInChildren<T>(List<T> results) => adaptee.GetComponentsInChildren(results);

  public void GetComponentsInChildren<T>(bool includeInactive, List<T> results)
    => adaptee.GetComponentsInChildren(includeInactive, results);

  public Component[] GetComponentsInChildren(Type type, bool includeInactive = false) 
    => adaptee.GetComponentsInChildren(type, includeInactive);

  public T[] GetComponentsInParent<T>() => adaptee.GetComponentsInParent<T>();

  public T[] GetComponentsInParent<T>(bool includeInactive) => adaptee.GetComponentsInParent<T>(includeInactive);

  public void GetComponentsInParent<T>(bool includeInactive, List<T> results)
    => adaptee.GetComponentsInParent(includeInactive, results);

  public Component[] GetComponentsInParent(Type type, bool includeInactive = false)
    => adaptee.GetComponentsInParent(type, includeInactive);

  public void SendMessage(string methodName, object value = null,
                          SendMessageOptions options = SendMessageOptions.RequireReceiver)
    => adaptee.SendMessage(methodName, value, options);

  public void SendMessageUpwards(string methodName, object value = null,
                                 SendMessageOptions options = SendMessageOptions.RequireReceiver)
    => adaptee.SendMessageUpwards(methodName, value, options);

  public void SetActive(bool value) => adaptee.SetActive(value);

  public bool TryGetComponent<T>(out T component) => adaptee.TryGetComponent(out component);

  public bool TryGetComponent(Type type, out Component component) => adaptee.TryGetComponent(type, out component);

  public IGameObjectAdapter CreatePrimitive(PrimitiveType type)
    => new GameObjectAdapter(GameObject.CreatePrimitive(type));

  public IGameObjectAdapter Find(string name) {
    var gameObj = GameObject.Find(name);
    return gameObj != null ? new GameObjectAdapter(gameObj) : null;
  }

  public IGameObjectAdapter[] FindGameObjectsWithTag(string tag) {
    var gameObjs = GameObject.FindGameObjectsWithTag(tag);
    return Array.ConvertAll(gameObjs, gameObj => new GameObjectAdapter(gameObj) as IGameObjectAdapter);
  }

  public IGameObjectAdapter FindWithTag(string tag) {
    var gameObj = GameObject.FindWithTag(tag);
    return gameObj != null ? new GameObjectAdapter(gameObj) : null;
  }

  public Scene GetScene(int instanceID) => GameObject.GetScene(instanceID);

  public void InstantiateGameObjects(int sourceInstanceID, int count, NativeArray<int> newInstanceIDs,
                                     NativeArray<int> newTransformInstanceIDs, Scene destinationScene = default)
    => GameObject.InstantiateGameObjects(sourceInstanceID, count, newInstanceIDs, newTransformInstanceIDs, destinationScene);

  public void SetGameObjectsActive(NativeArray<int> instanceIDs, bool active)
    => GameObject.SetGameObjectsActive(instanceIDs, active);

  public void SetGameObjectsActive(ReadOnlySpan<int> instanceIDs, bool active)
    => GameObject.SetGameObjectsActive(instanceIDs, active);

  private IObjectAdapter[] ConvertToAdapters(UnityEngine.Object[] objs) {
    var adapters = new IObjectAdapter[objs.Length];
    for (int i = 0; i < objs.Length; i++) {
      if (objs[i] is GameObject gameObj) adapters[i] = new GameObjectAdapter(gameObj);
      else adapters[i] = null;
    }
    
    return adapters;
  }

  private IObjectAdapter InstantiateInternal(IObjectAdapter original, Transform parent = null, bool instantiateInWorldSpace = false,
                                             Vector3? position = null, Quaternion? rotation = null) {
    if (original is GameObjectAdapter gameObjAdapter) {
      GameObject instantiated;
      if (position.HasValue && rotation.HasValue)
        instantiated = parent != null ?
          UnityEngine.Object.Instantiate(gameObjAdapter.adaptee, position.Value, rotation.Value, parent) :
          UnityEngine.Object.Instantiate(gameObjAdapter.adaptee, position.Value, rotation.Value);
      else
        instantiated = parent != null ?
          UnityEngine.Object.Instantiate(gameObjAdapter.adaptee, parent, instantiateInWorldSpace) :
          UnityEngine.Object.Instantiate(gameObjAdapter.adaptee);

      return new GameObjectAdapter(instantiated);
    } else return null;
  }
}

} // namespace

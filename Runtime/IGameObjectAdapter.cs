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
  public bool activeInHierachy => throw new NotImplementedException();

  public bool activeSelf => throw new NotImplementedException();

  public bool isStatic { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  public int layer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public Scene scene => throw new NotImplementedException();

  public ulong sceneCullingMask => throw new NotImplementedException();

  public string tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public Transform transform => throw new NotImplementedException();


  public GameObjectAdapter() => throw new NotImplementedException();

  public GameObjectAdapter(string name) => throw new NotImplementedException();

  public GameObjectAdapter(string name, params Type[] components) => throw new NotImplementedException();



  public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
  public HideFlags HideFlags { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

  public void Destroy(IObjectAdapter objAdapter, float f = 0) {
    throw new NotImplementedException();
  }

  public void DestroyImmediate(IObjectAdapter objAdapter, bool allowDestroyingAssets = false) {
    throw new NotImplementedException();
  }

  public void DontDestroyOnLoad(IObjectAdapter target) {
    throw new NotImplementedException();
  }

  public T FindObjectOfType<T>() where T : UnityEngine.Object {
    throw new NotImplementedException();
  }

  public T FindObjectOfType<T>(bool includeInactive) where T : UnityEngine.Object {
    throw new NotImplementedException();
  }

  public IObjectAdapter FindObjectOfType(Type type) {
    throw new NotImplementedException();
  }

  public IObjectAdapter FindObjectOfType(Type type, bool includeInactive) {
    throw new NotImplementedException();
  }

  public IObjectAdapter[] FindObjectsOfType(Type type) {
    throw new NotImplementedException();
  }

  public IObjectAdapter[] FindObjectsOfType(Type type, bool includeInactive) {
    throw new NotImplementedException();
  }

  public T[] FindObjectsOfType<T>(bool includeInactive) where T : UnityEngine.Object {
    throw new NotImplementedException();
  }

  public T[] FindObjectsOfType<T>() where T : UnityEngine.Object {
    throw new NotImplementedException();
  }

  public int GetInstanceID() {
    throw new NotImplementedException();
  }

  public IObjectAdapter Instantiate(IObjectAdapter original) {
    throw new NotImplementedException();
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent) {
    throw new NotImplementedException();
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent, bool instantiateInWorldSpace) {
    throw new NotImplementedException();
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation) {
    throw new NotImplementedException();
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation, Transform parent) {
    throw new NotImplementedException();
  }

  public Component AddComponent(Type componentType) {
    throw new NotImplementedException();
  }

  public void BroadCastMessage(string message, object parameter = null, SendMessageOptions options = SendMessageOptions.RequireReceiver) {
    throw new NotImplementedException();
  }

  public bool CompareTag(string tag) {
    throw new NotImplementedException();
  }

  public T GetComponent<T>() {
    throw new NotImplementedException();
  }

  public Component GetComponent(Type type) {
    throw new NotImplementedException();
  }

  public Component GetComponent(string type) {
    throw new NotImplementedException();
  }

  public T GetComponentInChildren<T>(bool includeInactive = false) {
    throw new NotImplementedException();
  }

  public Component GetComponentInChildren(Type type) {
    throw new NotImplementedException();
  }

  public Component GetComponentInChildren(Type type, bool includeInactive) {
    throw new NotImplementedException();
  }

  public T GetComponentInParent<T>(bool includeInactive = false) {
    throw new NotImplementedException();
  }

  public Component GetComponentInParent(Type type) {
    throw new NotImplementedException();
  }

  public Component GetComponentInParent(Type type, bool includeInactive) {
    throw new NotImplementedException();
  }

  public T[] GetComponents<T>() {
    throw new NotImplementedException();
  }

  public void GetComponents<T>(List<T> results) {
    throw new NotImplementedException();
  }

  public Component[] GetComponents(Type type) {
    throw new NotImplementedException();
  }

  public void GetComponents(Type type, List<Component> results) {
    throw new NotImplementedException();
  }

  public T[] GetComponentsInChildren<T>() {
    throw new NotImplementedException();
  }

  public T[] GetComponentsInChildren<T>(bool includeInactive) {
    throw new NotImplementedException();
  }

  public void GetComponentsInChildren<T>(List<T> results) {
    throw new NotImplementedException();
  }

  public void GetComponentsInChildren<T>(bool includeInactive, List<T> results) {
    throw new NotImplementedException();
  }

  public Component[] GetComponentsInChildren(Type type, bool includeInactive = false) {
    throw new NotImplementedException();
  }

  public T[] GetComponentsInParent<T>() {
    throw new NotImplementedException();
  }

  public T[] GetComponentsInParent<T>(bool includeInactive) {
    throw new NotImplementedException();
  }

  public void GetComponentsInParent<T>(bool includeInactive, List<T> results) {
    throw new NotImplementedException();
  }

  public Component[] GetComponentsInParent(Type type, bool includeInactive = false) {
    throw new NotImplementedException();
  }

  public void SendMessage(string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver) {
    throw new NotImplementedException();
  }

  public void SendMessageUpwards(string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver) {
    throw new NotImplementedException();
  }

  public void SetActive(bool value) {
    throw new NotImplementedException();
  }

  public bool TryGetComponent<T>(out T component) {
    throw new NotImplementedException();
  }

  public bool TryGetComponent(Type type, out Component component) {
    throw new NotImplementedException();
  }

  public IGameObjectAdapter CreatePrimitive(PrimitiveType type) {
    throw new NotImplementedException();
  }

  public IGameObjectAdapter Find(string name) {
    throw new NotImplementedException();
  }

  public IGameObjectAdapter[] FindGameObjectsWithTag(string tag) {
    throw new NotImplementedException();
  }

  public IGameObjectAdapter FindWithTag(string tag) {
    throw new NotImplementedException();
  }

  public Scene GetScene(int instanceID) {
    throw new NotImplementedException();
  }

  public void InstantiateGameObjects(int sourceInstanceID, int count, NativeArray<int> newInstanceIDs, NativeArray<int> newTransformInstanceIDs, Scene destinationScene = default) {
    throw new NotImplementedException();
  }

  public void SetGameObjectsActive(NativeArray<int> instanceIDs, bool active) {
    throw new NotImplementedException();
  }

  public void SetGameObjectsActive(ReadOnlySpan<int> instanceIDs, bool active) {
    throw new NotImplementedException();
  }
}

} // namespace

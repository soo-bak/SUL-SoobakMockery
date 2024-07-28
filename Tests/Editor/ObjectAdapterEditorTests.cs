using NSubstitute;
using NUnit.Framework;
using SUL.Adapters;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SUL_Tests.Adapters
{
    [TestFixture]
    [Category("Adapters")]
    public class ObjectAdapterEditorTests
    {
            private IObjectAdapter adapter;

            [SetUp]
            public void SetUp()
            {
                adapter = Substitute.For<IObjectAdapter>();
            }

            [Test]
            public void Name_GetSet_WorksCorrectly()
            {
                adapter.Name.Returns("TestName");
                Assert.AreEqual("TestName", adapter.Name);

                adapter.Name = "NewName";
                adapter.Received().Name = "NewName";

                adapter.Name = null;
                adapter.Received().Name = null;
            }

            [Test]
            public void HideFlags_GetSet_WorksCorrectly()
            {
                adapter.HideFlags.Returns(HideFlags.HideInHierarchy);
                Assert.AreEqual(HideFlags.HideInHierarchy, adapter.HideFlags);

                adapter.HideFlags = HideFlags.DontSave;
                adapter.Received().HideFlags = HideFlags.DontSave;
            }

            [Test]
            public void GetInstanceID_ReturnsCorrectID()
            {
                adapter.GetInstanceID().Returns(123);
                Assert.AreEqual(123, adapter.GetInstanceID());
            }

            [Test]
            public void ToString_ReturnsCorrectString()
            {
                adapter.ToString().Returns("MockObject");
                Assert.AreEqual("MockObject", adapter.ToString());
            }

            [Test]
            public void DestroyImmediate_CallsDestroyImmediateMethod()
            {
                var mockAdapter = Substitute.For<IObjectAdapter>();
                adapter.DestroyImmediate(mockAdapter, true);
                adapter.Received().DestroyImmediate(mockAdapter, true);
            }

            [Test]
            public void DontDestroyOnLoad_CallsDontDestroyOnLoadMethod()
            {
                var mockAdapter = Substitute.For<IObjectAdapter>();
                adapter.DontDestroyOnLoad(mockAdapter);
                adapter.Received().DontDestroyOnLoad(mockAdapter);
            }

            [Test]
            public void FindObjectOfType_Generic_ReturnsCorrectObject()
            {
            var mockComponent = new GameObject();
            adapter.FindObjectOfType<GameObject>().Returns(mockComponent);
            var result = adapter.FindObjectOfType<GameObject>();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<GameObject>(result);
        }

            [Test]
            public void FindObjectOfType_Generic_WithIncludeInactive_ReturnsCorrectObject()
            {
                var mockComponent = new GameObject();
                adapter.FindObjectOfType<GameObject>(true).Returns(mockComponent);

                var result = adapter.FindObjectOfType<GameObject>(true);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<GameObject>(result);
            }

            [Test]
            public void FindObjectOfType_WithType_ReturnsCorrectObject()
            {
                var mockAdapter = Substitute.For<IObjectAdapter>();
                adapter.FindObjectOfType(typeof(Component)).Returns(mockAdapter);

                var result = adapter.FindObjectOfType(typeof(Component));

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IObjectAdapter>(result);
            }

            [Test]
            public void FindObjectOfType_WithType_AndIncludeInactive_ReturnsCorrectObject()
            {
                var mockAdapter = Substitute.For<IObjectAdapter>();
                adapter.FindObjectOfType(typeof(Component), true).Returns(mockAdapter);

                var result = adapter.FindObjectOfType(typeof(Component), true);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IObjectAdapter>(result);
            }

            [Test]
            public void FindObjectsOfType_Generic_ReturnsCorrectObjects()
            {
                var mockComponents = new Component[] {
                Substitute.For<Component>(),
                Substitute.For<Component>()
            };
                adapter.FindObjectsOfType<Component>().Returns(mockComponents);

                var results = adapter.FindObjectsOfType<Component>();

                Assert.AreEqual(2, results.Length);
                Assert.IsTrue(results.All(r => r is Component));
            }

            [Test]
            public void FindObjectsOfType_Generic_WithIncludeInactive_ReturnsCorrectObjects()
            {
                var mockComponents = new Component[] {
                Substitute.For<Component>(),
                Substitute.For<Component>()
            };
                adapter.FindObjectsOfType<Component>(true).Returns(mockComponents);

                var results = adapter.FindObjectsOfType<Component>(true);

                Assert.AreEqual(2, results.Length);
                Assert.IsTrue(results.All(r => r is Component));
            }

            [Test]
            public void FindObjectsOfType_WithType_ReturnsCorrectObjects()
            {
                var mockAdapters = new IObjectAdapter[] {
                Substitute.For<IObjectAdapter>(),
                Substitute.For<IObjectAdapter>()
            };
                adapter.FindObjectsOfType(typeof(Component)).Returns(mockAdapters);

                var results = adapter.FindObjectsOfType(typeof(Component));

                Assert.AreEqual(2, results.Length);
                Assert.IsTrue(results.All(r => r is IObjectAdapter));
            }

            [Test]
            public void FindObjectsOfType_WithType_AndIncludeInactive_ReturnsCorrectObjects()
            {
                var mockAdapters = new IObjectAdapter[] {
                Substitute.For<IObjectAdapter>(),
                Substitute.For<IObjectAdapter>()
            };
                adapter.FindObjectsOfType(typeof(Component), true).Returns(mockAdapters);

                var results = adapter.FindObjectsOfType(typeof(Component), true);

                Assert.AreEqual(2, results.Length);
                Assert.IsTrue(results.All(r => r is IObjectAdapter));
            }

            [Test]
            public void Instantiate_CreatesNewObject()
            {
                var mockOriginal = Substitute.For<IObjectAdapter>();
                var mockInstantiated = Substitute.For<IObjectAdapter>();
                adapter.Instantiate(mockOriginal).Returns(mockInstantiated);

                var result = adapter.Instantiate(mockOriginal);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IObjectAdapter>(result);
            }

            [Test]
            public void Instantiate_WithParent_CreatesNewObjectWithParent()
            {
                var mockOriginal = Substitute.For<IObjectAdapter>();
                var mockInstantiated = Substitute.For<IObjectAdapter>();
                var mockParent = Substitute.For<Transform>();
                adapter.Instantiate(mockOriginal, mockParent).Returns(mockInstantiated);

                var result = adapter.Instantiate(mockOriginal, mockParent);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IObjectAdapter>(result);
            }

            [Test]
            public void Instantiate_WithParentAndWorldPositionStays_CreatesNewObjectCorrectly()
            {
                var mockOriginal = Substitute.For<IObjectAdapter>();
                var mockInstantiated = Substitute.For<IObjectAdapter>();
                var mockParent = Substitute.For<Transform>();
                adapter.Instantiate(mockOriginal, mockParent, true).Returns(mockInstantiated);

                var result = adapter.Instantiate(mockOriginal, mockParent, true);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IObjectAdapter>(result);
            }

            [Test]
            public void Instantiate_WithPosition_CreatesNewObjectAtPosition()
            {
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
            public void Instantiate_WithPositionAndParent_CreatesNewObjectCorrectly()
            {
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
    }

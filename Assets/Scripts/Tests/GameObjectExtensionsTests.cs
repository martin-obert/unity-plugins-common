// using System.Collections;
// using NUnit.Framework;
// using Obert.Common.Runtime.Extensions;
// using UnityEngine;
// using UnityEngine.TestTools;
//
// namespace Tests
// {
//     public class GameObjectExtensionsTests
//     {
//         private interface ITestInterface
//         {
//             string Name { get; }
//         }
//
//         private interface ITestInterfaceDerived : ITestInterface
//         {
//             new string Name { get; }
//         }
//
//         private sealed class TestInterfaceImpl : MonoBehaviour, ITestInterface
//         {
//             public string Name => gameObject.name;
//         }
//
//         private sealed class TestInterfaceDerivedImpl : MonoBehaviour, ITestInterfaceDerived
//         {
//             public string Name => gameObject.name;
//         }
//
//         [UnityTest]
//         public IEnumerator GetInterfaceOfType_Pass()
//         {
//             const string testGameObjectName = "Test GameObject";
//             var testGameObject = new GameObject(testGameObjectName, typeof(TestInterfaceImpl));
//             yield return new WaitForEndOfFrame();
//             var testInterface = testGameObject.GetInterfaceOfType<ITestInterface>();
//             Assert.AreEqual(testGameObjectName, testInterface.Name);
//         }
//
//         [UnityTest]
//         public IEnumerator GetInterfaceOfDerivedInterface_Pass()
//         {
//             const string testGameObjectName = "Test GameObject";
//             var testGameObject = new GameObject(testGameObjectName, typeof(TestInterfaceDerivedImpl));
//             yield return new WaitForEndOfFrame();
//             var testInterface = testGameObject.GetInterfaceOfType<ITestInterface>();
//             Assert.AreEqual(testGameObjectName, testInterface.Name);
//         }
//
//
//         [UnityTest]
//         public IEnumerator DestroyChildren_Pass()
//         {
//             const string testGameObjectName = "Test GameObject";
//             var parentGameObject = new GameObject(testGameObjectName);
//             var childGameObject = new GameObject($"{testGameObjectName} - Child");
//             var parentTransform = parentGameObject.transform;
//             
//             childGameObject.transform.SetParent(parentTransform);
//             yield return new WaitForFixedUpdate();
//             Assert.AreEqual(1, parentTransform.childCount);
//             var parent = childGameObject.transform.parent;
//             Assert.IsTrue(parent == parentTransform);
//             parentTransform.ClearChildGameObjectsImmediately();
//             yield return new WaitForFixedUpdate();
//             Assert.AreEqual(0, parentTransform.childCount);
//             Assert.Throws<MissingReferenceException>(() =>
//             {
//                 var _ = childGameObject.transform;
//             });
//         }
//     }
// }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Duckject.Core.Attributes;
using Duckject.Core.Container;
using Duckject.Core.Context;
using UnityEngine;

namespace Duckject.Core.Utils
{
    public static class QuackUtils
    {
        #region Public Methods

        public static void Inject(object targetObject)
        {
            foreach (MethodInfo methodInfo in GetMethods(targetObject))
            {
                object[] parameters = GetParameters(methodInfo, targetObject);

                methodInfo.Invoke(targetObject, parameters);
            }
        }

        public static void CreateNonLazyInstances()
        {
            IEnumerable<Binding> bindings = ContextBase.Containers.SelectMany(container => container.Bindings);

            IEnumerable<Binding> nonLazyBindings =
                bindings.Where(b => b.IsNonLazy && b.InstanceType == InstanceType.Cached && b.Service == null);

            foreach (Binding nonLazyBinding in nonLazyBindings)
            {
                if (TryToCreateMonoBehaviour(nonLazyBinding))
                    continue;

                TryToCreateNonBehaviourInstance(nonLazyBinding);

                Debug.Log(nonLazyBinding.Service);
            }
        }

        public static bool IsContainInjectionMethods(object targetObject) => GetMethods(targetObject).Any();

        #endregion

        #region Private Methods

        private static bool TryToCreateMonoBehaviour(Binding binding)
        {
            Type serviceType = binding.ServiceType;

            if (serviceType.IsSubclassOf(typeof(MonoBehaviour)))
            {
                GameObject newGameObject = new GameObject(serviceType.FullName);
                newGameObject.SetActive(false);
                Component service = newGameObject.AddComponent(serviceType);
                binding.FromInstance(service);
                Inject(service);
                newGameObject.SetActive(true);
                return true;
            }

            return false;
        }

        private static void TryToCreateNonBehaviourInstance(Binding binding)
        {
            Type serviceType = binding.ServiceType;
            foreach (ConstructorInfo constructorInfo in GetConstructors(serviceType))
            {
                object[] parameters = GetParameters(constructorInfo, null);
                binding.FromInstance(Activator.CreateInstance(serviceType, parameters));
                break;
            }
        }

        private static object[] GetParameters(MethodBase methodBase, object targetObject)
        {
            List<object> parameters = new List<object>();

            QuackAttribute attribute = methodBase.GetCustomAttribute(typeof(QuackAttribute)) as QuackAttribute;

            IEnumerable<Binding> bindings = ContextBase.Containers.SelectMany(container => container.Bindings)
                .ToArray();

            IEnumerable<Type> parameterTypes = methodBase.GetParameters()
                .Select(parameterInfo => parameterInfo.ParameterType);

            foreach (Type parameterType in parameterTypes)
            {
                Binding bind = bindings.Where(binding => binding.InterfaceCheck(parameterType))
                    .Where(binding => binding.AbstractClassCheck(parameterType))
                    .Where(binding => binding.IsEqualsIdentifier(attribute?.Identifier))
                    .Where(binding => binding.IsTargetedTo(parameterType))
                    .FirstOrDefault(binding => binding.IsTargetedTo(targetObject));

                if (bind != null)
                    parameters.Add(bind.Service);
            }

            return parameters.ToArray();
        }

        private static bool IsEqualsIdentifier(this Binding binding, object identifier) =>
            binding.Identifier == identifier;

        private static bool IsTargetedTo(this Binding binding, Type type) =>
            !binding.TargetTypes.Any() || binding.TargetTypes.Contains(type);

        private static bool IsTargetedTo(this Binding binding, object targetObject) =>
            !binding.Targets.Any() || binding.Targets.Contains(targetObject);

        private static bool InterfaceCheck(this Binding binding, Type type) =>
            !type.IsInterface || binding.ServiceType.GetInterfaces().Any(item => item == type);

        private static bool AbstractClassCheck(this Binding binding, Type type) =>
            !(type.IsClass && type.IsAbstract) || binding.ServiceType.GetBaseTypes().Any(item => item == type);

        private static IEnumerable<MethodInfo> GetMethods(object targetObject) 
        {
            return targetObject.GetType()
                .GetBaseTypes()
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.Public |
                                                    BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
                .Where(info => info.GetCustomAttributes(typeof(QuackAttribute), false).FirstOrDefault() != null);
        }

        private static IEnumerable<ConstructorInfo> GetConstructors(Type type)
        {
            return type.GetConstructors()
                .Where(info => info.GetParameters().Length == 0 ||
                               info.GetCustomAttributes(typeof(QuackAttribute), false).FirstOrDefault() != null)
                .OrderByDescending(info => info.GetParameters().Length);
        }

        private static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            List<Type> types = new List<Type>();

            while (type.BaseType != null)
            {
                types.Add(type);
                type = type.BaseType;
            }

            return types;
        }

        #endregion
    }
}
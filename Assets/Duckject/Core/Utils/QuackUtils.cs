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
                object[] parameters = GetParameters(methodInfo, targetObject.GetType(), targetObject);

                methodInfo.Invoke(targetObject, parameters);
            }
        }

        public static void CreateNonLazyInstances()
        {
            IEnumerable<Binding> bindings = ContextBase.Containers.SelectMany(container => container.Bindings);

            IEnumerable<Binding> nonLazyBindings =
                bindings.Where(b => b.IsNonLazy && b.InstanceType == InstanceType.Cached && b.Service == null)
                    .ToArray();

            foreach (Binding nonLazyBinding in nonLazyBindings)
                TryToCreateInstance(nonLazyBinding);
        }

        public static bool IsContainInjectionMethods(object targetObject) => GetMethods(targetObject).Any();

        #endregion

        #region Private Methods

        private static object TryToCreateInstance(Binding binding)
        {
            Type serviceType = binding.ServiceType;

            if (serviceType.IsSubclassOf(typeof(MonoBehaviour)))
            {
                GameObject newGameObject = new GameObject(serviceType.FullName);
                newGameObject.transform.SetParent(ContextBase.GetTransformFor(binding));
                newGameObject.SetActive(false);
                Component service = newGameObject.AddComponent(serviceType);
                if (binding.InstanceType == InstanceType.Cached)
                    binding.FromInstance(service);
                Inject(service);
                newGameObject.SetActive(true);
                return service;
            }

            foreach (ConstructorInfo constructorInfo in GetConstructors(serviceType))
            {
                object[] parameters = GetParameters(constructorInfo, serviceType, null);
                object service = Activator.CreateInstance(serviceType, parameters);

                if (binding.InstanceType == InstanceType.Cached)
                    binding.FromInstance(service);

                return service;
            }

            return null;
        }

        private static object[] GetParameters(MethodBase methodBase, Type typeObject, object targetObject)
        {
            List<object> parameters = new List<object>();

            QuackAttribute attribute = methodBase.GetCustomAttribute(typeof(QuackAttribute)) as QuackAttribute;

            IEnumerable<Binding> bindings = ContextBase.Containers.SelectMany(container => container.Bindings)
                .ToArray();

            IEnumerable<Type> parameterTypes = methodBase.GetParameters()
                .Select(parameterInfo => parameterInfo.ParameterType);

            foreach (Type parameterType in parameterTypes)
            {
                Binding bind = bindings.Where(binding => binding.TypesCheck(parameterType))
                    .Where(binding => binding.InterfaceCheck(parameterType))
                    .Where(binding => binding.AbstractClassCheck(parameterType))
                    .Where(binding => binding.IsTargetedTo(typeObject))
                    .Where(binding => binding.IsTargetedTo(targetObject))
                    .FirstOrDefault(binding => binding.IsEqualsIdentifier(attribute?.Identifier));

                if (bind != null)
                {
                    if (bind.Service == null)
                    {
                        object service = TryToCreateInstance(bind);
                        parameters.Add(service);
                        continue;
                    }

                    parameters.Add(bind.Service);
                }
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

        private static bool TypesCheck(this Binding binding, Type type) =>
            binding.ServiceType.GetBaseTypes().Any(item => item == type);

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
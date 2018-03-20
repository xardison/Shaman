using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Shaman.Common.Extension
{
    public static class ReflectExt
    {
        public static void GetPropertiesMarkAttribute<TAttribute>(this object @object, Action<TAttribute, object> action)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            PropertyInfo[] pInfos = @object.GetType().GetProperties();
            foreach (var propertyInfo in pInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(true)
                    .Where(x => x.GetType() == typeof(TAttribute));

                if (attributes.Count() > 0)
                {
                    var attribute = (TAttribute)propertyInfo.GetCustomAttributes(typeof(TAttribute), false).First();
                    action(attribute, propertyInfo.GetValue(@object, null));
                }
            }
        }
        public static void GetPropertiesMarkAttribute<TAttribute>(this object @object, Action<TAttribute, object, Type> action)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            PropertyInfo[] pInfos = @object.GetType().GetProperties();
            foreach (var propertyInfo in pInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(true)
                    .Where(x => x.GetType() == typeof(TAttribute));

                if (attributes.Count() > 0)
                {
                    var attribute = (TAttribute)propertyInfo.GetCustomAttributes(typeof(TAttribute), false).First();
                    action(attribute, propertyInfo.GetValue(@object, null), propertyInfo.PropertyType);
                }
            }
        }
        public static void GetPropertiesMarkAttribute<TAttribute>(this object @object, Action<TAttribute, PropertyInfo> action)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            PropertyInfo[] pInfos = @object.GetType().GetProperties();
            foreach (var propertyInfo in pInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(true)
                    .Where(x => x.GetType() == typeof(TAttribute));

                if (attributes.Count() > 0)
                {
                    var attribute = (TAttribute)propertyInfo.GetCustomAttributes(typeof(TAttribute), false).First();
                    action(attribute, propertyInfo);
                }
            }
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;

            return att != null ? valueSelector(att) : default(TValue);
        }

        public static bool HasAttribute<TAttribute>(this Type type)
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).Any();
        }
        public static bool HasAttribute<TAttribute>(this object @object)
        {
            return HasAttribute<TAttribute>(@object.GetType());
        }

        public static bool HasInterface<TInterface>(this Type type)
        {
            return type.GetInterface(typeof(TInterface).Name) != null;
        }
        public static bool HasInterface<TInterface>(this object @object)
        {
            return HasInterface<TInterface>(@object.GetType());
        }

        public static TResult InvokeGenericMethod<TResult>(this object evokedObject, Type genericType, string methodName, object[] parameters = null)
        {
            var result = evokedObject?.GetType().GetMethod(methodName)?.MakeGenericMethod(genericType)
                .Invoke(evokedObject, parameters);

            return (TResult)result;
        }

        public static object CreateGenericCollection(this IList collection, Type typeOfGenericList, object ctorParameter = null)
        {
            Type itemType = collection.GetType().GetProperty("Item").PropertyType;
            Type observableCollectionType = typeOfGenericList.MakeGenericType(itemType);
            return Activator.CreateInstance(observableCollectionType, ctorParameter);
        }
    }
}
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public static class Extensions
{
	public static T Copy<T>(this T source, Transform parent = null) where T : Component
	{
		if (source == null)
		{
			Debug.LogError("Null prefab");
			return null;
		}

		return source.gameObject.Copy(parent).GetComponent(source.GetType()) as T;
	}

	public static GameObject Copy(this GameObject source, Transform parent = null)
	{
		if (source == null)
		{
			Debug.LogError("Null prefab");
			return null;
		}
		var copy = UnityEngine.Object.Instantiate(source, parent);
		copy.name = source.name;
		copy.SetActive(true);
		foreach (var component in copy.GetComponents<Component>())
		{
			if (component is Transform) continue;
			if (component is RectTransform) continue;
			component.gameObject.SetActive(true);
		}
		return copy;
	}

	public static T GetRandomElement<T>(this IEnumerable<T> source, bool throwException = true)
	{
		if (source == null || !source.Any())
		{
			if (throwException)
			{
				throw new InvalidOperationException("Cannot select a random element from an empty or null collection.");
			}

			return default;
		}

		var random = new System.Random();

		var index = random.Next(0, source.Count());
		return source.ElementAt(index);
	}

	public static int GetRandomIndex<T>(this IEnumerable<T> source, bool throwException = true)
	{
		if (source == null || !source.Any())
		{
			if (throwException)
			{
				throw new InvalidOperationException("Cannot select a random element from an empty or null collection.");
			}
			return -1;
		}
		var random = new System.Random();
		var index = random.Next(0, source.Count());
		return index;
	}
}

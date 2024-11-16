using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections;
using System.Reflection;

namespace UExpo.Application.Utils;


public static class ReflectionHelper
{
	public static bool HasNullOrEmptyFields(this object obj)
	{
		if (obj == null) throw new ArgumentNullException(nameof(obj));

		var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

		foreach (var property in properties)
		{
			if (!property.CanRead) continue;

			var value = property.GetValue(obj);

			// Verificar se o campo é nulo
			if (value == null || (value is string vStr && string.IsNullOrEmpty(vStr)))
			{
				return true;
			}

			// Verificar se o campo é uma lista vazia
			if (value is IEnumerable enumerable && !(value is string))
			{
				// Verificar se a lista está vazia
				if (!enumerable.Cast<object>().Any())
				{
					return true;
				}
			}
		}

		return false; // Nenhum campo nulo ou lista vazia encontrado
	}
}

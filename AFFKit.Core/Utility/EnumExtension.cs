using System.ComponentModel;
using System.Reflection;

namespace AFFKit.Core.Utility;

public static class EnumExtension
{
	public static string GetDescription<T>(this T enumValue) where T : Enum
	{
		var field = enumValue.GetType().GetField(enumValue.ToString());
		var attr = field?.GetCustomAttribute<DescriptionAttribute>();
		return attr?.Description ?? enumValue.ToString();
	}
	
	public static T ParseByDescription<T>(string description, bool strict = false) where T : Enum
	{
		foreach (var field in typeof(T).GetFields())
		{
			var attr = field.GetCustomAttribute<DescriptionAttribute>();
			if (attr != null && attr.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
			{
				return (T)field.GetValue(null)!;
			}
		}

		if (strict)
		{
			throw new ArgumentException($"No matching enum value found for description '{description}' in enum type '{typeof(T).Name}'.", nameof(description));
		}

		return (T) Enum.Parse(typeof(T), description, true);
	}
	
	public static object ParseByDescription(this Type enumType, string description, bool strict = false)
	{
		foreach (var field in enumType.GetFields())
		{
			var attr = field.GetCustomAttribute<DescriptionAttribute>();
			if (attr != null && attr.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
			{
				return field.GetValue(null)!;
			}
		}
		
		if (strict)
		{
			throw new ArgumentException($"No matching enum value found for description '{description}' in enum type '{enumType.Name}'.", nameof(description));
		}
		
		return Enum.Parse(enumType, description, true);
	}
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Goodbyte.TradingSystem.Domain.Common
{
	// Token: 0x02000084 RID: 132
	public static class EnumHelper
	{
		// Token: 0x06000796 RID: 1942 RVA: 0x00019204 File Offset: 0x00017404
		public static string GetEnumDescription(Enum value)
		{
			if (value == null)
			{
				return null;
			}
			object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (customAttributes.Length == 0)
			{
				return value.ToString();
			}
			return ((DescriptionAttribute)customAttributes[0]).Description;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00019250 File Offset: 0x00017450
		public static T GetEnumFromDescription<T>(string stringValue) where T : struct
		{
			if (stringValue == null)
			{
				return default(T);
			}
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				if (EnumHelper.GetEnumDescription((Enum)obj).Equals(stringValue))
				{
					return (T)((object)obj);
				}
			}
			throw new ArgumentException();
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x000192D8 File Offset: 0x000174D8
		public static IEnumerable<string> GetEnumDescriptions(Type enumType)
		{
			if (enumType == null)
			{
				return null;
			}
			Collection<string> collection = new Collection<string>();
			foreach (object obj in Enum.GetValues(enumType))
			{
				Enum value = (Enum)obj;
				collection.Add(EnumHelper.GetEnumDescription(value));
			}
			return collection;
		}
	}
}

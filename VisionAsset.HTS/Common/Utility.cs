using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using Goodbyte.TradingSystem.Domain.Entities;

namespace Goodbyte.TradingSystem.Domain.Common
{
	// Token: 0x02000086 RID: 134
	public static class Utility
	{
		// Token: 0x0600079A RID: 1946 RVA: 0x0001936C File Offset: 0x0001756C
		public static string GetLeftString(string target, int length)
		{
			if (length <= target.Length)
			{
				return target.Substring(0, length);
			}
			return target;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00019381 File Offset: 0x00017581
		public static string GetMidString(string target, int start)
		{
			if (start <= target.Length)
			{
				return target.Substring(start - 1);
			}
			return string.Empty;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001939B File Offset: 0x0001759B
		public static string GetMidString(string target, int start, int length)
		{
			if (start > target.Length)
			{
				return string.Empty;
			}
			if (start + length - 1 <= target.Length)
			{
				return target.Substring(start - 1, length);
			}
			return target.Substring(start - 1);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x000193CD File Offset: 0x000175CD
		public static string GetRightString(string target, int length)
		{
			if (length <= target.Length)
			{
				return target.Substring(target.Length - length);
			}
			return target;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x000193E8 File Offset: 0x000175E8
		public static string GetPriceStringFormat(int precision)
		{
			string text = "0.";
			for (int i = 0; i < precision; i++)
			{
				text += "0";
			}
			return text;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x00019414 File Offset: 0x00017614
		public static double GetOptionPrice(string symbol)
		{
			string text = Utility.GetRightString(symbol, 3);
			if (Utility.GetRightString(text, 1) == "2" || Utility.GetRightString(text, 1) == "7")
			{
				text += ".5";
			}
			return Convert.ToDouble(text);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00019464 File Offset: 0x00017664
		public static string GetSymbolCode(Item item)
		{
			switch (item.ItemType)
			{
			case ItemType.Futures:
				return Utility.GetLeftString(item.Symbol, 3);
			case ItemType.Options:
				return Utility.GetLeftString(item.Symbol, 3);
			case ItemType.Cme:
				return Utility.GetLeftString(item.Symbol, 3);
			case ItemType.Eurex:
				return Utility.GetLeftString(item.Symbol, 3);
			case ItemType.Foreign:
				return item.Symbol.Replace(Utility.GetRightString(item.Symbol, 3), "");
			case ItemType.Kospi:
				return item.Symbol;
			case ItemType.Kosdaq:
				return item.Symbol;
			default:
				return string.Empty;
			}
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00019500 File Offset: 0x00017700
		public static int GetDelaySeconds(int hmmss)
		{
			DateTime now = DateTime.Now;
			int num = now.Minute * 60 + now.Second;
			int num2 = hmmss % 10000 / 100 * 60 + hmmss % 100;
			int num3 = num - num2;
			if (3540 < num3)
			{
				num3 -= 3600;
			}
			else if (num3 < -3540)
			{
				num3 += 3600;
			}
			return num3;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00019560 File Offset: 0x00017760
		public static int GetKospiItemReferencePoint(double price)
		{
			int[] array = new int[]
			{
				0,
				1000,
				5000,
				10000,
				50000,
				100000,
				500000
			};
			int result = 0;
			int num = int.MaxValue;
			foreach (int num2 in array)
			{
				if (Math.Abs(num2 - Convert.ToInt32(price)) < num)
				{
					num = Math.Abs(num2 - Convert.ToInt32(price));
					result = num2;
				}
			}
			return result;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000195C0 File Offset: 0x000177C0
		public static int GetKosdaqItemReferencePoint(double price)
		{
			int[] array = new int[]
			{
				0,
				1000,
				5000,
				10000,
				50000
			};
			int result = 0;
			int num = int.MaxValue;
			foreach (int num2 in array)
			{
				if (Math.Abs(num2 - Convert.ToInt32(price)) < num)
				{
					num = Math.Abs(num2 - Convert.ToInt32(price));
					result = num2;
				}
			}
			return result;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00019620 File Offset: 0x00017820
		public static bool IsMatchInitialSound(string target, string initialSound)
		{
			if (string.IsNullOrEmpty(initialSound))
			{
				return true;
			}
			target = target.ToUpper();
			initialSound = initialSound.ToUpper();
			char[] array = new char[]
			{
				'ㄱ',
				'ㄲ',
				'ㄴ',
				'ㄷ',
				'ㄸ',
				'ㄹ',
				'ㅁ',
				'ㅂ',
				'ㅃ',
				'ㅅ',
				'ㅆ',
				'ㅇ',
				'ㅈ',
				'ㅉ',
				'ㅊ',
				'ㅋ',
				'ㅌ',
				'ㅍ',
				'ㅎ'
			};
			string[] array2 = new string[]
			{
				"가",
				"까",
				"나",
				"다",
				"따",
				"라",
				"마",
				"바",
				"빠",
				"사",
				"싸",
				"아",
				"자",
				"짜",
				"차",
				"카",
				"타",
				"파",
				"하"
			};
			int[] array3 = new int[]
			{
				44032,
				44620,
				45208,
				45796,
				46384,
				46972,
				47560,
				48148,
				48736,
				49324,
				49912,
				50500,
				51088,
				51676,
				52264,
				52852,
				53440,
				54028,
				54616,
				55204
			};
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < initialSound.Length; i++)
			{
				if ('ㄱ' <= initialSound[i] && initialSound[i] <= 'ㅎ')
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (initialSound[i] == array[j])
						{
							stringBuilder.AppendFormat("[{0}-{1}]", array2[j], (char)(array3[j + 1] - 1));
						}
					}
				}
				else if ('가' <= initialSound[i])
				{
					int num = (int)((initialSound[i] - '가') % 'Ɍ');
					if (num == 0)
					{
						stringBuilder.AppendFormat("[{0}-{1}]", initialSound[i], initialSound[i] + '\u001b');
					}
					else
					{
						num = 27 - num % 28;
						stringBuilder.AppendFormat("[{0}-{1}]", initialSound[i], (char)((int)initialSound[i] + num));
					}
				}
				else if ('A' <= initialSound[i] && initialSound[i] <= 'z')
				{
					stringBuilder.Append(initialSound[i]);
				}
				else if ('0' <= initialSound[i] && initialSound[i] <= '9')
				{
					stringBuilder.Append(initialSound[i]);
				}
			}
			return Regex.IsMatch(target, stringBuilder.ToString());
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00019884 File Offset: 0x00017A84
		public static DataTable ListToDataTable<T>(IList<T> data)
		{
			DataTable dataTable = new DataTable();
			if (typeof(T).IsValueType || typeof(T) == typeof(string))
			{
				DataColumn column = new DataColumn("Value");
				dataTable.Columns.Add(column);
				using (IEnumerator<T> enumerator = data.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						T t = enumerator.Current;
						DataRow dataRow = dataTable.NewRow();
						dataRow[0] = t;
						dataTable.Rows.Add(dataRow);
					}
					return dataTable;
				}
			}
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
			foreach (object obj in properties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				dataTable.Columns.Add(propertyDescriptor.Name, Nullable.GetUnderlyingType(propertyDescriptor.PropertyType) ?? propertyDescriptor.PropertyType);
			}
			foreach (T t2 in data)
			{
				DataRow dataRow2 = dataTable.NewRow();
				foreach (object obj2 in properties)
				{
					PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
					try
					{
						dataRow2[propertyDescriptor2.Name] = (propertyDescriptor2.GetValue(t2) ?? DBNull.Value);
					}
					catch (Exception)
					{
						dataRow2[propertyDescriptor2.Name] = DBNull.Value;
					}
				}
				dataTable.Rows.Add(dataRow2);
			}
			return dataTable;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00019A90 File Offset: 0x00017C90
		public static bool IsConnectedNetwork()
		{
			bool result;
			try
			{
				PingReply pingReply = new Ping().Send("www.google.com", 120, Encoding.ASCII.GetBytes("PingTest"), new PingOptions
				{
					DontFragment = true
				});
				if (pingReply != null && pingReply.Status == IPStatus.Success)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00019AF4 File Offset: 0x00017CF4
		public static string GetRealIp()
		{
			string result;
			try
			{
				result = new StreamReader(WebRequest.Create("http://218.239.223.11/GetPublicIp.aspx").GetResponse().GetResponseStream()).ReadToEnd();
			}
			catch (Exception ex)
			{
				TraceEx.TraceException(ex);
				result = "0.0.0.0";
			}
			return result;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00019B40 File Offset: 0x00017D40
		public static string GetMacAddress()
		{
			string result = string.Empty;
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				if (allNetworkInterfaces.Any<NetworkInterface>())
				{
					result = allNetworkInterfaces[0].GetPhysicalAddress().ToString();
				}
			}
			catch (Exception ex)
			{
				TraceEx.TraceException(ex);
			}
			return result;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00019B8C File Offset: 0x00017D8C
		public static string GetUserDomainName()
		{
			return Environment.UserDomainName;
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00019B93 File Offset: 0x00017D93
		public static string GetOsVersion()
		{
			return Environment.OSVersion.VersionString;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00019B9F File Offset: 0x00017D9F
		public static string GetOsUserName()
		{
			return Environment.UserName;
		}
	}
}

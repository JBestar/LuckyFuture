using Goodbyte.TradingSystem.Client.BatchService;
using Goodbyte.TradingSystem.Client.RealTimeService;
using Goodbyte.TradingSystem.Domain.Common;
using Goodbyte.TradingSystem.Domain.Entities;
using Goodbyte.TradingSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Goodbyte.TradingSystem.Client.Common
{
	[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class RealTimeData : IDisposable, IRealTimeServiceCallback
	{
		// Token: 0x06000E2D RID: 3629 RVA: 0x000053D8 File Offset: 0x000035D8
		private RealTimeData()
		{
		}


		private readonly Guid _guid = Guid.NewGuid();

		// Token: 0x06000E2E RID: 3630 RVA: 0x0004D18D File Offset: 0x0004B38D
		public void SendQuote(Quote quote)
		{
			ClientState.Quote = quote;
			if (this.ReceiveQuote != null)
				this.ReceiveQuote(this, new ReceiveEventArgs(quote));
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0004D1A9 File Offset: 0x0004B3A9
		public void SendCurrent(Current current)
		{
			ClientState.Current = current;
			ClientState.ItemCurrents[ClientState.Item.ItemId] = current;
			if (this.ReceiveCurrent != null)
				this.ReceiveCurrent(this, new ReceiveEventArgs(current));
			ClientState.OldItemCurrents[ClientState.Item.ItemId] = current;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0004D1C5 File Offset: 0x0004B3C5
		public void SendOrder(OrderResult orderResult)
		{
			ClientState.Order = orderResult.Order;
			ClientState.Orders = orderResult.UnclearOrders;
			ClientState.UserAccounts = orderResult.UserAccounts;
			ClientState.DayProfitLoss = orderResult.DayProfitLoss;

			if (this.ReceiveOrder != null)
				this.ReceiveOrder(this, new ReceiveEventArgs(orderResult));
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0004D1E1 File Offset: 0x0004B3E1
		public void SendMitOrder(MitOrderResult mitOrderResult)
		{
			if (this.ReceiveMitOrder != null)
				this.ReceiveMitOrder(this, new ReceiveEventArgs(mitOrderResult));
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0004D1FD File Offset: 0x0004B3FD
		public void SendChangeStateSignal(long userId)
		{
			if (this.ReceiveChangeStateSignal != null)
				this.ReceiveChangeStateSignal(this, new ReceiveEventArgs(userId));
		}

		// Token: 0x14000163 RID: 355
		// (add) Token: 0x06000E33 RID: 3635 RVA: 0x0004D220 File Offset: 0x0004B420
		// (remove) Token: 0x06000E34 RID: 3636 RVA: 0x0004D258 File Offset: 0x0004B458
		public event EventHandler<ReceiveEventArgs> ReceiveQuote;

		// Token: 0x14000164 RID: 356
		// (add) Token: 0x06000E35 RID: 3637 RVA: 0x0004D290 File Offset: 0x0004B490
		// (remove) Token: 0x06000E36 RID: 3638 RVA: 0x0004D2C8 File Offset: 0x0004B4C8
		public event EventHandler<ReceiveEventArgs> ReceiveCurrent;

		// Token: 0x14000165 RID: 357
		// (add) Token: 0x06000E37 RID: 3639 RVA: 0x0004D300 File Offset: 0x0004B500
		// (remove) Token: 0x06000E38 RID: 3640 RVA: 0x0004D338 File Offset: 0x0004B538
		public event EventHandler<ReceiveEventArgs> ReceiveOrder;

		// Token: 0x14000166 RID: 358
		// (add) Token: 0x06000E39 RID: 3641 RVA: 0x0004D370 File Offset: 0x0004B570
		// (remove) Token: 0x06000E3A RID: 3642 RVA: 0x0004D3A8 File Offset: 0x0004B5A8
		public event EventHandler<ReceiveEventArgs> ReceiveMitOrder;

		// Token: 0x14000167 RID: 359
		// (add) Token: 0x06000E3B RID: 3643 RVA: 0x0004D3E0 File Offset: 0x0004B5E0
		// (remove) Token: 0x06000E3C RID: 3644 RVA: 0x0004D418 File Offset: 0x0004B618
		public event EventHandler<ReceiveEventArgs> ReceiveChangeStateSignal;
        
        // Token: 0x06000E3F RID: 3647 RVA: 0x0004D4BD File Offset: 0x0004B6BD
        public static RealTimeData GetInstance()
		{
			return RealTimeData.Instance;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0004D4C4 File Offset: 0x0004B6C4
		public bool Start(List<ItemInfo> itemInfos, string itemSymbol)
		{
			this._context = new InstanceContext(this);
			this._realTimeProxy = new RealTimeServiceClient(this._context);
			ClientState.SessionId = this._realTimeProxy.Subscribe(ClientState.Certification);
			return LoadSpeedViewInfo(itemInfos, itemSymbol);
		}

		bool LoadSpeedViewInfo(List<ItemInfo> itemInfos, string itemSymbol)
		{
			try
			{
				// load speed view info
				BatchServiceClient batchServiceClient = new BatchServiceClient();
				LoadSpeedOrderViewResult loadSpeedOrderViewResult = batchServiceClient.LoadSpeedOrderView(
					ClientState.Certification,
					ClientState.CompanyId,
					ClientState.UserId
				);

				ClientState.Company = loadSpeedOrderViewResult.Company;
				ClientState.Items = loadSpeedOrderViewResult.Items;
				ClientState.OptionAtm = loadSpeedOrderViewResult.OptionAtm;
				ClientState.Markets = loadSpeedOrderViewResult.Markets;
				ClientState.ItemCurrents = new Dictionary<long, Current>();
				ClientState.OldItemCurrents = new Dictionary<long, Current>();
				ClientState.CurrentUser = loadSpeedOrderViewResult.User;
				ClientState.UserAccounts = loadSpeedOrderViewResult.UserAccounts;
				ClientState.UserAccountSpecs = loadSpeedOrderViewResult.UserAccountSpecifics;

                ItemInfo itemInfo = null;
                for (int i = 0; i < ClientState.Items.Count; i++)
                {
                    if (ClientState.Items[i].ItemName.Length > 0)
                    {
                        itemInfo = new ItemInfo()
                        {
                            ItemId = ClientState.Items[i].ItemId,
                            ItemName = ClientState.Items[i].ItemName,
                            Symbol = ClientState.Items[i].Symbol,
							Precision = ClientState.Items[i].PricePrecision,
                        };
                        itemInfos.Add(itemInfo);

                        if (itemSymbol == "" && itemInfo.ItemName.Contains("NASDAQ"))
                            itemSymbol = itemInfo.Symbol;
                    }

                }

                if (itemSymbol == "" && itemInfos.Count > 0)
                    itemSymbol = itemInfos[0].Symbol;

                if (itemSymbol == "")
                {
                    return false;
                }

                foreach (KeyValuePair<long, Current> keyValuePair in loadSpeedOrderViewResult.Currents)
					ClientState.ItemCurrents.Add(keyValuePair.Key, keyValuePair.Value);
				foreach (KeyValuePair<long, Current> keyValuePair2 in loadSpeedOrderViewResult.Currents)
					ClientState.OldItemCurrents.Add(keyValuePair2.Key, keyValuePair2.Value);
				batchServiceClient.Close();

				ClientState.Item = ClientState.Items.FirstOrDefault(i => i.Symbol == itemSymbol);
				ClientState.Market = ClientState.Markets.FirstOrDefault(m => m.ItemId == ClientState.Item.ItemId);
				ClientState.UserId = ClientState.UserAccounts.Count > 0 ? ClientState.UserAccounts[0].UserId : 0;

				if (ClientState.UserAccounts.Count > 0 && ClientState.Item != null)
					LoadItem();

				SubscribeRealTimeData(true);
			}
			catch(Exception e)
			{
				TraceEx.TraceException(e);
				return false;
			}
			return true;
		}

		private void LoadItem()
		{
			BatchServiceClient batchServiceClient = new BatchServiceClient();
			try
			{
				LoadItemResult loadItemResult = batchServiceClient.LoadItem(
					ClientState.Certification, 
					ClientState.Item.ItemId,
					ClientState.UserAccounts[0].UserAccountId
				);
				ClientState.Quote = loadItemResult.Quote;
				ClientState.OldQuote = loadItemResult.Quote;
				ClientState.ItemCurrents[ClientState.Item.ItemId] = loadItemResult.Current;
				ClientState.Current = loadItemResult.Current;
				ClientState.Orders = loadItemResult.Orders;
				ClientState.MitOrders = loadItemResult.MitOrders;
				ClientState.DayProfitLoss = loadItemResult.DayProfitLoss;
				batchServiceClient.Close();
			}
			catch (Exception ex)
			{
				TraceEx.TraceException(ex);
				batchServiceClient.Abort();
			}			
		}

		private void SubscribeRealTimeData(bool isReconnect)
		{
			Dictionary<long, Item> dictionary = new Dictionary<long, Item>();
			dictionary.Add(ClientState.Item.ItemId, ClientState.Item);			
			RealTimeData.GetInstance().Subscribe(this._guid, dictionary, isReconnect);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0004D540 File Offset: 0x0004B740
		public void Stop()
		{
			if (this._realTimeProxy != null)
			{
				try
				{
					this._realTimeProxy.Unsubscribe();
					this._realTimeProxy.Close();
				}
				catch (Exception ex)
				{
					TraceEx.TraceException(ex);
				}
			}
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0004D5C8 File Offset: 0x0004B7C8
		public void Subscribe(Guid guid, Dictionary<long, Item> items, bool isReconnect)
		{
			if (this._realTimeProxy == null)
			{
				return;
			}
			if (RealTimeData.ViewSubscriptionItems.ContainsKey(guid))
			{
				int num = RealTimeData.ViewSubscriptionItems[guid].SequenceEqual(items) ? 1 : 0;
				RealTimeData.ViewSubscriptionItems[guid] = items;
				if (num != 0 && !isReconnect)
				{
					return;
				}
				RealTimeServiceClient realTimeServiceClient = new RealTimeServiceClient(this._context);
				try
				{
					realTimeServiceClient.UpdateClientItems(ClientState.SessionId, this.GetSubscriptionItems());
					realTimeServiceClient.Close();
					return;
				}
				catch (Exception ex)
				{
					TraceEx.TraceException(ex);
					return;
				}
			}
			RealTimeData.ViewSubscriptionItems.Add(guid, items);
			RealTimeServiceClient realTimeServiceClient2 = new RealTimeServiceClient(this._context);
			try
			{
				realTimeServiceClient2.UpdateClientItems(ClientState.SessionId, this.GetSubscriptionItems());
				realTimeServiceClient2.Close();
			}
			catch (Exception ex2)
			{
				TraceEx.TraceException(ex2);
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0004D69C File Offset: 0x0004B89C
		public void Unsubscribe(Guid guid)
		{
			if (this._realTimeProxy == null)
			{
				return;
			}
			if (RealTimeData.ViewSubscriptionItems.ContainsKey(guid))
			{
				RealTimeData.ViewSubscriptionItems.Remove(guid);
				RealTimeServiceClient realTimeServiceClient = new RealTimeServiceClient(this._context);
				try
				{
					realTimeServiceClient.UpdateClientItems(ClientState.SessionId, this.GetSubscriptionItems());
					realTimeServiceClient.Close();
				}
				catch (Exception ex)
				{
					TraceEx.TraceException(ex);
				}
			}
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0004D710 File Offset: 0x0004B910
		private Dictionary<long, Item> GetSubscriptionItems()
		{
			Dictionary<long, Item> dictionary = new Dictionary<long, Item>();
			foreach (KeyValuePair<Guid, Dictionary<long, Item>> keyValuePair in RealTimeData.ViewSubscriptionItems)
			{
				foreach (KeyValuePair<long, Item> keyValuePair2 in keyValuePair.Value)
				{
					if (!dictionary.ContainsKey(keyValuePair2.Key))
					{
						dictionary.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0004D7C4 File Offset: 0x0004B9C4
		public bool CheckConnection()
		{
			if (this._isRunCheckService)
				return true;

			this._isRunCheckService = true;
			try
			{
				if(this._realTimeProxy != null && this._realTimeProxy.State != CommunicationState.Opened)
					Close();
				try
				{
					this._realTimeProxy.ConnectionTest();
				}
				catch (Exception e)
				{
					TraceEx.TraceException(e);
					Close();
				}
			}
			finally
			{
				this._isRunCheckService = false;
			}
			return this._realTimeProxy != null;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0004D984 File Offset: 0x0004BB84
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Close()
		{
			if (this._realTimeProxy != null)
			{
				try
				{
					this._realTimeProxy.Close();
				}
				catch (Exception)
				{
					this._realTimeProxy.Abort();
				}
			}
			_realTimeProxy = null;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0004D994 File Offset: 0x0004BB94
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				if (this._realTimeProxy != null && this._realTimeProxy.State != CommunicationState.Closed)
				{
					try
					{
						this._realTimeProxy.Close();
					}
					catch (Exception ex)
					{
						TraceEx.TraceException(ex);
						this._realTimeProxy.Abort();
					}
				}
			}
			this._disposed = true;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0004DA18 File Offset: 0x0004BC18
		~RealTimeData()
		{
			this.Dispose(false);
		}

		// Token: 0x040004D4 RID: 1236
		private static readonly RealTimeData Instance = new RealTimeData();

		// Token: 0x040004D5 RID: 1237
		private static readonly Dictionary<Guid, Dictionary<long, Item>> ViewSubscriptionItems = new Dictionary<Guid, Dictionary<long, Item>>();

		// Token: 0x040004D6 RID: 1238
		private InstanceContext _context;

		// Token: 0x040004D7 RID: 1239
		private bool _isRunCheckService;

		// Token: 0x040004D8 RID: 1240
		private RealTimeServiceClient _realTimeProxy;

		// Token: 0x040004E0 RID: 1248
		private bool _disposed;
	}
}

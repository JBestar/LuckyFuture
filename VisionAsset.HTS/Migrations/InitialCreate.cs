using System;
using System.CodeDom.Compiler;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Resources;

namespace Goodbyte.TradingSystem.Domain.Migrations
{
	// Token: 0x0200004D RID: 77
	[GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
	public sealed class InitialCreate : DbMigration, IMigrationMetadata
	{
		// Token: 0x060003BE RID: 958 RVA: 0x00013974 File Offset: 0x00011B74
		public override void Up()
		{
			base.CreateTable("dbo.Admins", (ColumnBuilder c) => new
			{
				AdminId = c.String(new bool?(false), new int?(128), null, null, null, null, null, null, null),
				AdminPassword = c.String(null, null, null, null, null, null, null, null, null),
				AdminName = c.String(null, null, null, null, null, null, null, null, null),
				NickName = c.String(null, null, null, null, null, null, null, null, null),
				Phone = c.String(null, null, null, null, null, null, null, null, null),
				Email = c.String(null, null, null, null, null, null, null, null, null),
				AdminType = c.Int(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.AdminId, null, true, null);
			base.CreateTable("dbo.Companies", (ColumnBuilder c) => new
			{
				CompanyId = c.Long(new bool?(false), true, null, null, null, null, null),
				CompanyName = c.String(null, null, null, null, null, null, null, null, null),
				CustomerService = c.String(null, null, null, null, null, null, null, null, null),
				OperatingHourInfo = c.String(null, null, null, null, null, null, null, null, null),
				WebSiteUrl = c.String(null, null, null, null, null, null, null, null, null),
				IsSummerTime = c.Boolean(new bool?(false), null, null, null, null, null)
			}, null).PrimaryKey(t => t.CompanyId, null, true, null);
			base.CreateTable("dbo.Certifications", (ColumnBuilder c) => new
			{
				Id = c.String(new bool?(false), new int?(128), null, null, null, null, null, null, null),
				CertificationId = c.Long(new bool?(false), false, null, null, null, null, null),
				Password = c.String(null, null, null, null, null, null, null, null, null),
				CompanyId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.Id, null, true, null).ForeignKey("dbo.Companies", t => t.CompanyId, true, null, null).Index(t => t.CompanyId, null, false, false, null);
			base.CreateTable("dbo.CompanyAccounts", (ColumnBuilder c) => new
			{
				CompanyAccountId = c.Long(new bool?(false), true, null, null, null, null, null),
				BankName = c.String(null, null, null, null, null, null, null, null, null),
				BankUserName = c.String(null, null, null, null, null, null, null, null, null),
				BankAccount = c.String(null, null, null, null, null, null, null, null, null),
				CompanyId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.CompanyAccountId, null, true, null).ForeignKey("dbo.Companies", t => t.CompanyId, true, null, null).Index(t => t.CompanyId, null, false, false, null);
			base.CreateTable("dbo.Notices", (ColumnBuilder c) => new
			{
				NoticeId = c.Long(new bool?(false), true, null, null, null, null, null),
				Subject = c.String(null, null, null, null, null, null, null, null, null),
				Content = c.String(null, null, null, null, null, null, null, null, null),
				WriteDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				CompanyId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.NoticeId, null, true, null).ForeignKey("dbo.Companies", t => t.CompanyId, true, null, null).Index(t => t.CompanyId, null, false, false, null);
			base.CreateTable("dbo.ParentAccounts", (ColumnBuilder c) => new
			{
				ParentAccountId = c.Long(new bool?(false), true, null, null, null, null, null),
				ParentAccountType = c.Int(new bool?(false), false, null, null, null, null, null),
				ParentAccountNumber = c.String(null, null, null, null, null, null, null, null, null),
				ParentAccountPassword = c.String(null, null, null, null, null, null, null, null, null),
				CompanyId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.ParentAccountId, null, true, null).ForeignKey("dbo.Companies", t => t.CompanyId, true, null, null).Index(t => t.CompanyId, null, false, false, null);
			base.CreateTable("dbo.Users", (ColumnBuilder c) => new
			{
				UserId = c.Long(new bool?(false), true, null, null, null, null, null),
				LoginId = c.String(null, null, null, null, null, null, null, null, null),
				UserPassword = c.String(null, null, null, null, null, null, null, null, null),
				UserName = c.String(null, null, null, null, null, null, null, null, null),
				NickName = c.String(null, null, null, null, null, null, null, null, null),
				Phone = c.String(null, null, null, null, null, null, null, null, null),
				Email = c.String(null, null, null, null, null, null, null, null, null),
				BankName = c.String(null, null, null, null, null, null, null, null, null),
				BankUserName = c.String(null, null, null, null, null, null, null, null, null),
				BankAccount = c.String(null, null, null, null, null, null, null, null, null),
				UserType = c.Int(new bool?(false), false, null, null, null, null, null),
				IsBlocked = c.Boolean(new bool?(false), null, null, null, null, null),
				ExpertId = c.String(null, null, null, null, null, null, null, null, null),
				RegistrationDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				Memo = c.String(null, null, null, null, null, null, null, null, null),
				CompanyId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.UserId, null, true, null).ForeignKey("dbo.Companies", t => t.CompanyId, true, null, null).Index(t => t.CompanyId, null, false, false, null);
			base.CreateTable("dbo.Connections", (ColumnBuilder c) => new
			{
				ConnectionId = c.Long(new bool?(false), true, null, null, null, null, null),
				IpAddress = c.String(null, null, null, null, null, null, null, null, null),
				MacAddress = c.String(null, null, null, null, null, null, null, null, null),
				UserDomainName = c.String(null, null, null, null, null, null, null, null, null),
				OsVersion = c.String(null, null, null, null, null, null, null, null, null),
				OsUserName = c.String(null, null, null, null, null, null, null, null, null),
				ConnectionType = c.Int(new bool?(false), false, null, null, null, null, null),
				ConnectionDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				UserId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.ConnectionId, null, true, null).ForeignKey("dbo.Users", t => t.UserId, true, null, null).Index(t => t.UserId, null, false, false, null);
			base.CreateTable("dbo.UserAccounts", (ColumnBuilder c) => new
			{
				UserAccountId = c.Long(new bool?(false), true, null, null, null, null, null),
				Balance = c.Long(new bool?(false), false, null, null, null, null, null),
				Leverage = c.Int(new bool?(false), false, null, null, null, null, null),
				CreateDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				UserAccountStateType = c.Int(new bool?(false), false, null, null, null, null, null),
				LatestLossCutDateTime = c.DateTime(null, null, null, null, null, null, null),
				IsFuturesTrade = c.Boolean(new bool?(false), null, null, null, null, null),
				FuturesOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				FuturesCommission = c.Double(new bool?(false), null, null, null, null, null),
				FuturesMaxSellQty = c.Int(new bool?(false), false, null, null, null, null, null),
				FuturesMaxBuyQty = c.Int(new bool?(false), false, null, null, null, null, null),
				IsFuturesOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsFuturesOvernightSettingPermission = c.Boolean(new bool?(false), null, null, null, null, null),
				IsOptionTrade = c.Boolean(new bool?(false), null, null, null, null, null),
				OptionOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				OptionCommission = c.Double(new bool?(false), null, null, null, null, null),
				OptionMaxSellQty = c.Int(new bool?(false), false, null, null, null, null, null),
				OptionMaxBuyQty = c.Int(new bool?(false), false, null, null, null, null, null),
				IsOptionOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsOptionOvernightSettingPermission = c.Boolean(new bool?(false), null, null, null, null, null),
				IsCmeTrade = c.Boolean(new bool?(false), null, null, null, null, null),
				CmeOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				CmeCommission = c.Double(new bool?(false), null, null, null, null, null),
				CmeMaxSellQty = c.Int(new bool?(false), false, null, null, null, null, null),
				CmeMaxBuyQty = c.Int(new bool?(false), false, null, null, null, null, null),
				IsCmeOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsCmeOvernightSettingPermission = c.Boolean(new bool?(false), null, null, null, null, null),
				IsEurexTrade = c.Boolean(new bool?(false), null, null, null, null, null),
				EurexOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				EurexCommission = c.Double(new bool?(false), null, null, null, null, null),
				EurexMaxSellQty = c.Int(new bool?(false), false, null, null, null, null, null),
				EurexMaxBuyQty = c.Int(new bool?(false), false, null, null, null, null, null),
				IsEurexOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsEurexOvernightSettingPermission = c.Boolean(new bool?(false), null, null, null, null, null),
				IsForeignTrade = c.Boolean(new bool?(false), null, null, null, null, null),
				ForeignOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				ForeignCommission = c.Double(new bool?(false), null, null, null, null, null),
				ForeignMaxSellQty = c.Int(new bool?(false), false, null, null, null, null, null),
				ForeignMaxBuyQty = c.Int(new bool?(false), false, null, null, null, null, null),
				IsForeignOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsForeignOvernightSettingPermission = c.Boolean(new bool?(false), null, null, null, null, null),
				IsKospiTrade = c.Boolean(new bool?(false), null, null, null, null, null),
				KospiOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				KospiCommission = c.Double(new bool?(false), null, null, null, null, null),
				KospiMaxSellMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				KospiMaxBuyMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				IsKospiOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsKospiOvernightSettingPermission = c.Boolean(new bool?(false), null, null, null, null, null),
				IsKosdaqTrade = c.Boolean(new bool?(false), null, null, null, null, null),
				KosdaqOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				KosdaqCommission = c.Double(new bool?(false), null, null, null, null, null),
				KosdaqMaxSellMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				KosdaqMaxBuyMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				IsKosdaqOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsKosdaqOvernightSettingPermission = c.Boolean(new bool?(false), null, null, null, null, null),
				UserId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.UserAccountId, null, true, null).ForeignKey("dbo.Users", t => t.UserId, true, null, null).Index(t => t.UserId, null, false, false, null);
			base.CreateTable("dbo.DayProfitLosses", (ColumnBuilder c) => new
			{
				DayProfitLossId = c.Long(new bool?(false), true, null, null, null, null, null),
				MarketDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				OpenBalance = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalDeposit = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalWithdraw = c.Long(new bool?(false), false, null, null, null, null, null),
				FuturesRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				FuturesRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				FuturesParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				OptionRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				OptionRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				OptionParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				CmeRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				CmeRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				CmeParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				EurexRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				EurexRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				EurexParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				ForeignRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				ForeignRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				ForeignParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				KospiRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				KospiRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				KospiParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				KosdaqRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				KosdaqRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				KosdaqParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				FuturesVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				FuturesVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				OptionVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				OptionVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				CmeVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				CmeVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				EurexVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				EurexVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				ForeignVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				ForeignVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				KospiVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				KospiVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				KosdaqVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				KosdaqVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalRealProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalRealCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalParentCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalVirtualProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalVirtualCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalProfit = c.Long(new bool?(false), false, null, null, null, null, null),
				TotalCommission = c.Long(new bool?(false), false, null, null, null, null, null),
				CloseBalance = c.Long(new bool?(false), false, null, null, null, null, null),
				UserAccountId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.DayProfitLossId, null, true, null).ForeignKey("dbo.UserAccounts", t => t.UserAccountId, true, null, null).Index(t => t.UserAccountId, null, false, false, null);
			base.CreateTable("dbo.DepositWithdraws", (ColumnBuilder c) => new
			{
				DepositWithdrawId = c.Long(new bool?(false), true, null, null, null, null, null),
				MarketDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				RequestAmount = c.Long(new bool?(false), false, null, null, null, null, null),
				RequestDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				ApplyAmount = c.Long(new bool?(false), false, null, null, null, null, null),
				ApplyDate = c.DateTime(null, null, null, null, null, null, null),
				DepositWithdrawType = c.Int(new bool?(false), false, null, null, null, null, null),
				DepositWithdrawStateType = c.Int(new bool?(false), false, null, null, null, null, null),
				Memo = c.String(null, null, null, null, null, null, null, null, null),
				UserAccountId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.DepositWithdrawId, null, true, null).ForeignKey("dbo.UserAccounts", t => t.UserAccountId, true, null, null).Index(t => t.UserAccountId, null, false, false, null);
			base.CreateTable("dbo.MitOrders", (ColumnBuilder c) => new
			{
				MitOrderId = c.Long(new bool?(false), true, null, null, null, null, null),
				RootMitOrderId = c.Long(new bool?(false), false, null, null, null, null, null),
				MitOrderType = c.Int(new bool?(false), false, null, null, null, null, null),
				TradeType = c.Int(new bool?(false), false, null, null, null, null, null),
				Qty = c.Int(new bool?(false), false, null, null, null, null, null),
				Price = c.Double(new bool?(false), null, null, null, null, null),
				IsReverse = c.Boolean(new bool?(false), null, null, null, null, null),
				ProcessDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				NotExecuteQty = c.Int(new bool?(false), false, null, null, null, null, null),
				Current = c.Double(new bool?(false), null, null, null, null, null),
				Symbol = c.String(null, null, null, null, null, null, null, null, null),
				MarketId = c.Long(new bool?(false), false, null, null, null, null, null),
				UserAccountId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.MitOrderId, null, true, null).ForeignKey("dbo.Markets", t => t.MarketId, true, null, null).ForeignKey("dbo.UserAccounts", t => t.UserAccountId, true, null, null).Index(t => t.RootMitOrderId, null, false, false, null).Index(t => t.NotExecuteQty, null, false, false, null).Index(t => t.MarketId, null, false, false, null).Index(t => t.UserAccountId, null, false, false, null);
			base.CreateTable("dbo.Markets", (ColumnBuilder c) => new
			{
				MarketId = c.Long(new bool?(false), true, null, null, null, null, null),
				MarketDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				OpenSynchronizedTime = c.DateTime(new bool?(false), null, null, null, null, null, null),
				OpenTime = c.DateTime(new bool?(false), null, null, null, null, null, null),
				EndOrderTime = c.DateTime(new bool?(false), null, null, null, null, null, null),
				CloseTime = c.DateTime(new bool?(false), null, null, null, null, null, null),
				PauseTime1 = c.DateTime(new bool?(false), null, null, null, null, null, null),
				ReopenTime1 = c.DateTime(new bool?(false), null, null, null, null, null, null),
				PauseTime2 = c.DateTime(new bool?(false), null, null, null, null, null, null),
				ReopenTime2 = c.DateTime(new bool?(false), null, null, null, null, null, null),
				MarketStateType = c.Int(new bool?(false), false, null, null, null, null, null),
				ItemId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.MarketId, null, true, null).ForeignKey("dbo.Items", t => t.ItemId, true, null, null).Index(t => t.MarketDate, null, false, false, null).Index(t => t.ItemId, null, false, false, null);
			base.CreateTable("dbo.Items", (ColumnBuilder c) => new
			{
				ItemId = c.Long(new bool?(false), true, null, null, null, null, null),
				Symbol = c.String(null, null, null, null, null, null, null, null, null),
				ItemName = c.String(null, null, null, null, null, null, null, null, null),
				ItemType = c.Int(new bool?(false), false, null, null, null, null, null),
				CurrencyType = c.Int(new bool?(false), false, null, null, null, null, null),
				ParentCommission = c.Double(new bool?(false), null, null, null, null, null),
				Leverage = c.Int(new bool?(false), false, null, null, null, null, null),
				PricePrecision = c.Int(new bool?(false), false, null, null, null, null, null),
				AveragePricePrecision = c.Int(new bool?(false), false, null, null, null, null, null),
				ReferencePoint = c.Double(new bool?(false), null, null, null, null, null),
				OverTick = c.Double(new bool?(false), null, null, null, null, null),
				UnderTick = c.Double(new bool?(false), null, null, null, null, null),
				OverTickValue = c.Double(new bool?(false), null, null, null, null, null),
				UnderTickValue = c.Double(new bool?(false), null, null, null, null, null),
				StartTradingDay = c.DateTime(new bool?(false), null, null, null, null, null, null),
				LastTradingDay = c.DateTime(new bool?(false), null, null, null, null, null, null),
				SellMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				BuyMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				SellMinimumMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				BuyMinimumMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				SellLossCutMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				BuyLossCutMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				SellOvernightMinimumMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				BuyOvernightMinimumMargin = c.Long(new bool?(false), false, null, null, null, null, null),
				OrderUpLimit = c.Double(new bool?(false), null, null, null, null, null),
				OrderDownLimit = c.Double(new bool?(false), null, null, null, null, null),
				ConclusionSpeed = c.Int(new bool?(false), false, null, null, null, null, null),
				CancelOrderTime = c.Time(new bool?(false), new byte?((byte)7), null, null, null, null, null),
				CancelOrderTick = c.Int(new bool?(false), false, null, null, null, null, null),
				IsBlankQuoteOrder = c.Boolean(new bool?(false), null, null, null, null, null),
				IsBlankQuoteConslusion = c.Boolean(new bool?(false), null, null, null, null, null),
				IsOnlyLimitPrice = c.Boolean(new bool?(false), null, null, null, null, null),
				SortIndex = c.Int(new bool?(false), false, null, null, null, null, null),
				IsUse = c.Boolean(new bool?(false), null, null, null, null, null)
			}, null).PrimaryKey(t => t.ItemId, null, true, null);
			base.CreateTable("dbo.Currents", (ColumnBuilder c) => new
			{
				CurrentId = c.Long(new bool?(false), true, null, null, null, null, null),
				Symbol = c.String(null, null, null, null, null, null, null, null, null),
				CurrentPrice = c.Double(new bool?(false), null, null, null, null, null),
				StartPrice = c.Double(new bool?(false), null, null, null, null, null),
				HighPrice = c.Double(new bool?(false), null, null, null, null, null),
				LowPrice = c.Double(new bool?(false), null, null, null, null, null),
				BeforeClosePrice = c.Double(new bool?(false), null, null, null, null, null),
				UpLimitPrice = c.Double(new bool?(false), null, null, null, null, null),
				DownLimitPrice = c.Double(new bool?(false), null, null, null, null, null),
				ConclusionVolume = c.Int(new bool?(false), false, null, null, null, null, null),
				Volume = c.Int(new bool?(false), false, null, null, null, null, null),
				TradeType = c.Int(new bool?(false), false, null, null, null, null, null),
				CurrentTime = c.String(null, null, null, null, null, null, null, null, null),
				ReceivedDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				ItemId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.CurrentId, null, true, null).ForeignKey("dbo.Items", t => t.ItemId, true, null, null).Index(t => t.ItemId, null, false, false, null);
			base.CreateTable("dbo.Quotes", (ColumnBuilder c) => new
			{
				QuoteId = c.Long(new bool?(false), true, null, null, null, null, null),
				Symbol = c.String(null, null, null, null, null, null, null, null, null),
				Ask1 = c.Double(new bool?(false), null, null, null, null, null),
				Ask2 = c.Double(new bool?(false), null, null, null, null, null),
				Ask3 = c.Double(new bool?(false), null, null, null, null, null),
				Ask4 = c.Double(new bool?(false), null, null, null, null, null),
				Ask5 = c.Double(new bool?(false), null, null, null, null, null),
				Ask6 = c.Double(new bool?(false), null, null, null, null, null),
				Ask7 = c.Double(new bool?(false), null, null, null, null, null),
				Ask8 = c.Double(new bool?(false), null, null, null, null, null),
				Ask9 = c.Double(new bool?(false), null, null, null, null, null),
				Ask10 = c.Double(new bool?(false), null, null, null, null, null),
				AskQty1 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty2 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty3 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty4 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty5 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty6 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty7 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty8 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty9 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskQty10 = c.Int(new bool?(false), false, null, null, null, null, null),
				TotalAskQty = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount1 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount2 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount3 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount4 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount5 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount6 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount7 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount8 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount9 = c.Int(new bool?(false), false, null, null, null, null, null),
				AskCount10 = c.Int(new bool?(false), false, null, null, null, null, null),
				TotalAskCount = c.Int(new bool?(false), false, null, null, null, null, null),
				Bid1 = c.Double(new bool?(false), null, null, null, null, null),
				Bid2 = c.Double(new bool?(false), null, null, null, null, null),
				Bid3 = c.Double(new bool?(false), null, null, null, null, null),
				Bid4 = c.Double(new bool?(false), null, null, null, null, null),
				Bid5 = c.Double(new bool?(false), null, null, null, null, null),
				Bid6 = c.Double(new bool?(false), null, null, null, null, null),
				Bid7 = c.Double(new bool?(false), null, null, null, null, null),
				Bid8 = c.Double(new bool?(false), null, null, null, null, null),
				Bid9 = c.Double(new bool?(false), null, null, null, null, null),
				Bid10 = c.Double(new bool?(false), null, null, null, null, null),
				BidQty1 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty2 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty3 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty4 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty5 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty6 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty7 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty8 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty9 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidQty10 = c.Int(new bool?(false), false, null, null, null, null, null),
				TotalBidQty = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount1 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount2 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount3 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount4 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount5 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount6 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount7 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount8 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount9 = c.Int(new bool?(false), false, null, null, null, null, null),
				BidCount10 = c.Int(new bool?(false), false, null, null, null, null, null),
				TotalBidCount = c.Int(new bool?(false), false, null, null, null, null, null),
				QuoteTime = c.String(null, null, null, null, null, null, null, null, null),
				ReceivedDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				ItemId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.QuoteId, null, true, null).ForeignKey("dbo.Items", t => t.ItemId, true, null, null).Index(t => t.ItemId, null, false, false, null);
			base.CreateTable("dbo.Orders", (ColumnBuilder c) => new
			{
				OrderId = c.Long(new bool?(false), true, null, null, null, null, null),
				RootOrderId = c.Long(new bool?(false), false, null, null, null, null, null),
				OrderNumber = c.Long(new bool?(false), false, null, null, null, null, null),
				OrderType = c.Int(new bool?(false), false, null, null, null, null, null),
				TradeType = c.Int(new bool?(false), false, null, null, null, null, null),
				PriceType = c.Int(new bool?(false), false, null, null, null, null, null),
				Qty = c.Int(new bool?(false), false, null, null, null, null, null),
				Price = c.Double(new bool?(false), null, null, null, null, null),
				ApplyLeverage = c.Int(new bool?(false), false, null, null, null, null, null),
				Profit = c.Long(new bool?(false), false, null, null, null, null, null),
				Commission = c.Long(new bool?(false), false, null, null, null, null, null),
				ProcessDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				OrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				OrderRouteType = c.Int(new bool?(false), false, null, null, null, null, null),
				NotConclusionQty = c.Int(new bool?(false), false, null, null, null, null, null),
				UnliquidationQty = c.Int(new bool?(false), false, null, null, null, null, null),
				ProcessCount = c.Int(new bool?(false), false, null, null, null, null, null),
				IsSatisfaction = c.Boolean(new bool?(false), null, null, null, null, null),
				IsNotCanceled = c.Boolean(new bool?(false), null, null, null, null, null),
				IsNotCompleted = c.Boolean(new bool?(false), null, null, null, null, null),
				Symbol = c.String(null, null, null, null, null, null, null, null, null),
				MarketId = c.Long(new bool?(false), false, null, null, null, null, null),
				UserAccountId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.OrderId, null, true, null).ForeignKey("dbo.Markets", t => t.MarketId, true, null, null).ForeignKey("dbo.UserAccounts", t => t.UserAccountId, true, null, null).Index(t => t.RootOrderId, null, false, false, null).Index(t => t.OrderNumber, null, false, false, null).Index(t => t.NotConclusionQty, null, false, false, null).Index(t => t.UnliquidationQty, null, false, false, null).Index(t => t.MarketId, null, false, false, null).Index(t => t.UserAccountId, null, false, false, null);
			base.CreateTable("dbo.StopLosses", (ColumnBuilder c) => new
			{
				StopLossId = c.Long(new bool?(false), true, null, null, null, null, null),
				SpeedOrderViewGuid = c.Guid(new bool?(false), false, null, null, null, null, null),
				StopLossType = c.Int(new bool?(false), false, null, null, null, null, null),
				IsLoss = c.Boolean(new bool?(false), null, null, null, null, null),
				LossTick = c.Int(new bool?(false), false, null, null, null, null, null),
				IsProfit = c.Boolean(new bool?(false), null, null, null, null, null),
				ProfitTick = c.Int(new bool?(false), false, null, null, null, null, null),
				CurrentPrice = c.Double(new bool?(false), null, null, null, null, null),
				PositionTradeType = c.Int(null, false, null, null, null, null, null),
				PositionAveragePrice = c.Double(new bool?(false), null, null, null, null, null),
				ProcessDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				Symbol = c.String(null, null, null, null, null, null, null, null, null),
				MarketId = c.Long(new bool?(false), false, null, null, null, null, null),
				UserAccountId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.StopLossId, null, true, null).ForeignKey("dbo.Markets", t => t.MarketId, true, null, null).ForeignKey("dbo.UserAccounts", t => t.UserAccountId, true, null, null).Index(t => t.MarketId, null, false, false, null).Index(t => t.UserAccountId, null, false, false, null);
			base.CreateTable("dbo.Overnights", (ColumnBuilder c) => new
			{
				OvernightId = c.Long(new bool?(false), true, null, null, null, null, null),
				OvernightType = c.Int(new bool?(false), false, null, null, null, null, null),
				IsFuturesOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsCmeFuturesOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsOptionOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsEurexOptionOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsForeignOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsKospiOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				IsKosdaqOvernight = c.Boolean(new bool?(false), null, null, null, null, null),
				NeedBalance = c.Long(new bool?(false), false, null, null, null, null, null),
				Balance = c.Long(new bool?(false), false, null, null, null, null, null),
				OvernightRouteType = c.Int(new bool?(false), false, null, null, null, null, null),
				ProcessDate = c.DateTime(new bool?(false), null, null, null, null, null, null),
				UserAccountId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.OvernightId, null, true, null).ForeignKey("dbo.UserAccounts", t => t.UserAccountId, true, null, null).Index(t => t.UserAccountId, null, false, false, null);
			base.CreateTable("dbo.UserAccountSpecifics", (ColumnBuilder c) => new
			{
				UserAccountSpecificId = c.Long(new bool?(false), true, null, null, null, null, null),
				SymbolCode = c.String(null, null, null, null, null, null, null, null, null),
				ItemCommission = c.Double(new bool?(false), null, null, null, null, null),
				ItemOrderSignalType = c.Int(new bool?(false), false, null, null, null, null, null),
				UserAccountId = c.Long(new bool?(false), false, null, null, null, null, null)
			}, null).PrimaryKey(t => t.UserAccountSpecificId, null, true, null).ForeignKey("dbo.UserAccounts", t => t.UserAccountId, true, null, null).Index(t => t.UserAccountId, null, false, false, null);
			base.CreateTable("dbo.Currencies", (ColumnBuilder c) => new
			{
				CurrencyId = c.Int(new bool?(false), false, null, null, null, null, null),
				Exchange = c.Double(new bool?(false), null, null, null, null, null),
				IsAutoUpdateExchange = c.Boolean(new bool?(false), null, null, null, null, null)
			}, null).PrimaryKey(t => t.CurrencyId, null, true, null);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00015034 File Offset: 0x00013234
		public override void Down()
		{
			base.DropForeignKey("dbo.UserAccountSpecifics", "UserAccountId", "dbo.UserAccounts");
			base.DropForeignKey("dbo.UserAccounts", "UserId", "dbo.Users");
			base.DropForeignKey("dbo.Overnights", "UserAccountId", "dbo.UserAccounts");
			base.DropForeignKey("dbo.MitOrders", "UserAccountId", "dbo.UserAccounts");
			base.DropForeignKey("dbo.StopLosses", "UserAccountId", "dbo.UserAccounts");
			base.DropForeignKey("dbo.StopLosses", "MarketId", "dbo.Markets");
			base.DropForeignKey("dbo.Orders", "UserAccountId", "dbo.UserAccounts");
			base.DropForeignKey("dbo.Orders", "MarketId", "dbo.Markets");
			base.DropForeignKey("dbo.MitOrders", "MarketId", "dbo.Markets");
			base.DropForeignKey("dbo.Quotes", "ItemId", "dbo.Items");
			base.DropForeignKey("dbo.Markets", "ItemId", "dbo.Items");
			base.DropForeignKey("dbo.Currents", "ItemId", "dbo.Items");
			base.DropForeignKey("dbo.DepositWithdraws", "UserAccountId", "dbo.UserAccounts");
			base.DropForeignKey("dbo.DayProfitLosses", "UserAccountId", "dbo.UserAccounts");
			base.DropForeignKey("dbo.Connections", "UserId", "dbo.Users");
			base.DropForeignKey("dbo.Users", "CompanyId", "dbo.Companies");
			base.DropForeignKey("dbo.ParentAccounts", "CompanyId", "dbo.Companies");
			base.DropForeignKey("dbo.Notices", "CompanyId", "dbo.Companies");
			base.DropForeignKey("dbo.CompanyAccounts", "CompanyId", "dbo.Companies");
			base.DropForeignKey("dbo.Certifications", "CompanyId", "dbo.Companies");
			base.DropIndex("dbo.UserAccountSpecifics", new string[]
			{
				"UserAccountId"
			}, null);
			base.DropIndex("dbo.Overnights", new string[]
			{
				"UserAccountId"
			}, null);
			base.DropIndex("dbo.StopLosses", new string[]
			{
				"UserAccountId"
			}, null);
			base.DropIndex("dbo.StopLosses", new string[]
			{
				"MarketId"
			}, null);
			base.DropIndex("dbo.Orders", new string[]
			{
				"UserAccountId"
			}, null);
			base.DropIndex("dbo.Orders", new string[]
			{
				"MarketId"
			}, null);
			base.DropIndex("dbo.Orders", new string[]
			{
				"UnliquidationQty"
			}, null);
			base.DropIndex("dbo.Orders", new string[]
			{
				"NotConclusionQty"
			}, null);
			base.DropIndex("dbo.Orders", new string[]
			{
				"OrderNumber"
			}, null);
			base.DropIndex("dbo.Orders", new string[]
			{
				"RootOrderId"
			}, null);
			base.DropIndex("dbo.Quotes", new string[]
			{
				"ItemId"
			}, null);
			base.DropIndex("dbo.Currents", new string[]
			{
				"ItemId"
			}, null);
			base.DropIndex("dbo.Markets", new string[]
			{
				"ItemId"
			}, null);
			base.DropIndex("dbo.Markets", new string[]
			{
				"MarketDate"
			}, null);
			base.DropIndex("dbo.MitOrders", new string[]
			{
				"UserAccountId"
			}, null);
			base.DropIndex("dbo.MitOrders", new string[]
			{
				"MarketId"
			}, null);
			base.DropIndex("dbo.MitOrders", new string[]
			{
				"NotExecuteQty"
			}, null);
			base.DropIndex("dbo.MitOrders", new string[]
			{
				"RootMitOrderId"
			}, null);
			base.DropIndex("dbo.DepositWithdraws", new string[]
			{
				"UserAccountId"
			}, null);
			base.DropIndex("dbo.DayProfitLosses", new string[]
			{
				"UserAccountId"
			}, null);
			base.DropIndex("dbo.UserAccounts", new string[]
			{
				"UserId"
			}, null);
			base.DropIndex("dbo.Connections", new string[]
			{
				"UserId"
			}, null);
			base.DropIndex("dbo.Users", new string[]
			{
				"CompanyId"
			}, null);
			base.DropIndex("dbo.ParentAccounts", new string[]
			{
				"CompanyId"
			}, null);
			base.DropIndex("dbo.Notices", new string[]
			{
				"CompanyId"
			}, null);
			base.DropIndex("dbo.CompanyAccounts", new string[]
			{
				"CompanyId"
			}, null);
			base.DropIndex("dbo.Certifications", new string[]
			{
				"CompanyId"
			}, null);
			base.DropTable("dbo.Currencies");
			base.DropTable("dbo.UserAccountSpecifics");
			base.DropTable("dbo.Overnights");
			base.DropTable("dbo.StopLosses");
			base.DropTable("dbo.Orders");
			base.DropTable("dbo.Quotes");
			base.DropTable("dbo.Currents");
			base.DropTable("dbo.Items");
			base.DropTable("dbo.Markets");
			base.DropTable("dbo.MitOrders");
			base.DropTable("dbo.DepositWithdraws");
			base.DropTable("dbo.DayProfitLosses");
			base.DropTable("dbo.UserAccounts");
			base.DropTable("dbo.Connections");
			base.DropTable("dbo.Users");
			base.DropTable("dbo.ParentAccounts");
			base.DropTable("dbo.Notices");
			base.DropTable("dbo.CompanyAccounts");
			base.DropTable("dbo.Certifications");
			base.DropTable("dbo.Companies");
			base.DropTable("dbo.Admins");
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x000155B3 File Offset: 0x000137B3
		string IMigrationMetadata.Id
		{
			get
			{
				return "201609010729538_InitialCreate";
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x000155BA File Offset: 0x000137BA
		string IMigrationMetadata.Source
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000155BD File Offset: 0x000137BD
		string IMigrationMetadata.Target
		{
			get
			{
				return this.Resources.GetString("Target");
			}
		}

		// Token: 0x040001D8 RID: 472
		private readonly ResourceManager Resources = new ResourceManager(typeof(InitialCreate));
	}
}

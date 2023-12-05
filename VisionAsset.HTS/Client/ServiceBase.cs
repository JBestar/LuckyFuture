using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace VisionAsset.Client
{
	public abstract class ServiceBase<TChannel> where TChannel : class
	{
		protected ChannelFactory<TChannel> _channelFactory;
		protected TChannel _channel;
		public ServiceBase()
		{

		}
		public ServiceBase(Binding binding, string uri)
		{
			Create(binding, new EndpointAddress(new Uri(uri)));
		}
		public ServiceBase(Binding binding, EndpointAddress address)
		{
			Create(binding, address);
		}

		public void Create(Binding binding, string uri)
		{
			Create(binding, new EndpointAddress(new Uri(uri)));
		}

		public void Create(Binding binding, EndpointAddress address)
		{
			var contract = ContractDescription.GetContract(typeof(TChannel));
			_channelFactory = new ChannelFactory<TChannel>(new ServiceEndpoint(contract, binding, address));
			_channel = _channelFactory.CreateChannel();
		}
		
		protected TChannel Channel
		{
			get
			{
				if (_channel == null)
					throw new Exception("Channel not configured");
				return _channel;
			}
		}
		public CommunicationState State
		{
			get
			{
				IChannel channel = (IChannel)((object)this._channel);
				if (channel != null)
					return channel.State;
				return CommunicationState.Created;
			}
		}

		public void Close()
		{
			try
			{
				if (_channel != null)
					(_channel as IDisposable).Dispose();
			}
			catch
			{
				
			}
			Abort();
		}

		public void Abort()
		{
			IChannel channel = (IChannel)((object)this._channel);
			if (channel != null)
				channel.Abort();

			if (_channelFactory != null)
			{
				_channelFactory.Abort();
				_channelFactory = null;
			}
		}
	}
}

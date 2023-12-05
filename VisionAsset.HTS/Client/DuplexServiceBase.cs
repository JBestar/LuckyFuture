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
	public abstract class DuplexServiceBase<TChannel>: ServiceBase<TChannel> where TChannel : class
	{
		public DuplexServiceBase(InstanceContext callbackInstance)
		{

		}
		public DuplexServiceBase(InstanceContext callbackInstance, Binding binding, string uri)
		{
			Create(callbackInstance, binding, new EndpointAddress(new Uri(uri)));
		}
		public DuplexServiceBase(InstanceContext callbackInstance, Binding binding, EndpointAddress address)
		{
			Create(callbackInstance, binding, address);
		}

		public void Create(InstanceContext callbackInstance, Binding binding, string uri)
		{
			Create(callbackInstance, binding, new EndpointAddress(new Uri(uri)));
		}

		public void Create(InstanceContext callbackInstance, Binding binding, EndpointAddress address)
		{
			var contract = ContractDescription.GetContract(typeof(TChannel));
			_channelFactory = new DuplexChannelFactory<TChannel>(callbackInstance, new ServiceEndpoint(contract, binding, address));
			_channel = _channelFactory.CreateChannel();
		}
	}
}

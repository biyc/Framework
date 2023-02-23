using System.ComponentModel;
using Newtonsoft.Json;

// using LitJson;

namespace ETModel
{
	public abstract class Object: ISupportInitialize
	{
		public virtual void BeginInit()
		{
		}

		public virtual void EndInit()
		{
		}

		public override string ToString()
		{
			// return JsonMapper.ToJson(this);
			return JsonConvert.SerializeObject(this);
		}
	}
}
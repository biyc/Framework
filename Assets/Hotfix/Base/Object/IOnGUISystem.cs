using System;

namespace ETHotfix
{
	public interface IOnGUISystem
    {
		Type Type();
		void Run(object o);
	}

	public abstract class OnGUISystem<T> : IOnGUISystem
    {
		public void Run(object o)
		{
			this.OnGUI((T)o);
		}

		public Type Type()
		{
			return typeof(T);
		}

		public abstract void OnGUI(T self);
	}
}

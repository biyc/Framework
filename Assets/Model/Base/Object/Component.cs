using System;


namespace ETModel
{
	
	public abstract class Component : Object, IDisposable, IComponentSerialize
	{
		
		public long InstanceId { get; protected set; }

		
		private bool isFromPool;

		
		public bool IsFromPool
		{
			get
			{
				return this.isFromPool;
			}
			set
			{
				this.isFromPool = value;

				if (!this.isFromPool)
				{
					return;
				}

				if (this.InstanceId == 0)
				{
					this.InstanceId = IdGenerater.GenerateId();
				}

				Game.EventSystem.Add(this);
			}
		}

		
		public bool IsDisposed
		{
			get
			{
				return this.InstanceId == 0;
			}
		}
		
		
		public Component Parent { get; set; }

		public T GetParent<T>() where T : Component
		{
			return this.Parent as T;
		}

		
		public Entity Entity
		{
			get
			{
				return this.Parent as Entity;
			}
		}
		
		protected Component()
		{
			this.InstanceId = IdGenerater.GenerateId();
		}

		public virtual void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			// 触发Destroy事件
			Game.EventSystem.Destroy(this);

			Game.EventSystem.Remove(this.InstanceId);

			this.InstanceId = 0;

			if (this.IsFromPool)
			{
				Game.ObjectPool.Recycle(this);
			}
		}

		public virtual void BeginSerialize()
		{
		}

		public virtual void EndDeSerialize()
		{
		}
	}
}
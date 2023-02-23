using ETModel;


namespace ETHotfix
{

	/// <summary>
	/// 基础对象，独立存在或附加在其它对象下
	/// </summary>
	
	public abstract class Component : Object, IDisposable, IComponentSerialize
	{
		
		public long InstanceId { get; protected set; }

		
		private bool _isFromPool;

		
		public bool IsFromPool
		{
			get
			{
				return this._isFromPool;
			}
			set
			{
				this._isFromPool = value;

				if (!this._isFromPool)
				{
					return;
				}

				if (this.InstanceId == 0)
				{
					// 池中对象进行标号
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
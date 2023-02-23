using System;
using System.Collections.Generic;
using System.Linq;


namespace ETHotfix
{

	/// <summary>
	/// Entity 中支持添加多个 Component 组件
	/// Component 是最小元素，不支持添加其它 Component 组件
	/// </summary>
	public class Entity : Component
	{
		private readonly HashSet<Component> _components;

		
		private readonly Dictionary<Type, Component> _componentDict;

		protected Entity()
		{
			this._components = new HashSet<Component>();
			this._componentDict = new Dictionary<Type, Component>();
		}


		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			foreach (Component component in this.GetComponents())
			{
				try
				{
					component.Dispose();
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
			this._components.Clear();
			this._componentDict.Clear();

			base.Dispose();

		}
		
		public Component AddComponent(Component component)
		{
			Type type = component.GetType();
			if (this._componentDict.ContainsKey(type))
			{
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {type.Name}");
                return GetComponent(type);
            }
			
			component.Parent = this;

			if (component is ISerializeToEntity)
			{
				this._components.Add(component);
			}
			this._componentDict.Add(type, component);
			return component;
		}

		public Component AddComponent(Type type)
		{
			if (this._componentDict.ContainsKey(type))
			{
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {type.Name}");
                return GetComponent(type);
            }

			Component component = ComponentFactory.CreateWithParent(type, this);

			if (component is ISerializeToEntity)
			{
				this._components.Add(component);
			}
			this._componentDict.Add(type, component);
			return component;
		}

		public K AddComponent<K>() where K : Component, new()
		{
			Type type = typeof (K);
            K component = null;

            if (this._componentDict.ContainsKey(type))
			{
                component = GetComponent<K>();
                return component;
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
            }

			component = ComponentFactory.CreateWithParent<K>(this);

			if (component is ISerializeToEntity)
			{
				this._components.Add(component);
			}
			this._componentDict.Add(type, component);
			return component;
		}

		public K AddComponent<K, P1>(P1 p1) where K : Component, new()
		{
			Type type = typeof (K);
			if (this._componentDict.ContainsKey(type))
			{
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
                return GetComponent<K>();
            }

			K component = ComponentFactory.CreateWithParent<K, P1>(this, p1);
			
			if (component is ISerializeToEntity)
			{
				this._components.Add(component);
			}
			this._componentDict.Add(type, component);
			return component;
		}

		public K AddComponent<K, P1, P2>(P1 p1, P2 p2) where K : Component, new()
		{
			Type type = typeof (K);
			if (this._componentDict.ContainsKey(type))
			{
				//throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
                return GetComponent<K>();
            }

			K component = ComponentFactory.CreateWithParent<K, P1, P2>(this, p1, p2);
			
			if (component is ISerializeToEntity)
			{
				this._components.Add(component);
			}
			this._componentDict.Add(type, component);
			return component;
		}

		public K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where K : Component, new()
		{
			Type type = typeof (K);
			if (this._componentDict.ContainsKey(type))
			{
                //throw new Exception($"AddComponent, component already exist, id: {this.Id}, component: {typeof(K).Name}");
                return GetComponent<K>();
            }

			K component = ComponentFactory.CreateWithParent<K, P1, P2, P3>(this, p1, p2, p3);
			
			if (component is ISerializeToEntity)
			{
				this._components.Add(component);
			}
			this._componentDict.Add(type, component);
			return component;
		}

		public void RemoveComponent<K>() where K : Component
		{
			Type type = typeof (K);
			Component component;
			if (!this._componentDict.TryGetValue(type, out component))
			{
				return;
			}

			this._components.Remove(component);
			this._componentDict.Remove(type);

			component.Dispose();
		}

		public void RemoveComponent(Type type)
		{
			Component component;
			if (!this._componentDict.TryGetValue(type, out component))
			{
				return;
			}

			this._components?.Remove(component);
			this._componentDict.Remove(type);

			component.Dispose();
		}

		public K GetComponent<K>() where K : Component
		{
			Component component;
			if (!this._componentDict.TryGetValue(typeof(K), out component))
			{
				return default(K);
			}
			return (K)component;
		}

		public Component GetComponent(Type type)
		{
			Component component;
			if (!this._componentDict.TryGetValue(type, out component))
			{
				return null;
			}
			return component;
		}

		public Component[] GetComponents()
		{
			return this._componentDict.Values.ToArray();
		}

		public override void EndInit()
		{
			try
			{
				base.EndInit();
				
				this._componentDict.Clear();

				if (this._components != null)
				{
					foreach (Component component in this._components)
					{
						component.Parent = this;
						this._componentDict.Add(component.GetType(), component);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public override void BeginSerialize()
		{
			base.BeginSerialize();

			foreach (Component component in this._components)
			{
				component.BeginSerialize();
			}
		}

		public override void EndDeSerialize()
		{
			base.EndDeSerialize();
			
			foreach (Component component in this._components)
			{
				component.EndDeSerialize();
			}
		}
	}
}
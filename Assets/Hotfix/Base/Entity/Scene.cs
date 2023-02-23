namespace ETHotfix
{
	public sealed class Scene: Entity
	{

		/// <summary>
		/// 母包中的上下文环境
		/// </summary>
		public ETModel.Scene ModelScene { get; set; } = new ETModel.Scene();

		public string Name { get; set; }



		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();

			this.ModelScene.Dispose();
		} 
    }
}
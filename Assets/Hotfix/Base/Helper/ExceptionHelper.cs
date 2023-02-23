using System;
using System.Threading.Tasks;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
	public static class ExceptionHelper
	{


		/// <summary>
		/// 对Task 进行扩展，可以通过回调直接获取异步数据
		/// </summary>
		/// <param name="task"></param>
		/// <param name="loadCb"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Task<T> OnLoad<T>(this Task<T> task,Action<T> loadCb)
		{
			// Log.PrintThread();
			if (task.GetAwaiter().IsCompleted)
			{
				loadCb?.Invoke(task.Result);
				return task;
			}

			task.GetAwaiter().OnCompleted(delegate
			{
				loadCb?.Invoke(task.Result);
			});
			// task.GetAwaiter().UnsafeOnCompleted(delegate
			// {
			//
			// });
			return task;
		}


		public static void SetTransformDefalut(this Transform trans)
		{
			trans.localPosition = Vector3.zero;
			trans.localEulerAngles = Vector3.zero;
			trans.localScale = Vector3.one;
		}
		public static void DestroyAllChild(this Transform trans)
		{
			if (trans.childCount < 0) return;
			int count = trans.childCount;
			while (count > 0)
			{
				UnityEngine.Object.DestroyImmediate(trans.GetChild(0).gameObject);
				count--;
			}
		}


		public static string ToStr(this Exception exception)
		{
#if ILRuntime
			return $"{exception.Data["StackTrace"]} \n\n {exception}";
#else
			return exception.ToString();
#endif
		}
	}
}
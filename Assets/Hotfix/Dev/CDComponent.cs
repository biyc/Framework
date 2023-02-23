using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ETModel.ObjectSystem]
    public class CDComponentAwakeSystem : AwakeSystem<CDComponent>
    {
        public override void Awake(CDComponent self)
        {
            self.Awake();
        }
    }

    [ETModel.ObjectSystem]
    public class CDComponentUpdateSystem : UpdateSystem<CDComponent>
    {
        public override void Update(CDComponent self)
        {
            self.Update();
        }
    }

    public class CDComponent : Component
    {
        public class CDData
        {
            public long cd;
            public long elapse;
            public bool reverse;//true:倒计时(10 9 8 ...) false:正计时(1 2 3 ...)
            public Action<long> actionProcess;
            public Action<float> actionProcessPercent;//百分比
            public Action actionFinish;
            public Action actionCancel;
            public Action actionComplete;
            public TaskCompletionSource<bool> tcs;
            public CancellationToken cancellationToken;
        }

        private List<CDData> list;
        private int id;

        public int Id
        {
            get
            {
                return id;
            }
            private set
            {
                id = value;
            }
        }

        public void Awake()
        {
            this.list = new List<CDData>();
            pauseUpdate = false;
        }

        public void Update()
        {
            if (pauseUpdate)
            {
                return;
            }
            Process();
        }


        private void Process()
        {
            if (this.list.Count == 0)
            {
                return;
            }
            int index = 0;
            while (index >= 0 && this.list.Count > index)
            {
                var data = this.list[index];
                if (data.elapse > data.cd)
                {
                    //if (!data.tcs.Task.IsCompleted)
                    //{
                    //    data.tcs.SetResult(true);
                    //}
                    //Finish(data);
                    Complete(data);
                    index--;
                    continue;
                }
                data.elapse += (long)(UnityEngine.Time.deltaTime * 1000);
                if (data.reverse)
                {
                    data.actionProcess?.Invoke(data.cd - data.elapse);
                    data.actionProcessPercent?.Invoke(1 - data.elapse / (float)data.cd);
                }
                else
                {
                    data.actionProcess?.Invoke(data.elapse);
                    data.actionProcessPercent?.Invoke(data.elapse / (float)data.cd);
                }
                index++;
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            this.list?.Clear();
            this.list = null;
            Id = 0;
            pauseUpdate = false;
            backstageElapseTime = 0;
            base.Dispose();
        }

        public void Start(CDData data)
        {
            Id++;
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            data.cancellationToken.Register(() =>
            {
                data.actionFinish?.Invoke();
                list.Remove(data);
            });
            data.tcs = tcs;
            this.list.Add(data);
        }

        public void Cancel(CDData data)
        {
            if (!data.tcs.Task.IsCompleted)
            {
                data.tcs.SetCanceled();
                data.actionCancel?.Invoke();
                Finish(data);
            }
        }

        public void Complete(CDData data)
        {
            if (!data.tcs.Task.IsCompleted)
            {
                data.tcs.SetResult(true);
                data.actionComplete?.Invoke();
                Finish(data);
            }
            else
            {
                Finish(data);
            }
        }

        public void Finish(CDData data)
        {
            data.actionFinish?.Invoke();
            list.Remove(data);
        }

        private float backstageElapseTime;
        private bool pauseUpdate;
        public void BackstageIn()
        {
            UnityEngine.Debug.Log("进入后台");
            backstageElapseTime = UnityEngine.Time.realtimeSinceStartup;
        }

        public void BackstageOut()
        {
            UnityEngine.Debug.Log("退出后台");
            backstageElapseTime = UnityEngine.Time.realtimeSinceStartup - backstageElapseTime;
            UnityEngine.Debug.Log($"后台持续时长：{backstageElapseTime}");
            if (list != null && list.Count > 0)
            {
                pauseUpdate = true;
                foreach (var item in list)
                {
                    item.elapse += (long)(backstageElapseTime * 1000);
                }
                Process();
                pauseUpdate = false;
            }
        }
    }
}
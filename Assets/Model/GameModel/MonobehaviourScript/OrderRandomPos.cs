using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Blaze.Resource.Task;
using UniRx;
using UnityEngine;


/// <summary>
/// 网格对象
/// </summary>
public class GridItem
{
    /// <summary>
    /// 网格
    /// </summary>
    //   public GridItem[,] GridItems;
    public List<List<GridItem>> GridItems;
    //public Dictionary<string, GridItem> GridItems;

    /// <summary>
    /// 行
    /// </summary>
    public int Row;

    /// <summary>
    /// 列
    /// </summary>
    public int Column;

    /// <summary>
    /// 每个网格的中心点
    /// </summary>
    public Vector2 Center;

    /// <summary>
    /// 权重
    /// </summary>
    public int Weight;

    /// <summary>
    /// 获取该网格周围的网格数据
    /// </summary>
    /// <param name="rowNum"></param>
    /// <param name="columnNum"></param>
    /// <returns></returns>
    public GridAreaData GetAreaWights(int rowNum, int columnNum)
    {
        GridAreaData tempItem = new GridAreaData();
        tempItem.GridItem = this;
        for (int i = Row - rowNum; i < Row + rowNum; i++)
        {
            for (int j = Column - columnNum; j < Column + columnNum; j++)
            {
                try
                {
                    //  var item = GridItems[i, j];
                    var item = GridItems[i][j];
                    // var item = GridItems.Find(x => x.Row == i && x.Column == j);
                    //   var item = GridItems[i + "|" + j];
                    tempItem.Weights += item.Weight;
                    tempItem.Areas.Add(item);
                }
                catch (Exception e)
                {
                }
            }
        }

        return tempItem;
    }
}

public class ImgData
{
    /// <summary>
    /// 图片对象
    /// </summary>
    public Transform Img;

    /// <summary>
    /// 该对象的占的行数的一半
    /// </summary>
    public int HalfRow;

    /// <summary>
    /// 该对象占的列数的一半
    /// </summary>
    public int HalfColumn;
}

/// <summary>
/// 网格周围的数据信息
/// </summary>
public class GridAreaData
{
    /// <summary>
    /// 网格对象
    /// </summary>
    public GridItem GridItem;

    /// <summary>
    /// 网格周围的权重
    /// </summary>
    public int Weights;

    /// <summary>
    /// 网格周围的网格数据
    /// </summary>
    public List<GridItem> Areas = new List<GridItem>();
}

public class OrderRandomPos : MonoBehaviour
{
    /// <summary>
    ///显示对象的父物体
    /// </summary>
    private Transform _notesParent;

    /// <summary>
    /// 父物体的宽度
    /// </summary>
    private float _pWidth;

    /// <summary>
    /// 父物体的高度
    /// </summary>
    private float _pHeight;

    /// <summary>
    /// 网格的行数
    /// </summary>
    private int _gridRow = 3;

    /// <summary>
    /// 网格的列数
    /// </summary>
    private int _gridColumn = 4;

    /// <summary>
    /// 网格
    /// </summary>
    //   private GridItem[,] _gridItems;
    //private List<GridItem> _gridItems;
    // private Dictionary<string, GridItem> _gridItems = new Dictionary<string, GridItem>();
    public List<List<GridItem>> _gridItems;

    /// <summary>
    /// 每个网格的长度
    /// </summary>
    public float GridSlitLength = 20;

    /// <summary>
    /// 图片数据
    /// </summary>
    private List<ImgData> _imgItems = new List<ImgData>();

    // public OrderRandomPos(Transform NotesParent)
    // {
    //     _notesParent = NotesParent;
    //     Awake();
    // }

    public void StartRandom()
    {
        _notesParent = this.transform;
        // Parent.GetRectTransform().pivot=Vector2.zero;
        // Parent.GetRectTransform().ForceUpdateRectTransforms();
        //父物体的长宽
        _pWidth = _notesParent.transform.GetComponent<RectTransform>().rect.width;
        _pHeight = _notesParent.transform.GetComponent<RectTransform>().rect.height;

        //网格的行列数计算
        _gridRow = Mathf.FloorToInt(_pHeight / GridSlitLength);
        _gridColumn = Mathf.FloorToInt(_pWidth / GridSlitLength);

        _gridItems = new List<List<GridItem>>();
        for (int i = 0; i < _gridRow; i++)
        {
            _gridItems.Add(new List<GridItem>(_gridColumn));
        }

        //   _gridItems = new GridItem[_gridRow, _gridColumn];
        //_gridItems = new List<GridItem>();

        //生成网格数据
        for (int i = 0; i < _gridRow; i++)
        {
            for (int j = 0; j < _gridColumn; j++)
            {
                var f = new GridItem();
                f.Center = new Vector2(j * GridSlitLength + GridSlitLength / 2,
                    i * GridSlitLength + GridSlitLength / 2);
                //f.GridItems = _gridItems;
                f.GridItems = _gridItems;
                f.Row = i;
                f.Column = j;
                if (_gridItems[i] == null)
                {
                    _gridItems[i] = new List<GridItem>();
                }

                // _gridItems[i][j] = f;
                _gridItems[i].Add(f);
                //_gridItems.Add(i + "|" + j, f);
                //  _gridItems[i, j] = f;
            }
        }

        //var ImgItems = _notesParent.GetComponentsInChildren<Transform>().ToList();
        var ImgItems = new List<Transform>();
        foreach (Transform o in _notesParent)
        {
            ImgItems.Add(o);
        }
        //ImgItems.RemoveAt(0);

        //转换成相对应的的数据
        foreach (var v in ImgItems)
        {
            var x = new ImgData()
            {
                HalfRow = Mathf.CeilToInt(v.GetComponent<RectTransform>().rect.height / 2 / GridSlitLength),
                HalfColumn = Mathf.CeilToInt(v.GetComponent<RectTransform>().rect.width / 2 / GridSlitLength),
                Img = v
            };
            _imgItems.Add(x);
        }

        //根据面积的大小进行排序
        _imgItems.Sort((x, y) => (x.HalfColumn * x.HalfRow).CompareTo((y.HalfRow * y.HalfColumn)));
        //将面积大的放在最底层，避免遮挡
        foreach (var v in _imgItems)
            v.Img.SetAsFirstSibling();


        // new Thread(delegate(object o)
        // {
        //     for (int i = _imgItems.Count - 1; i >= 0; i--)
        //     {
        //         var v = _imgItems[i];
        //
        //         var areasData = GetGridAreaDatas(v.HalfRow, v.HalfColumn);
        //         //将每一个网格区域信息进行排序
        //         areasData.Sort((x, y) => x.Weights.CompareTo(y.Weights));
        //         //找到权重最低的网格点
        //         var minItems = areasData.FindAll(x => x.Weights == areasData[0].Weights);
        //
        //
        //         MonoScheduler.DispatchMain(delegate
        //         {
        //             //从最低的网格点钟随机一个点
        //             GridAreaData minItem = minItems[UnityEngine.Random.Range(0, minItems.Count)];
        //             v.Img.transform.localPosition = minItem.GridItem.Center;
        //             minItem.Areas.ForEach(x => x.Weight++);
        //         });
        //     }
        // }).Start();
        for (int i = _imgItems.Count - 1; i >= 0; i--)
        {
            var v = _imgItems[i];

            var areasData = GetGridAreaDatas(v.HalfRow, v.HalfColumn);
            //将每一个网格区域信息进行排序
            areasData.Sort((x, y) => x.Weights.CompareTo(y.Weights));
            //找到权重最低的网格点
            var minItems = areasData.FindAll(x => x.Weights == areasData[0].Weights);

            //从最低的网格点钟随机一个点
            GridAreaData minItem = minItems[UnityEngine.Random.Range(0, minItems.Count)];
            v.Img.transform.localPosition = minItem.GridItem.Center;
            minItem.Areas.ForEach(x => x.Weight++);
        }
    }

    /// <summary>
    /// 获取每个网格一定范围内的网格数据
    /// </summary>
    /// <param name="rowNum"></param>
    /// <param name="columnNum"></param>
    /// <returns></returns>
    List<GridAreaData> GetGridAreaDatas(int rowNum, int columnNum)
    {
        List<GridAreaData> tempItems = new List<GridAreaData>();
        for (int i = rowNum; i < _gridRow - rowNum; i++)
        {
            for (int j = columnNum; j < _gridColumn - columnNum; j++)
            {
                // var item = _gridItems[i, j];
                //var item = _gridItems.Find(x => x.Row == i && x.Column == j);
                //var item = _gridItems[i + "|" + j];
                //Debug.LogError(item.Center);
                var item = _gridItems[i][j];
                var tempItem = item.GetAreaWights(rowNum, columnNum);
                tempItems.Add(tempItem);
            }
        }

        return tempItems;
    }
}
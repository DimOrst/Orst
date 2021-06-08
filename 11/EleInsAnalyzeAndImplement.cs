using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 元素指令的接收、分析与执行
/// </summary>
public class EleInsAnalyzeAndImplement : MonoBehaviour
{
    /// <summary>
    /// 用以规划路径的三个控制点
    /// </summary>
    public GameObject SpellCaster;
    public Vector3 TargetPos;
    public Vector3 MidPos;

    /// <summary>
    /// 两点距离过近则不重置路径，此为阈值
    /// </summary>
    public float RouteResetDis;

    /// <summary>
    /// 魔力消耗
    /// </summary>
    public float ManaCost;

    /// <summary>
    /// 被操作的元素们
    /// </summary>
    public List<GameObject> ElementsUnderControl;

    /// <summary>
    /// 要执行的指令序列
    /// </summary>
    public Queue<int> MagicInstructions = new Queue<int>();

    // Start is called before the first frame update
    void Start()
    {
        TargetPos = SpellCaster.transform.position;
        MidPos = SpellCaster.transform.position;

        //下面是测试的指令序列
        MagicInstructions.Enqueue(0);
        MagicInstructions.Enqueue(1);
        MagicInstructions.Enqueue(8);
        MagicInstructions.Enqueue(9);
    }

    // Update is called once per frame
    void Update()
    {
        InstructionImplementation();
    }

    /// <summary>
    /// 指令在此处被分析，将输入参数转传入将执行的方法并执行方法
    /// </summary>
    public void InstructionImplementation()
    {
        if (MagicInstructions.Count != 0)
        {
            var insNum = MagicInstructions.Peek();

            switch (insNum)
            {
                case 0:
                    {
                        ElementGeneration();
                        break;
                    }
                case 1:
                    {
                        ElementEnable();
                        break;
                    }
                case 2:
                    {
                        ElementCompressing();
                        break;
                    }
                case 3:
                    {
                        ElementLifeMaintaining();
                        break;
                    }
                case 4:
                    {
                        TakeControlOfElement();
                        break;
                    }
                case 5:
                    {
                        ElementForming();
                        break;
                    }
                case 6:
                    {
                        ElementClipping();
                        break;
                    }
                case 7:
                    {
                        ElementCombining();
                        break;
                    }
                case 8:
                    {
                        RouteSetting();
                        break;
                    }
                case 9:
                    {
                        MagicEnding();
                        break;
                    }
                default: { break; }
            }
        }
    }
    
    /// <summary>
    /// 元素生成
    /// </summary>
    public void ElementGeneration()
    {
        Debug.Log("元素生成");
        MagicInstructions.Dequeue();
    }

    /// <summary>
    /// 元素激活
    /// </summary>
    public void ElementEnable()
    {
        Debug.Log("元素激活");
        MagicInstructions.Dequeue();
    }

    /// <summary>
    /// 元素压缩
    /// </summary>
    public void ElementCompressing()
    {
        MagicInstructions.Dequeue();
    }

    /// <summary>
    /// 元素寿命维持
    /// </summary>
    public void ElementLifeMaintaining()
    {
        MagicInstructions.Dequeue();
    }

    /// <summary>
    /// 元素支配
    /// </summary>
    public void TakeControlOfElement()
    {
        MagicInstructions.Dequeue();
    }

    /// <summary>
    /// 元素成型
    /// </summary>
    public void ElementForming()
    {
        MagicInstructions.Dequeue();
    }

    /// <summary>
    /// 元素分割
    /// </summary>
    public void ElementClipping()
    {
        MagicInstructions.Dequeue();
    }

    /// <summary>
    /// 元素合并
    /// </summary>
    public void ElementCombining()
    {
        MagicInstructions.Dequeue();
    }

    /// <summary>
    ///  路径设置函数，魔力消耗计算未设置，魔法产物沿路径运动的代码没写
    /// </summary>
    public void RouteSetting()
    {
        RaycastHit hit;
        LineRenderer LR = gameObject.GetComponent<LineRenderer>();
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out hit);
        if (Input.GetMouseButtonDown(0))
        {
            TargetPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);// + new Vector3(0f, 1f, 0f);
            MidPos = TargetPos;
        }
        //var TargetDis = MidPos - TargetPos;
        if (Input.GetMouseButton(0))  // && TargetDis.magnitude >= RouteResetDis)
        {
            //MidPos = new Vector3(hit.point.x, SpellCaster.transform.position.y, hit.point.z);
            MidPos = new Vector3(hit.point.x, hit.point.y, hit.point.z) + new Vector3(0, Mathf.Abs(TargetPos.y - SpellCaster.transform.position.y) / 2, 0);

            if ((MidPos - TargetPos).magnitude >= RouteResetDis)
            {
                Vector3[] Points = { SpellCaster.transform.position, MidPos, TargetPos };
                Vector3 Mid2Mid = ((Points[0] + Points[1]) / 2 - (Points[2] + Points[1]) / 2) * 0.6f;
                Vector3 CtrlP1 = Points[1] + Mid2Mid;
                Vector3 CtrlP2 = Points[1] - Mid2Mid;
                Vector3[] RouteDots = new Vector3[100];
                for (int i = 0; i < 50; i++)
                {
                    //一
                    Vector3 pos1 = Vector3.Lerp(Points[0], CtrlP1, i / 50f);
                    Vector3 pos2 = Vector3.Lerp(CtrlP1, Points[1], i / 50f);

                    Vector3 pos3 = Vector3.Lerp(Points[1], CtrlP2, i / 50f);
                    Vector3 pos4 = Vector3.Lerp(CtrlP2, Points[2], i / 50f);
                    //二
                    var pos1_0 = Vector3.Lerp(pos1, pos2, i / 50f);

                    var pos1_1 = Vector3.Lerp(pos3, pos4, i / 50f);

                    RouteDots[i] = pos1_0;
                    RouteDots[i + 50] = pos1_1;
                }
                LR.positionCount = RouteDots.Length;
                LR.SetPositions(RouteDots);
            }
            else
            {
                LR.positionCount = 2;
                LR.SetPosition(0, SpellCaster.transform.position);
                LR.SetPosition(1, TargetPos);
                //LineRenderer LR1 = new LineRenderer();
                //LR1.material = LR.material;
                //LR1.startWidth = LR.startWidth;
                //LR1.endWidth = LR.endWidth;
                //Vector3[] circlePoints = new Vector3[25];
                //for (int i = 0; i < 25; i++)
                //{
                //    circlePoints[i] = TargetPos + new Vector3(RouteResetDis * Mathf.Cos(i * 2 * Mathf.PI / 25), 0, RouteResetDis * Mathf.Sin(i * 2 * Mathf.PI / 25));
                //}
                //LR1.positionCount = 25;
                //LR1.SetPositions(circlePoints);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //此时将魔法释放
            LR.positionCount = 0;
            //return true;
            MagicInstructions.Dequeue();
        }
    }

    /// <summary>
    /// 魔法结束时结算之类的工作
    /// </summary>
    public void MagicEnding()
    {
        MagicInstructions.Dequeue();
        Debug.Log("Magic Ended");
    }

}
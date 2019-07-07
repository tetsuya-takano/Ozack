using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ozack;

public class Data
{
    public Data(OzackComandArgs args)
    {
        Key = args.Cmd;
        Values = args.Params;
    }

    public string Key { get; internal set; }
    public IReadOnlyList<string> Values { get; internal set; }
}

public sealed class SetTextBehaviour : OzackBehaviour<Data>
{
    private const string Cmd = "SetText";

    //======================================
    // factory
    //======================================
    private sealed class BehaviourFactory : OzackBehaviourFactory<Data>
    {
        public override bool CanCreate(Data cmd)
        {
            return cmd.Key == Cmd;
        }

        protected override IOzackBehaviourGenerater<Data> DoCreate()
        {
            return new SetTextBehaviour();
        }
    }

    public static IOzackBehaviourFactory<Data> Factory() => new BehaviourFactory();

    //======================================
    // 変数
    //======================================
    private string m_message = string.Empty;

    //======================================
    // method
    //======================================

    protected override void DoBuild(Data cmd)
    {
        m_message = string.Join(Environment.NewLine, cmd.Values);

    }


    protected override void DoBegin()
    {
        Context.Get<UnityEngine.UI.Text>().text = m_message;
    }
}


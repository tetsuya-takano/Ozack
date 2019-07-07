using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ozack;
using System.Linq;
using UnityEngine.UI;

public class Sample : MonoBehaviour
{
   


    [SerializeField] private TextAsset m_script = null;
    [SerializeField] private Text m_text = null;

    // Start is called before the first frame update
    void Start()
    {
        // ファイルンの解釈の仕方
        var format = new Formatter(
            new string[] { "|" }, 
            System.StringSplitOptions.None
        );
        // コマンドとして解釈する文字列
        var table = new HashSet<string>
        {
            "SetText","bbb",
        };
        // 引数からコマンドデータへの変換方法
        var cmdBuilder = new OzackCommandBuilder<Data>(table, args => new Data( args ));
        // コマンドへ変換
        var system = new OzackParseSystem<Data>(format, cmdBuilder);

        //
        var context = new OzackContext();
        context.Add("txt", m_text);

        var commands = system.Parse( m_script.text );
        var builder = new OzackBehaviourBuilder<Data>( 
            context, 
            SetTextBehaviour.Factory()
        );
        var behaviours 
            = commands
                .Select(cmd => builder.Build(cmd))
                .ToArray();


        foreach (var b in behaviours)
        {
            b.Begin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

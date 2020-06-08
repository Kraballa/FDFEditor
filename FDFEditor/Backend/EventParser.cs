using System;
using System.Collections.Generic;
using System.Text;

namespace FDFEditor.Backend
{
    public static class EventParser
    {

        private static Dictionary<string, string> commands = new Dictionary<string, string>()
        {
            {"且","and" },
            {"或","or" },
            {"变化到","change to" },
            {"增加","increase" },
            {"减少","reduce" },
            {"恢复","resume" }, //resume/recover maybe?
            {"发射","shoot" }, //launch/shoot ?
            {"新事件组","new event group" },
            {"当前","current" },
            {"帧","frame" },
            {"角度","angle" },
            {"正比","relative" },
            {"变化","change" }, //google said variety so maybe random?
            {"到自机","to player" },
            {"子弹","danmaku" },
            {"速度","speed" },
            {"变化到","change to" }
        };

        public static string Parse(string @event)
        {
            foreach (KeyValuePair<string, string> kv in commands)
            {
                @event.Replace(kv.Key, kv.Value);
            }
            return @event;
        }

        public static string Encode(string @event)
        {
            foreach (KeyValuePair<string, string> kv in commands)
            {
                @event.Replace(kv.Value, kv.Key);
            }
            return @event;
        }
    }
}

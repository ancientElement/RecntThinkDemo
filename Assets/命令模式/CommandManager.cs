using System.Collections.Generic;
using UnityEngine;

public static class CommandManager
{
    private static Stack<BaseCommand> m_commands = new Stack<BaseCommand>();
    public static void Clear() { m_commands.Clear(); }
    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="command"></param>
    /// <param name="obj"></param>
    public static void Excute(BaseCommand command, GameObject obj)
    {
        command.Execute(obj);
        m_commands.Push(command);
    }
    /// <summary>
    /// 回退命令
    /// </summary>
    /// <param name="obj"></param>
    public static void Revock(GameObject obj)
    {
        if (m_commands.TryPop(out BaseCommand commmand))
        {
            commmand.Revoke(obj);
        }
    }
}

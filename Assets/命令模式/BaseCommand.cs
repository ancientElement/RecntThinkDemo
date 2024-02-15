using UnityEngine;

public interface BaseCommand
{
    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="command"></param>
    /// <param name="obj"></param>
    public void Execute(GameObject gameObject);
    /// <summary>
    /// 回退命令
    /// </summary>
    /// <param name="obj"></param>
    public void Revoke(GameObject gameObject);
}

public class MoveCommand : BaseCommand
{
    private GameObject m_gameObject;
    private Vector3 m_direction;

    public MoveCommand(Vector3 dir)
    {
        m_direction = dir;
    }

    public void Execute(GameObject obj)
    {
        m_gameObject = obj;
        m_gameObject.transform.Translate(m_direction);
    }

    public void Revoke(GameObject obj)
    {
        m_gameObject = obj;
        m_gameObject.transform.Translate(Quaternion.Euler(0f, 180f, 0f) * m_direction);
    }
}
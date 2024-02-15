using UnityEngine;

public class TestCommandMode : MonoBehaviour
{
    private MoveCommand m_moveForwad;
    private MoveCommand m_moveBack;
    private MoveCommand m_moveLeft;
    private MoveCommand m_moveRight;

    private void Start()
    {
        m_moveForwad = new MoveCommand(Vector3.forward);
        m_moveBack = new MoveCommand(Vector3.back);
        m_moveLeft = new MoveCommand(Vector3.left);
        m_moveRight = new MoveCommand(Vector3.right);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CommandManager.Excute(m_moveForwad, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CommandManager.Excute(m_moveBack, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CommandManager.Excute(m_moveLeft, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            CommandManager.Excute(m_moveRight, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CommandManager.Revock(gameObject);
        }
    }
}

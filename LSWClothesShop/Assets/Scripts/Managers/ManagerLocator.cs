using Controllers;
using Managers;

public class ManagerLocator
{
    public WardroveManager WardroveManager;
    public UIManager UImanager;
    public PlayerController PlayerController;
    
    public static ManagerLocator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ManagerLocator();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    private static ManagerLocator _instance;

    public ManagerLocator()
    {
        
    }
}

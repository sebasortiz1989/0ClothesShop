public class ManagerLocator
{
    public ShopManager shopManager;
    public UIManager uImanager;
    
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

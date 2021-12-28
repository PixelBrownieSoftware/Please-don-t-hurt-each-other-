using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
//Code inspired from https://github.com/YousicianGit/UnityMenuSystem/blob/e6ff71e7287482e826344349d836abeccf918882/Assets/Scripts/MenuSystem/MenuManager.cs#L60
public class s_MenuManager : MonoBehaviour
{
    public men_ActionMenu actionMenu;
    public men_Target targetMenu;
    public men_BattleOptions battleMenu;
    public men_SelectStage selectMenu;
    public men_goals goalsMenu;

    private Stack<s_Menu> menuStack = new Stack<s_Menu>();

    public static s_MenuManager instance { get; set; }

    private void Start()
    {
        men_SelectStage.Appear();
    }

    private void Awake()
    {
        instance = this;
    }

    public void CreateMenu<T>() where T : s_Menu
    {
        var prefab = GetMenuPrefab<T>();
        Instantiate(prefab,transform);
    }

    private T GetMenuPrefab<T>() where T : s_Menu
    {
        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if (prefab != null)
            {
                return prefab;
            }
        }
        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    public void OpenMenu(s_Menu menuInstance)
    {
        if (menuStack.Count > 0)
        {
            if (menuInstance.DisableMenusUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    if(menu!= null)
                        menu.gameObject.SetActive(false);
                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }
        }
        menuStack.Push(menuInstance);
    }
    public void CloseMenu(s_Menu menuInstance)
    {
        if (menuStack.Count == 0)
        {
            return;
        }
        if (menuStack.Peek() != menuInstance)
        {
            return;
        }
        CloseTopMenu();
    }


    public void CloseTopMenu() {
        var instance = menuStack.Pop();
        if (instance.DestroyWhenClosed)
        {
            Destroy(instance.gameObject);
        }
        else
            instance.gameObject.SetActive(false);

        foreach (var menu in menuStack)
        {
            if(menu != null)
                menu.gameObject.SetActive(true);
            if (menu.DisableMenusUnderneath)
                break;
        }
    }

    /*
    private void Update()
    {
        // On Android the back button is sent as Esc
        if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0)
        {
            menuStack.Peek().OnBackPressed();
        }
    }
    */

}

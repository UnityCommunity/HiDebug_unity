﻿//****************************************************************************
// Description:hidebug's view logic
// Author: hiramtan@live.com
//****************************************************************************

#define HiDebug_CurrentMouse

using UnityEngine;
public partial class HiDebugView : MonoBehaviour
{
    private static float _buttonWidth = 0.2f;
    private static float _buttonHeight = 0.1f;
    private static float _panelHeight = 0.7f;//0.3 is for stack
    private enum EDisplay
    {
        Button,//switch button
        Panel,//log panel
    }
    private EDisplay _eDisplay = EDisplay.Button;

    void OnGUI()
    {
        Button();
        Panel();
    }
}


public partial class HiDebugView : MonoBehaviour
{
    private enum EMouse
    {
        Up,
        Down,
    }
    private readonly float _mouseClickTime = 0.2f;//less than this is click
    private EMouse _eMouse = EMouse.Up;
    private float _mouseDownTime;
    Rect _rect = new Rect(0, 0, Screen.width * _buttonWidth, Screen.height * _buttonHeight);
    void Button()
    {
        if (_eDisplay != EDisplay.Button)
        {
            return;
        }
        if (_rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown)
            {
                _eMouse = EMouse.Down;
                _mouseDownTime = Time.realtimeSinceStartup;
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                _eMouse = EMouse.Up;
                if (Time.realtimeSinceStartup - _mouseDownTime < _mouseClickTime)//click
                { _eDisplay = EDisplay.Panel; }
            }
        }
        if (_eMouse == EMouse.Down && Event.current.type == EventType.mouseDrag)
        {
            _rect.x = Event.current.mousePosition.x - _rect.width / 2f;
            _rect.y = Event.current.mousePosition.y - _rect.height / 2f;
        }
        GUI.Button(_rect, "On");
    }
}

public partial class HiDebugView : MonoBehaviour
{
    void Panel()
    {
        if (_eDisplay != EDisplay.Panel)
            return;
        GUI.Window(0, new Rect(0, 0, Screen.width, Screen.height * _panelHeight), LogWindow, "HiDebug");
        GUI.Window(1, new Rect(0, Screen.height * _panelHeight, Screen.width, Screen.height * (1 - _panelHeight)), StackWindow, "Stack");
    }

    private bool _isLogOn = true;
    private bool _isWarnningOn = true;
    private bool _isErrorOn = true;
    private Vector2 _scrollLogPosition;
    void LogWindow(int windowID)
    {
        if (GUI.Button(new Rect(0, 0, Screen.width * _buttonWidth, Screen.height * _buttonHeight), "Clear"))
        {

        }
        if (GUI.Button(new Rect(Screen.width * (1 - _buttonWidth), 0, Screen.width * _buttonWidth, Screen.height * _buttonHeight), "Close"))
        {
            _eDisplay = EDisplay.Button;
        }
        var headHeight = GUI.skin.window.padding.top;//height of head
        var logStyle = GetGUIStype(new GUIStyle(GUI.skin.toggle), Color.white);
        _isLogOn = GUI.Toggle(new Rect(Screen.width * 0.3f, headHeight, Screen.width * _buttonWidth, Screen.height * _buttonHeight - headHeight), _isLogOn, "Log", logStyle);
        var WarnningStyle = GetGUIStype(new GUIStyle(GUI.skin.toggle), Color.yellow);
        _isWarnningOn = GUI.Toggle(new Rect(Screen.width * 0.5f, headHeight, Screen.width * _buttonWidth, Screen.height * _buttonHeight - headHeight), _isWarnningOn, "Warnning", WarnningStyle);
        var errorStyle = GetGUIStype(new GUIStyle(GUI.skin.toggle), Color.red);
        _isErrorOn = GUI.Toggle(new Rect(Screen.width * 0.7f, headHeight, Screen.width * _buttonWidth, Screen.height * _buttonHeight - headHeight), _isErrorOn, "Error", errorStyle);



        GUILayout.Space(Screen.height * _buttonHeight - headHeight);
        _scrollLogPosition = GUILayout.BeginScrollView(_scrollLogPosition);
        TestButton();
        GUILayout.EndScrollView();
    }

    void TestButton()
    {
        for (int i = 0; i < 20; i++)
            GUILayout.Button("sdfaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
    }




    private Vector2 _scrollStackPosition;
    void StackWindow(int windowID)
    {
        _scrollStackPosition = GUILayout.BeginScrollView(_scrollStackPosition);
        TestButton();
        GUILayout.EndScrollView();
    }

    GUIStyle GetGUIStype(GUIStyle guiStyle, Color color)
    {
        guiStyle.normal.textColor = color;
        guiStyle.hover.textColor = color;
        guiStyle.active.textColor = color;
        guiStyle.onNormal.textColor = color;
        guiStyle.onHover.textColor = color;
        guiStyle.onActive.textColor = color;
        return guiStyle;
    }
}
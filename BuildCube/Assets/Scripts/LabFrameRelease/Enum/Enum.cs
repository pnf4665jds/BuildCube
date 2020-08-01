


/// <summary>
/// 游戏语言类别
/// </summary>
public enum Language
{
    English,
    简体,
    繁体
}
/// <summary>
/// 提醒方式
/// </summary>
public enum RemindType
{
    Text,
    Voice,
    TextVoice
}

/// <summary>
/// 游戏运行的平台
/// </summary>
public enum GamePlantType
{
    Pc,
    Vr
}


public enum HandType
{
    LeftHand,
    RightHand,
    Hands
}

/// <summary>
/// 遊戲難度
/// </summary>
public enum Difficult
{
    Easy,
    Normal,
    Hard
}

/// <summary>
/// 操作模式
/// </summary>
public enum Mode
{
    Edit,   // 編輯模式，可添加方塊
    Delete  // 刪除模式，可移除方塊
}

/// <summary>
/// 遊戲狀態
/// </summary>
public enum GameState
{
    Prepare,    // 準備中
    Running,    // 運行中
    Finish      // 結束
}


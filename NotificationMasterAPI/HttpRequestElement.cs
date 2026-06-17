namespace NotificationMaster;

public class HttpRequestElement
{
    /// <summary>
    /// 0: get
    /// 1: post
    /// 2: json post
    /// </summary>
    public int Type = 0;
    public string URI = "";
    public string Content = "";

    public override string ToString()
    {
        return $"[{Type}/{URI}/{Content}]";
    }
}

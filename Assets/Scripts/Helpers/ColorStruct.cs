using UnityEngine;

[System.Serializable]
public struct ColorStruct
{
    [Range(0f, 255f)] public float m_Red;
    [Range(0f, 255f)] public float m_Green;
    [Range(0f, 255f)] public float m_Blue;
    [Range(0f, 1f)] public float m_Alpha;

    public ColorStruct(float r, float g, float b, float a)
    {
        m_Red = r;
        m_Green = g;
        m_Blue = b;
        m_Alpha = a;
    }

    /// <summary>
    /// Converts member variables with standard RGBA to Unity float values
    /// </summary>
    /// <returns>Returns a Unity Color(Struct / Value) with converted RGBA values</returns>
    public Color ToColor()
    {
        Color color = new Color();
        color.r = m_Red / 255f;
        color.g = m_Green / 255f;
        color.b = m_Blue / 255f;
        color.a = m_Alpha;
        return color;
    }
}

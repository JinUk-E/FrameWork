using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mine.Framework.Extension
{
    static class EnumerableEx
    {
        #region Public
        
        public static void SetTextValueOrClear(this IEnumerable<TextMeshProUGUI> sources, string text="")
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            foreach (var item in sources) item.SetText(text);
        }
        public static void SetTextValues(this IEnumerable<TextMeshProUGUI> sources, string[] text)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            if (text is null) throw new AggregateException(nameof(text));
            var textMeshProList = sources as TextMeshProUGUI[] ?? sources.ToArray();
            if (textMeshProList.Count() != text.Length) throw new ArgumentException("The length of 'sources' and 'text' must be equal.");
            
            var idx = 0;
            foreach (var item in textMeshProList)
            {
                item.SetText(text[idx]);
                idx++;
            }
        }
        public static void ColorSetting(this IEnumerable<Graphic> sources, Color color) 
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            foreach (var item in sources) item.color = color;
        }

        public static void ColliderSetting(this IEnumerable<Collider> sources, bool isActivated = false)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            foreach (var item in sources) item.enabled = isActivated;
        }
        
        public static void SwitchGameObject(this IEnumerable<GameObject> sources, bool isActivated = false)
        {
            if (sources is null) throw new ArgumentNullException(nameof(sources));
            foreach (var item in sources) item.SetActive(isActivated);
        }

        public static string StringJoin(this IEnumerable<string> source, string delimiter =",")
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            var strBuilder = new System.Text.StringBuilder();
            foreach (var item in source)
            {
                strBuilder.Append(item);
                strBuilder.Append(delimiter);
            }
            return strBuilder.ToString();
        }
        #endregion
    }

    static class StringEx
    {
        // string setting for Log and TextMeshPro
        public static string Color(this string text,Color color) => $"<color=#{color.ToHexString()}>{text}</color>";
        public static string Bold(this string text) => $"<b>{text}</b>";
        public static string Italic(this string text) => $"<i>{text}</i>";
        public static string Size(this string text, int size) => $"<size={size}>{text}</size>";
        public static string Underline(this string text) => $"<u>{text}</u>";
        public static string Strikethrough(this string text) => $"<s>{text}</s>";
        public static string Subscript(this string text) => $"<sub>{text}</sub>";
        public static string Superscript(this string text) => $"<sup>{text}</sup>";
        public static string Indent(this string text, int indent) => $"<indent={indent}>{text}</indent>";
        public static string LineHeight(this string text, int lineHeight) => $"<line-height={lineHeight}>{text}</line-height>";
        public static string LineSpacing(this string text, int lineSpacing) => $"<line-spacing={lineSpacing}>{text}</line-spacing>";
        public static string Font(this string text, string fontName) => $"<font=\"{fontName}\">{text}</font>";
        public static string FontStyle(this string text, string fontStyle) => $"<style=\"{fontStyle}\">{text}</style>";
    }
    static class GraphicEx
    {
        public static void SetColor(this Graphic source, Color color)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            source.color = color;
        }

        public static void SetMaterial(this Graphic source, Material material)
        {
            if (source is null) throw new ArgumentException(nameof(source));
            source.material = material;
        }
    }
    
    static class ColorEx
    {
        #region Public
        
        /// <summary>
        /// Return to HexCode from Color
        /// </summary>
        /// <param name="color"> origin Color is Color</param>
        /// <returns></returns>
        public static string ToHexString(this Color color) => $"{(byte)(color.r*255):X2}{(byte)(color.g*255):X2}{(byte)(color.b*255):X2}{(byte)(color.a*255):X2}";
        
        /// <summary>
        /// <para>Solid Purple. RGBA is (1, 0, 1, 1).</para>
        /// </summary>
        public static Color Purple { get; } = new Color(1f, 0f, 1f, 1f);

        /// <summary>
        /// <para>Solid LightGray. RGBA is (0.745, 0.745, 0.745, 1).</para>
        /// </summary>
        public static Color LightGray { get; } = new Color(.745f, .745f, .745f, 1f);

        /// <summary>
        /// <para>Solid LightBlue. RGBA is (0.117, 0.5, 0.8, 1).</para>
        /// </summary>
        public static Color LightBlue { get; } = new Color(0.117f, .5f, .8f, 1f);

        /// <summary>
        /// <para>Solid LightGreen. RGBA is (0, 1, 0.5, 1).</para>
        /// </summary>
        public static Color LightGreen { get; } = new Color(0f, 1f, .5f, 1f);
        
        /// <summary>
        /// <para>Solid LightGreen. RGBA is (0.1792453, 0.1792453, 0.1792453, 1).</para>
        /// </summary>
        public static Color LightBlack { get; } = new Color(0.1792453f, 0.1792453f, 0.1792453f, 1f);
        
        /// <summary>
        /// <para>Solid LightGreen. RGBA is (0.5, 1, 0.5, 1).</para>
        /// </summary>
        public static Color HighLightGreen { get; } = new Color(0.5f, 1f, 0.5f,1f);
        
        /// <summary>
        /// <para>Solid LightGreen. RGBA is (0.5, 1, 0.5, 0.7).</para>
        /// </summary>
        public static Color HighLightGreenTransparency { get; } = new Color(0.5f, 1f, 0.5f, 0.7f);

        /// <summary>
        /// <para>Solid LightGreen. RGBA is (1, 1, 1, 0).</para>
        /// </summary>
        public static Color WhiteTransparency { get; } = new Color(1, 1, 1, 0f);
        #endregion
    }
    
    /// <summary>
    /// Transform Extension Class
    /// </summary>
    static class TransformEx
    {
        /// <summary>
        /// Set The LocalLocation for Transform 
        /// </summary>
        /// <param name="target"> target Transform </param>
        /// <param name="position"> location of target Transform </param>
        /// <param name="rotation"> rotation of target Transform </param>
        public static void SetLocalTransform(this Transform target, Vector3 position, Quaternion rotation)
        {
            target.localPosition = position;
            target.localRotation = rotation;
            target.localScale = Vector3.one;
        }
        
        /// <summary>
        /// Set the center of a RectTransform to match the center of its parent RectTransform and stretch it to fill the parent RectTransform.
        /// </summary>
        /// <param name="childTransform">The child RectTransform to set.</param>
        public static async UniTask SetCenterAndFillParent(this Transform childTransform)
        { 
            if (childTransform is not RectTransform childRectTransform) return;
            childRectTransform.anchorMin = Vector2.zero;
            childRectTransform.anchorMax = Vector2.one;
            childRectTransform.sizeDelta = Vector2.zero;
            childRectTransform.localScale = Vector3.one;
            await UniTask.Yield();
            childRectTransform.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Setting the x-axis for the translation location
        /// Default Drop X value 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="x"></param>
        public static void SetOrDropX(this Transform transform, float x = 0)
        {
            var position = transform.position;
            position = new Vector3(x, position.y, position.z);
            transform.position = position;
        }

        /// <summary>
        /// Setting the y-axis for the translation location
        /// Default Drop Y value 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="y"></param>
        public static void SetOrDropY(this Transform transform, float y = 0)
        {
            var position = transform.position;
            position = new Vector3(position.x, y, position.z);
            transform.position = position;
        } 
        
        /// <summary>
        /// Setting the z-axis for the translation location
        /// Default Drop Z value 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="z"></param>
        public static void SetOrDropZ(this Transform transform, float z = 0)
        {
            var position = transform.position;
            position = new Vector3(position.x,position.y,z);
            transform.position = position;
        } 
    }
    
    /// <summary>
    /// Math Extension Class
    /// </summary>
    static class MathEx
    {
        #region public
        
        // vector2 is ref type
        public static void SetX(this ref Vector2 source, float x) => source.x = x;
        public static void SetY(this ref Vector2 source, float y) => source.y = y;
        
        // vector3 is value type
        public static Vector3 SetX(this Vector3 source, float x)
        {
            source.x = x;
            return source;
        }

        public static Vector3 SetY(this Vector3 source,float y)
        {
            source.y = y;
            return source;
        }

        public static Vector3 SetZ(this Vector3 source,float z)
        {
            source.z = z;
            return source;
        }

        #endregion
       
    }
    
    static class RayCastEx
    {
        #region public

        /// <summary>
        /// return The Object for hitObject
        /// </summary>
        /// <param name="hit"> RayCast Target Object Info </param>
        /// <typeparam name="T"> Find Type of TargetObject </typeparam>
        /// <returns> is Same to "T" </returns>
        public static T HitObjectAndFind<T>(this RaycastHit hit) => hit.transform.GetComponent<T>();
        #endregion
    }
}

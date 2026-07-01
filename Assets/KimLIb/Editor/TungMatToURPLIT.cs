#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace KimLIb.Editor
{
    public static class TungMaterialToURPLit
    {
        [MenuItem("Tools/Render/Convert Selected To URP Lit")]
        private static void ConvertSelected()
        {
            Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");

            if (urpLit == null)
            {
                Debug.LogError("URP Lit Shader를 찾지 못했습니다.");
                return;
            }

            foreach (Object selected in Selection.objects)
            {
                if (selected is not Material material)
                    continue;

                Convert(material, urpLit);
            }

            AssetDatabase.SaveAssets();
            Debug.Log("선택한 Material의 URP Lit 변환이 완료되었습니다.");
        }

        private static void Convert(Material material, Shader urpLit)
        {
            Undo.RecordObject(material, "Convert Material To URP Lit");

            // 기존 셰이더 값 백업
            Texture baseMap = GetTexture(material, "_BaseMap", "_MainTex", "_Albedo");
            Color baseColor = GetColor(material, Color.white,
                "_BaseColor", "_Color", "_TintColor");

            Vector2 textureScale = GetTextureScale(material, "_BaseMap", "_MainTex");
            Vector2 textureOffset = GetTextureOffset(material, "_BaseMap", "_MainTex");

            Texture normalMap = GetTexture(material, "_BumpMap", "_NormalMap");
            float normalScale = GetFloat(material, 1f, "_BumpScale", "_NormalScale");

            Texture metallicMap = GetTexture(material, "_MetallicGlossMap");
            float metallic = GetFloat(material, 0f, "_Metallic");
            float smoothness = GetFloat(material, 0.5f,
                "_Smoothness", "_Glossiness");

            Texture occlusionMap = GetTexture(material, "_OcclusionMap");
            float occlusionStrength = GetFloat(material, 1f, "_OcclusionStrength");

            Texture emissionMap = GetTexture(material, "_EmissionMap");
            Color emissionColor = GetColor(material, Color.black, "_EmissionColor");

            float cutoff = GetFloat(material, 0.5f, "_Cutoff");
            int renderQueue = material.renderQueue;

            // Shader 변경
            material.shader = urpLit;

            // URP Lit 슬롯에 복구
            material.SetTexture("_BaseMap", baseMap);
            material.SetColor("_BaseColor", baseColor);
            material.SetTextureScale("_BaseMap", textureScale);
            material.SetTextureOffset("_BaseMap", textureOffset);

            material.SetTexture("_BumpMap", normalMap);
            material.SetFloat("_BumpScale", normalScale);

            material.SetTexture("_MetallicGlossMap", metallicMap);
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Smoothness", smoothness);

            material.SetTexture("_OcclusionMap", occlusionMap);
            material.SetFloat("_OcclusionStrength", occlusionStrength);

            material.SetTexture("_EmissionMap", emissionMap);
            material.SetColor("_EmissionColor", emissionColor);
            material.SetFloat("_Cutoff", cutoff);

            if (normalMap != null)
                material.EnableKeyword("_NORMALMAP");

            if (metallicMap != null)
                material.EnableKeyword("_METALLICSPECGLOSSMAP");

            if (emissionMap != null || emissionColor.maxColorComponent > 0f)
            {
                material.EnableKeyword("_EMISSION");
                material.globalIlluminationFlags =
                    MaterialGlobalIlluminationFlags.BakedEmissive;
            }

            if (renderQueue >= 0)
                material.renderQueue = renderQueue;

            EditorUtility.SetDirty(material);
        }

        private static Texture GetTexture(Material material, params string[] names)
        {
            foreach (string name in names)
            {
                if (material.HasProperty(name))
                    return material.GetTexture(name);
            }

            return null;
        }

        private static float GetFloat(
            Material material, float fallback, params string[] names)
        {
            foreach (string name in names)
            {
                if (material.HasProperty(name))
                    return material.GetFloat(name);
            }

            return fallback;
        }

        private static Color GetColor(
            Material material, Color fallback, params string[] names)
        {
            foreach (string name in names)
            {
                if (material.HasProperty(name))
                    return material.GetColor(name);
            }

            return fallback;
        }

        private static Vector2 GetTextureScale(
            Material material, params string[] names)
        {
            foreach (string name in names)
            {
                if (material.HasProperty(name))
                    return material.GetTextureScale(name);
            }

            return Vector2.one;
        }

        private static Vector2 GetTextureOffset(
            Material material, params string[] names)
        {
            foreach (string name in names)
            {
                if (material.HasProperty(name))
                    return material.GetTextureOffset(name);
            }

            return Vector2.zero;
        }
    }
}
#endif
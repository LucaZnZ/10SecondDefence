using UnityEngine;

namespace TGS {

    static class ShaderParams {
        public static int FadeAmount = Shader.PropertyToID("_FadeAmount");
        public static int Scale = Shader.PropertyToID("_Scale");
        public static int Offset = Shader.PropertyToID("_Offset");
        public static int ZWrite = Shader.PropertyToID("_ZWrite");
        public static int SrcBlend = Shader.PropertyToID("_SrcBlend");
        public static int DstBlend = Shader.PropertyToID("_DstBlend");
        public static int Cull = Shader.PropertyToID("_Cull");
        public static int ZTest = Shader.PropertyToID("_ZTest");
        public static int StencilRef = Shader.PropertyToID("_StencilRef");
        public static int StencilComp = Shader.PropertyToID("_StencilComp");
        public static int StencilOp = Shader.PropertyToID("_StencilOp");
        public static int NearClip = Shader.PropertyToID("_NearClip");
        public static int FallOff = Shader.PropertyToID("_FallOff");
        public static int FarFadeDistance = Shader.PropertyToID("_FarFadeDistance");
        public static int FarFadeFallOff = Shader.PropertyToID("_FarFadeFallOff");
        public static int Color = Shader.PropertyToID("_Color");
        public static int Color2 = Shader.PropertyToID("_Color2");
        public static int MainTex = Shader.PropertyToID("_MainTex");
        public static int Thickness = Shader.PropertyToID("_Thickness");
        public static int CircularFadePosition = Shader.PropertyToID("_CircularFadePosition");
        public static int CircularFadeDistanceSqr = Shader.PropertyToID("_CircularFadeDistanceSqr");
        public static int CircularFadeFallOff = Shader.PropertyToID("_CircularFadeFallOff");
        public static int HighlightBorderColor = Shader.PropertyToID("_HighlightBorderColor");
        public static int HighlightBorderSize = Shader.PropertyToID("_HighlightBorderSize");
    }

}
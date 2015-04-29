Shader "Transparent/InvisibleShadowCaster"
 {
     Subshader
     {
         UsePass "VertexLit/SHADOWCOLLECTOR"    
         UsePass "VertexLit/SHADOWCASTER"
     }
 
     Fallback off
 }
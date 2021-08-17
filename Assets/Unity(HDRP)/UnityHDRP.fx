///////////////////////////////////////////////////////////////////////
//  UnityHDRP.fx
//
//
//  *** INTERACTIVE DATA VISUALIZATION (IDV) CONFIDENTIAL AND PROPRIETARY INFORMATION ***
//
//  This software is supplied under the terms of a license agreement or
//  nondisclosure agreement with Interactive Data Visualization, Inc. and
//  may not be copied, disclosed, or exploited except in accordance with
//  the terms of that agreement.
//
//      Copyright (c) 2003-2019 IDV, Inc.
//      All rights reserved in all media.
//
//      IDV, Inc.
//      Web: http://www.speedtree.com
///////////////////////////////////////////////////////////////////////
//  TexturePacking
void TexturePacking(STexturePackingInput sInput, inout STexturePackingOutput sOutput)
{
    // add dithering to opacity for fast blending
    if (sInput.m_bBillboards)
    {
        sInput.m_fOpacity *= (sInput.m_fNoise * 2.0 + 1.0) / 3.0;
    }
    else
    {
        sInput.m_fOpacity *= sInput.m_fNoise * 0.5 + 0.5;
    }
    // color/opacity
    sOutput.m_vTexture0 = float4(LinearTosRgb(sInput.m_vColor), sInput.m_fOpacity);
    // normal
    if (sInput.m_vNormal.z < 0.5)
    {
        // normals are later compressed into a format that only points out. Try to avoid artifacts by clamping
        sInput.m_vNormal = sInput.m_vNormal * 2.0 - 1.0;
        sInput.m_vNormal.z = 0.0;
        sInput.m_vNormal = normalize(sInput.m_vNormal) * 0.5 + 0.5;
    }
    sOutput.m_vTexture1.rgb = sInput.m_vNormal;
    // mask (metal, ao, detail, gloss) (matches the mask in HDRP Lit)
    sOutput.m_vTexture2 = float4(sInput.m_fMetallic, sInput.m_fAmbientOcclusion, 0.0, sInput.m_fGloss * sInput.m_fGloss);
    // subsurface
    if (sInput.m_bTwoSided || sInput.m_bBillboards)
    {
        sOutput.m_vTexture3.r = 1.0 - Luminance(sInput.m_vSubsurfaceColor * sInput.m_fSubsurfaceAmount);  
    }
}

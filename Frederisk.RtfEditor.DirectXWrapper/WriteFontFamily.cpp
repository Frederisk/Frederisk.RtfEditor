#include "pch.h"
#include "WriteFontFamily.h"

using namespace Frederisk_RtfEditor_DirectXWrapper;
using namespace Platform;
// using namespace Microsoft::WRL;
using namespace Windows::UI::Text;

WriteFontFamily::WriteFontFamily(Microsoft::WRL::ComPtr<IDWriteFontFamily> pFontFamily) {
    this->pFontFamily = pFontFamily;
}

WriteLocalizedStrings^ WriteFontFamily::GetFamilyNames() {
    Microsoft::WRL::ComPtr<IDWriteLocalizedStrings> pFamilyNames;
    Check(pFontFamily->GetFamilyNames(&pFamilyNames));
    return ref new WriteLocalizedStrings(pFamilyNames);
}

WriteFont^ WriteFontFamily::GetFirstMatchingFont(FontWeight fontWeight, FontStretch fontStretch, FontStyle fontStyle) {
    DWRITE_FONT_WEIGHT writeFontWeight = DWRITE_FONT_WEIGHT_NORMAL;
    if (fontWeight.Equals(FontWeights::Black)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_BLACK;
    }
    else if (fontWeight.Equals(FontWeights::Bold)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_BOLD;
    }
    else if (fontWeight.Equals(FontWeights::ExtraBlack)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_EXTRA_BLACK;
    }
    else if (fontWeight.Equals(FontWeights::ExtraBold)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_EXTRA_BOLD;
    }
    else if (fontWeight.Equals(FontWeights::ExtraLight)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_EXTRA_LIGHT;
    }
    else if (fontWeight.Equals(FontWeights::Light)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_LIGHT;
    }
    else if (fontWeight.Equals(FontWeights::Medium)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_MEDIUM;
    }
    else if (fontWeight.Equals(FontWeights::Normal)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_NORMAL;
    }
    else if (fontWeight.Equals(FontWeights::SemiBold)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_SEMI_BOLD;
    }
    else if (fontWeight.Equals(FontWeights::SemiLight)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_SEMI_LIGHT;
    }
    else if (fontWeight.Equals(FontWeights::Thin)) {
        writeFontWeight = DWRITE_FONT_WEIGHT_THIN;
    }

    DWRITE_FONT_STRETCH writeFontStretch = (DWRITE_FONT_STRETCH)fontStretch;
    DWRITE_FONT_STYLE writeFontStyle = (DWRITE_FONT_STYLE)fontStyle;
    Microsoft::WRL::ComPtr<IDWriteFont> pWriteFont = nullptr;
    Check(pFontFamily->GetFirstMatchingFont(writeFontWeight, writeFontStretch, writeFontStyle, &pWriteFont));
    return ref new WriteFont(pWriteFont);
}
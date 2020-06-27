#include "pch.h"
#include "WriteFont.h"

using namespace Frederisk_RtfEditor_DirectXWrapper;
using namespace Platform;
using namespace Microsoft::WRL;

WriteFont::WriteFont(ComPtr<IDWriteFont> pWriteFont) {
    this->pWriteFont = pWriteFont;
}

WriteFontMetrics WriteFont::GetMetrics() {
    DWRITE_FONT_METRICS fontMetrics;
    this->pWriteFont->GetMetrics(&fontMetrics);
    WriteFontMetrics writeFontMetrics = WriteFontMetrics{
        fontMetrics.designUnitsPerEm,
        fontMetrics.ascent,
        fontMetrics.descent,
        fontMetrics.lineGap,
        fontMetrics.capHeight,
        fontMetrics.xHeight,
        fontMetrics.underlinePosition,
        fontMetrics.underlineThickness,
        fontMetrics.strikethroughPosition,
        fontMetrics.strikethroughThickness
    };
    return writeFontMetrics;
}

bool WriteFont::HasCharacter(UINT32 unicodeValue) {
    BOOL exists = 0;
    Check(pWriteFont->HasCharacter(unicodeValue, &exists));
    return exists != 0;
}

bool WriteFont::IsSymbolFont() {
    return this->pWriteFont->IsSymbolFont() != 0;
}
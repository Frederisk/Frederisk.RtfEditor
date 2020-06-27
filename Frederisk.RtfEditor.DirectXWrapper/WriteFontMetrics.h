#pragma once

namespace Frederisk_RtfEditor_DirectXWrapper {
    public value struct WriteFontMetrics {
        uint16 DesignUnitsPerEm;
        uint16 Ascent;
        uint16 Descent;
        int16 LineGap;
        uint16 CapHeight;
        uint16 XHeight;
        int16 UnderlinePosition;
        uint16 UnderlineThickness;
        int16 StrikethroughPosition;
        uint16 StrikethoughThickness;
    };
}
#include "pch.h"
#include "WriteFactory.h"

using namespace Frederisk_RtfEditor_DirectXWrapper;
using namespace Platform;
// using namespace Microsoft::WRL;
using namespace Microsoft::WRL;

WriteFactory::WriteFactory() {
    Check(DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED, __uuidof(IDWriteFactory), &pFactory));
}

WriteFontCollection^ WriteFactory::GetSystemFontCollection() {
    return GetSystemFontCollection(false);
}

WriteFontCollection^ WriteFactory::GetSystemFontCollection(bool checkForUpdates) {
    ComPtr<IDWriteFontCollection> pFontCollection;
    Check(pFactory->GetSystemFontCollection(&pFontCollection, checkForUpdates));
    return ref new WriteFontCollection(pFontCollection);
}
#include "pch.h"
#include "WriteFactory.h"

using namespace Frederisk_RtfEditor_DirectXWrapper;
using namespace Platform;
using namespace winrt;
// using namespace Microsoft::WRL;

WriteFactory::WriteFactory() {
    Check(DWriteCreateFactory(DWRITE_FACTORY_TYPE_SHARED, __uuidof(IDWriteFactory), (IUnknown**)(get_abi(pFactory.put()))));
}

WriteFontCollection^ WriteFactory::GetSystemFontCollection() {
    return GetSystemFontCollection(false);
}

WriteFontCollection^ WriteFactory::GetSystemFontCollection(bool checkForUpdates) {
    com_ptr<IDWriteFontCollection> pFontCollection;
    Check(pFactory->GetSystemFontCollection(pFontCollection.put(), checkForUpdates));
    return ref new WriteFontCollection(pFontCollection);
}
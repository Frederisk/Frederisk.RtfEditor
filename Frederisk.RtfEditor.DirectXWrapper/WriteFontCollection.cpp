#include "pch.h"
#include "WriteFontCollection.h"
#include "WriteFontFamily.h"

using namespace Frederisk_RtfEditor_DirectXWrapper;
using namespace Platform;
using namespace Microsoft::WRL;

WriteFontCollection::WriteFontCollection(ComPtr<IDWriteFontCollection> pFontCollection) {
    this->pFontCollection = pFontCollection;
}

bool WriteFontCollection::FindFamilyName(String^ familyName, int* index) {
    uint32 familyIndex;
    BOOL exists;
    Check(this->pFontCollection->FindFamilyName(familyName->Data(), &familyIndex, &exists));
    *index = familyIndex;
    return exists != 0;
}

int WriteFontCollection::GetFontFamilyCount() {
    return pFontCollection->GetFontFamilyCount();
}

WriteFontFamily^ WriteFontCollection::GetFontFamily(int index) {
    ComPtr<IDWriteFontFamily> pfontFamily;
    Check(pFontCollection->GetFontFamily(index, &pfontFamily));
    return ref new WriteFontFamily(pfontFamily);
}
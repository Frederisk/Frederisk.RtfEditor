using Windows.UI.Xaml.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frederisk_RtfEditor_DirectXWrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace Frederisk.RtfEditor.DirectXWrapper.Tests {

    [TestClass]
    public class UnitTest1 {

        [TestMethod]
        public void TestMethod1() {
            var writeFactory = new WriteFactory();
            // 獲得系統字體集合資訊。
            var writeFontCollection = writeFactory.GetSystemFontCollection();
            var count = writeFontCollection.GetFontFamilyCount();
            var fontFamilies = new FontFamily[count];

            for (var i = 0; i < count; i++) {
                var writeFontFamily = writeFontCollection.GetFontFamily(i);
                var writeLocalizedStrings = writeFontFamily.GetFamilyNames();
                // 以名稱實例化字體。
                fontFamilies[i] = new FontFamily(writeLocalizedStrings.GetString(0));
            }

            Assert.IsFalse(count < 0);
        }
    }
}
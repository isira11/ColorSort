// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("8XJ8c0Pxcnlx8XJyc+sGOE1GuRHXbCQwI/0dPcnXGmlFlFkcYWlPL8cxLsG3QyEhlg9F/MziIGbCkrd9CFjLPvGIo7nvuRj66PNUkjk8e5BD8XJRQ351eln1O/WEfnJycnZzcDuWD9kpDbgthB0ffZwU/qsobjXSIb9oIoSrtQOO2FxlHWatEfPPTADmeuC3XzNRqecIiiPwGR2HysjKITFh2OD/INDHxpH4gCD1y6sWzl3GNfZBVFEqmeKQ/mf3vClUZIXoaGd7qZgrBs//3UhnxCTrtTFc0QcOVA7gp6LXewSiQF/WD0NO4ji+/eVYoXVullgRIeAzoOGEWdmT4Qt8IoOcUwm7OIjtB2fw7OOnbNVmRpAFhA5m5Wb3xCFMjHFwcnNy");
        private static int[] order = new int[] { 1,9,12,3,9,10,8,8,11,12,11,13,12,13,14 };
        private static int key = 115;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
